import { fetchApi, API_ENDPOINTS, SchoolDetails, Department, Instructor } from '../../lib/api';

// Arama sonuç tipi
export interface SearchResult {
  type: 'department' | 'instructor';
  id: string | number;
  name: string;
  email?: string;
  imageUrl?: string;
  urlPath?: string;
  departmentName?: string; // Akademisyen için bölüm adı
}

// Arama sonuçları
export interface SearchResults {
  departments: SearchResult[];
  instructors: SearchResult[];
  query: string;
}

/**
 * Okul ID'sine göre arama yapan fonksiyon
 * @param schoolId Okul ID'si
 * @param query Arama sorgusu
 * @returns Arama sonuçları
 */
export async function searchBySchoolId(schoolId: number, query: string): Promise<SearchResults> {
  try {
    // Sorgu boşsa boş sonuç döndür
    if (!query.trim()) {
      return {
        departments: [],
        instructors: [],
        query: ''
      };
    }

    // API'den okul detaylarını al
    const schoolDetails = await fetchApi<SchoolDetails>(
      `${API_ENDPOINTS.SEARCH.INSTRUCTOR_SCHOOL_DETAILS}/${schoolId}/details`
    );

    // Arama sorgusunu küçük harfe çevir
    const normalizedQuery = query.toLowerCase().trim();

    // Bölümlerde ara
    const departmentResults = schoolDetails.departments
      .filter(dept => dept.name.toLowerCase().includes(normalizedQuery))
      .map(dept => ({
        type: 'department' as const,
        id: dept.id,
        name: dept.name
      }));

    // Bölüm ID'lerinin hızlı erişim için haritası
    const departmentMap = new Map<number | string, Department>();
    schoolDetails.departments.forEach(dept => {
      departmentMap.set(dept.id, dept);
    });

    // Akademisyenlerde ara
    const instructorResults = schoolDetails.instructors
      .filter((instructor: Instructor) => 
        instructor.userFullName.toLowerCase().includes(normalizedQuery) ||
        (instructor.email && instructor.email.toLowerCase().includes(normalizedQuery))
      )
      .map((instructor: Instructor) => {
        // Akademisyenin bölüm adını bul (varsa)
        let departmentName: string | undefined;
        if (instructor.departmentId && departmentMap.has(instructor.departmentId)) {
          departmentName = departmentMap.get(instructor.departmentId)?.name;
        }

        return {
          type: 'instructor' as const,
          id: instructor.id,
          name: instructor.userFullName,
          email: instructor.email,
          imageUrl: instructor.imageUrl,
          urlPath: instructor.urlPath,
          departmentName
        };
      });

    return {
      departments: departmentResults,
      instructors: instructorResults,
      query
    };
  } catch (error) {
    console.error('Arama sırasında hata oluştu:', error);
    throw error;
  }
}

/**
 * Akademisyen ve bölüm sonuçlarını birleştiren fonksiyon
 * @param results Arama sonuçları
 * @param limit Maksimum sonuç sayısı (isteğe bağlı)
 * @returns Birleştirilmiş arama sonuçları
 */
export function combineSearchResults(results: SearchResults, limit?: number): SearchResult[] {
  // Önce akademisyenleri, sonra bölümleri göster
  const combinedResults = [
    ...results.instructors,
    ...results.departments
  ];
  
  // Limit varsa, sonuçları sınırla
  return limit ? combinedResults.slice(0, limit) : combinedResults;
}

/**
 * Okul ID'sine göre arama yapan ve sonuçları birleştiren fonksiyon
 * @param schoolId Okul ID'si
 * @param query Arama sorgusu
 * @param limit Maksimum sonuç sayısı (isteğe bağlı)
 * @returns Birleştirilmiş arama sonuçları
 */
export async function searchAndCombineResults(
  schoolId: number, 
  query: string, 
  limit?: number
): Promise<SearchResult[]> {
  const results = await searchBySchoolId(schoolId, query);
  return combineSearchResults(results, limit);
}

/**
 * Test amaçlı - Konsola arama sonuçlarını yazdıran fonksiyon
 * @param schoolId Okul ID'si
 * @param query Arama sorgusu
 */
export async function testSearch(schoolId: number, query: string): Promise<void> {
  try {
    console.log(`"${query}" için arama yapılıyor...`);
    const results = await searchBySchoolId(schoolId, query);
    
    console.log('Arama sonuçları:', {
      query: results.query,
      departmentCount: results.departments.length,
      instructorCount: results.instructors.length
    });
    
    if (results.departments.length > 0) {
      console.log('Bölümler:');
      results.departments.forEach(dept => {
        console.log(`- ${dept.name} (ID: ${dept.id})`);
      });
    }
    
    if (results.instructors.length > 0) {
      console.log('Akademisyenler:');
      results.instructors.forEach(instructor => {
        console.log(`- ${instructor.name} (ID: ${instructor.id}, Bölüm: ${instructor.departmentName || 'Belirtilmemiş'})`);
      });
    }
  } catch (error) {
    console.error('Test sırasında hata:', error);
  }
}

export default {
  searchBySchoolId,
  combineSearchResults,
  searchAndCombineResults,
  testSearch
};

import { getDepartments, Department, getInstructorsByDepartment, Instructor, getInstructorById, getInstructorByUrlPath, getDepartmentsBySchool } from '../../lib/api';

// Typescript arayüzleri tanımlıyoruz
export interface Bolum {
  id: string;
  name: string;
  fakulte_id: string | null;
  url_path: string | null;
}

// API'den gelen akademisyen verilerini uygulamamızda kullanılabilir formata dönüştüren arayüz
export interface Akademisyen {
  id: string;
  fullName: string;
  bolum_id: string;
  profileImage: string | null;
  email: string | null;
  urlPath: string | null;
}

// API'den gelen departman bilgilerini uygulamamızdaki Bolum tipine dönüştüren fonksiyon
export const convertApiDepartmentToBolum = (department: Department): Bolum => {
  return {
    id: department.id.toString(),
    name: department.name,
    fakulte_id: null, // API'den fakulte_id gelmediği için null bırakıyoruz
    url_path: null
  };
};

// API'den gelen akademisyen verilerini dönüştüren fonksiyon
export const convertApiInstructorToAkademisyen = (instructor: Instructor, departmentId: string): Akademisyen => {
  return {
    id: instructor.id,
    fullName: instructor.userFullName,
    bolum_id: departmentId,
    profileImage: instructor.imageUrl,
    email: instructor.email,
    urlPath: instructor.urlPath
  };
};

/**
 * API'den bölümleri getirir
 */
export const getBolumlerFromApi = async (): Promise<Bolum[]> => {
  try {
    // API'den departmanları getir
    const departments = await getDepartments();
    
    // API'den gelen departmanları Bolum tipine dönüştür
    return departments.map(department => convertApiDepartmentToBolum(department));
  } catch (error) {
    console.error('API\'den bölümler getirilirken hata:', error);
    return [];
  }
};

/**
 * Belirli bir okula ait bölümleri getirir
 * @param schoolId Okul ID'si
 */
export const getBolumlerBySchool = async (schoolId: number): Promise<Bolum[]> => {
  try {
    // Eğer schoolId yoksa veya geçersizse, varsayılan olarak 1 kullan
    const validSchoolId = schoolId || 1;
    
    // API'den seçilen okula ait departmanları getir
    const departments = await getDepartmentsBySchool(validSchoolId);
    
    // API'den gelen departmanları Bolum tipine dönüştür
    return departments.map(department => convertApiDepartmentToBolum(department));
  } catch (error) {
    console.error(`${schoolId} ID'li okula ait bölümler getirilirken hata:`, error);
    return [];
  }
};

/**
 * Belirli bir ID'ye sahip bölümü getirir
 */
export const getBolumById = async (bolumId: string): Promise<Bolum | null> => {
  try {
    // API'den tüm bölümleri getir
    const bolumler = await getBolumlerFromApi();
    
    // Belirtilen ID'ye sahip bölümü bul
    const bolum = bolumler.find(b => b.id === bolumId);
    
    return bolum || null;
  } catch (error) {
    console.error(`${bolumId} ID'li bölüm getirilirken hata:`, error);
    return null;
  }
};

/**
 * Belirli bir bölüme ait akademisyenleri API'den getirir
 */
export const getAkademisyenlerByBolumId = async (bolumId: string): Promise<Akademisyen[]> => {
  try {
    // API'den akademisyenleri getir
    const instructors = await getInstructorsByDepartment(bolumId);
    
    // API'den gelen verileri dönüştür
    return instructors.map(instructor => convertApiInstructorToAkademisyen(instructor, bolumId));
  } catch (error) {
    console.error(`${bolumId} ID'li bölümün akademisyenleri getirilirken hata:`, error);
    return [];
  }
};

/**
 * ID'ye veya URL path'e göre akademisyen getir
 * @param idOrUrlPath - Akademisyen ID'si veya URL path'i
 */
export const getAkademisyenById = async (idOrUrlPath: string): Promise<Akademisyen | null> => {
  if (!idOrUrlPath) {
    console.error('getAkademisyenById fonksiyonuna geçersiz parametre:', idOrUrlPath);
    return null;
  }
  
  try {
    // SearchContext tarafından kaydedilen verileri kontrol et (recent_instructors)
    const searchContextData = localStorage.getItem('recent_instructors');
    if (searchContextData) {
      try {
        const searchResults = JSON.parse(searchContextData);
        // SearchResult dizisinden eşleşen akademisyeni bul
        const matchedInstructor = searchResults.find((item: any) => 
          (item.urlPath === idOrUrlPath) || (item.id === idOrUrlPath)
        );
        
        if (matchedInstructor) {
          console.log('Akademisyen verisi SearchContext localStorage\'dan alındı:', matchedInstructor);
          // SearchContext verisini Akademisyen formatına dönüştür
          const akademisyen: Akademisyen = {
            id: matchedInstructor.id.toString(),
            fullName: matchedInstructor.name,
            bolum_id: matchedInstructor.departmentId || "0",
            profileImage: matchedInstructor.imageUrl || null,
            email: matchedInstructor.email || null,
            urlPath: matchedInstructor.urlPath || null
          };
          
          // Güncel veriyi currentAkademisyen olarak da kaydet
          localStorage.setItem('currentAkademisyen', JSON.stringify(akademisyen));
          return akademisyen;
        }
      } catch (e) {
        console.warn('SearchContext localStorage verisi parse edilirken hata:', e);
      }
    }
    
    // Mevcut currentAkademisyen verisini kontrol et
    const storedData = localStorage.getItem('currentAkademisyen');
    if (storedData) {
      try {
        const parsedData = JSON.parse(storedData) as Akademisyen;
        if (parsedData.id === idOrUrlPath || parsedData.urlPath === idOrUrlPath) {
          console.log('Akademisyen verisi currentAkademisyen\'den alındı:', parsedData);
          return parsedData;
        }
      } catch (e) {
        console.warn('localStorage verisi parse edilirken hata:', e);
      }
    }
    
    // localStorage'da veri yoksa veya eşleşme sağlanmadıysa, API'ye istek yap
    let instructor: Instructor;
    
    // URL path mi yoksa ID mi olduğunu daha güvenli bir şekilde kontrol et
    // URL path genellikle kullanıcı adı formatında ve akademisyen ID'lerine göre daha kısadır
    // UUID formatında ID'ler genellikle uzun ve tire içerir
    if (idOrUrlPath.length < 36) {
      // Önce URL path olarak dene
      try {
        instructor = await getInstructorByUrlPath(idOrUrlPath);
      } catch (error) {
        // URL path olarak başarısız olursa ID olarak dene
        instructor = await getInstructorById(idOrUrlPath);
      }
    } else {
      // ID olarak değerlendir
      instructor = await getInstructorById(idOrUrlPath);
    }
    
    // API'den veri gelirse dönüştür ve döndür
    if (instructor) {
      // Bölüm ID'si bilinmediği için departmentId varsa onu kullan, yoksa 0
      const departmentId = instructor.departmentId ? instructor.departmentId.toString() : "0";
      const akademisyen = convertApiInstructorToAkademisyen(instructor, departmentId);
      
      // localStorage'a kaydet
      localStorage.setItem('currentAkademisyen', JSON.stringify(akademisyen));
      
      return akademisyen;
    }
    
    throw new Error(`${idOrUrlPath} için akademisyen verisi bulunamadı.`);
  } catch (error) {
    console.error(`${idOrUrlPath} ile akademisyen getirilirken hata:`, error);
    return null;
  }
}; 
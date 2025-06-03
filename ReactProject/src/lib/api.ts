// API temel URL'i - .env dosyasından alınıyor
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://34.90.200.240:90/api';

// API Endpoints
export const API_ENDPOINTS = {
  AUTH: {
    REGISTER: '/Auth/register',
    LOGIN: '/Auth/login',
    LOGOUT: '/Auth/logout',
    FORGOT_PASSWORD: '/Auth/forgotpassword',
    RESET_PASSWORD: '/Auth/resetpassword'
  },
  ADMIN: {
    SCHOOLS: '/admin/AdminSchool',
    SCHOOL_BY_ID: '/admin/AdminSchool',  // + /{id} şeklinde kullanılacak
    DEPARTMENTS: '/admin/AdminSchool/1/departments',
    DEPARTMENTS_BY_SCHOOL: '/admin/AdminSchool',  // + /{schoolId}/departments şeklinde kullanılacak
    INSTRUCTORS_BY_DEPARTMENT: '/admin/AdminUser/instructors-by-department',
    STUDENTS_BY_DEPARTMENT: '/admin/AdminUser/student-by-department', // + /{departmentId} şeklinde kullanılacak
    APPOINTMENT: '/admin/AdminAppointment',
    APPOINTMENT_UPDATE_STATUS: '/admin/AdminAppointment/update-status',
    STUDENT_GET_APPOINTMENTS: '/admin/AdminAppointment/student/{studentId}',
    ACADEMIC_GET_APPOINTMENTS: '/admin/AdminAppointment/academic/{academicId}'
  },
  SEARCH: {
    INSTRUCTOR_SCHOOL_DETAILS: '/admin/AdminSchool/instructor-school' // + /{school_id}/details şeklinde kullanılacak
  }
};

// Üniversite verisi için interface tanımı
export interface School {
  id: number;
  name: string;
}

// Bölüm verisi için interface tanımı
export interface Department {
  id: number;
  name: string;
}

// Akademisyen verisi için interface tanımı
export interface Instructor {
  id: string;
  userFullName: string;
  email: string;
  imageUrl: string;
  urlPath: string;
  departmentId?: number | string; // Akademisyenin bölüm ID'si
}

// Okul detayları için interface tanımı (arama için)
export interface SchoolDetails {
  id: number;
  name: string;
  departments: Department[];
  instructors: Instructor[];
}

// Öğrenci verisi için interface tanımı
export interface Student {
  id: string;
  userFullName: string;
  email: string;
  imageUrl?: string | null;
  urlPath?: string | null;
}

/**
 * API istekleri için temel fonksiyon
 * @param endpoint API endpoint adresi (örn. '/Auth/register')
 * @param method HTTP metodu
 * @param data Gönderilecek veri
 * @param headers İsteğe bağlı HTTP başlıkları
 * @returns API yanıtı
 */
export async function fetchApi<T>(
  endpoint: string, 
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' = 'GET',
  data?: any,
  headers?: HeadersInit
): Promise<T> {
  const url = `${API_BASE_URL}${endpoint}`;
  
  const options: RequestInit = {
    method,
    headers: {
      'Content-Type': 'application/json',
      ...headers
    },
    body: data ? JSON.stringify(data) : undefined
  };

  const response = await fetch(url, options);
  
  // Yanıt kontrolü
  if (!response.ok) {
    const errorText = await response.text();
    try {
      // Hata yanıtını JSON olarak parse etmeye çalış
      const errorData = JSON.parse(errorText);
      throw new Error(
        errorData?.message || 
        `İstek başarısız: ${response.status} ${response.statusText}`
      );
    } catch (e) {
      // JSON parse edilemezse, düz metin olarak fırlat
      throw new Error(errorText || `İstek başarısız: ${response.status} ${response.statusText}`);
    }
  }
  
  // Boş yanıt kontrolü
  if (response.status === 204) {
    return {} as T;
  }

  // Content-Type başlığını kontrol et
  const contentType = response.headers.get('content-type');
  
  if (contentType && contentType.includes('application/json')) {
    // JSON yanıt
    return await response.json() as T;
  } else {
    // Düz metin yanıt
    const textResponse = await response.text();
    
    // Düz metin yanıtı T tipinde bir nesneye dönüştür
    return {
      success: true,
      message: textResponse
    } as unknown as T;
  }
}

export default {
  API_BASE_URL,
  API_ENDPOINTS,
  fetchApi
};

/**
 * Tüm üniversiteleri getiren fonksiyon
 * @returns Üniversite listesi
 */
export async function getSchools(): Promise<School[]> {
  return fetchApi<School[]>(API_ENDPOINTS.ADMIN.SCHOOLS);
}

/**
 * Bölümleri getiren fonksiyon
 * @returns Bölüm listesi
 */
export async function getDepartments(): Promise<Department[]> {
  return fetchApi<Department[]>(API_ENDPOINTS.ADMIN.DEPARTMENTS);
}

/**
 * Belirli bir üniversiteye ait bölümleri getiren fonksiyon
 * @param schoolId Üniversite ID'si
 * @returns Bölüm listesi
 */
export async function getDepartmentsBySchool(schoolId: number): Promise<Department[]> {
  return fetchApi<Department[]>(`${API_ENDPOINTS.ADMIN.DEPARTMENTS_BY_SCHOOL}/${schoolId}/departments`);
}

/**
 * Belirli bir bölüme ait akademisyenleri getiren fonksiyon
 * @param departmentId Bölüm ID'si
 * @returns Akademisyen listesi
 */
export async function getInstructorsByDepartment(departmentId: string): Promise<Instructor[]> {
  return fetchApi<Instructor[]>(`${API_ENDPOINTS.ADMIN.INSTRUCTORS_BY_DEPARTMENT}/${departmentId}`);
}

/**
 * ID'ye göre akademisyen bilgilerini getiren fonksiyon
 * @param instructorId Akademisyen ID'si
 * @returns Akademisyen bilgileri
 */
export async function getInstructorById(instructorId: string): Promise<Instructor> {
  return fetchApi<Instructor>(`/admin/AdminUser/instructor/${instructorId}`);
}

/**
 * URL path'e göre akademisyen bilgilerini getiren fonksiyon
 * @param urlPath Akademisyen URL path bilgisi
 * @returns Akademisyen bilgileri
 */
export async function getInstructorByUrlPath(urlPath: string): Promise<Instructor> {
  return fetchApi<Instructor>(`/admin/AdminUser/instructor-by-path/${urlPath}`);
}

/**
 * Okul ID'sine göre detaylı okul bilgilerini (bölümler ve akademisyenler dahil) getiren fonksiyon
 * @param schoolId Okul ID'si
 * @returns Okul detayları (bölümler ve akademisyenler dahil)
 */
export async function getSchoolDetails(schoolId: number): Promise<SchoolDetails> {
  return fetchApi<SchoolDetails>(`${API_ENDPOINTS.SEARCH.INSTRUCTOR_SCHOOL_DETAILS}/${schoolId}/details`);
}

/**
 * Belirli bir bölüme ait öğrencileri getiren fonksiyon
 * @param departmentId Bölüm ID'si
 * @returns Öğrenci listesi
 */
export async function getStudentsByDepartment(departmentId: string): Promise<Student[]> {
  return fetchApi<Student[]>(`${API_ENDPOINTS.ADMIN.STUDENTS_BY_DEPARTMENT}/${departmentId}`);
}


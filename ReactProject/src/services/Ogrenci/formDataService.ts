import { fetchApi, API_ENDPOINTS } from '../../lib/api';
import { 
  getSchools, 
  getDepartmentsBySchool, 
  School, 
  Department 
} from '../../lib/api';

// =============== KAYIT İŞLEMLERİ İLE İLGİLİ TANIMLAR ===============

/**
 * Kayıt formu verileri için interface
 */
export interface FormData {
  userFullName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

/**
 * API'ye gönderilecek kayıt verileri için interface
 */
export interface RegisterData {
  userFullName: string;
  email: string;
  password: string;
  confirmPassword: string;
  schoolId: number;
  departmentId: number;
}

/**
 * Form validasyon fonksiyonu
 * @param formData Form verileri
 * @returns Hata mesajı veya null
 */
export const validateForm = (formData: FormData): string | null => {
  // Ad Soyad kontrolü
  if (!formData.userFullName.trim()) {
    return 'Ad Soyad alanı zorunludur';
  }
  
  // Email kontrolü
  if (!formData.email.trim()) {
    return 'Email alanı zorunludur';
  }
  
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(formData.email)) {
    return 'Geçerli bir email adresi giriniz';
  }
  
  // Şifre kontrolü
  if (!formData.password) {
    return 'Şifre alanı zorunludur';
  }
  
  if (formData.password.length < 6) {
    return 'Şifre en az 6 karakter olmalıdır';
  }
  
  // Şifre tekrar kontrolü
  if (formData.password !== formData.confirmPassword) {
    return 'Şifreler eşleşmiyor';
  }
  
  return null;
};

/**
 * Kayıt verisi oluşturan yardımcı fonksiyon
 * @param formData Form verileri
 * @returns API için hazırlanmış kayıt verisi
 */
export const createRegisterData = (formData: FormData & { schoolId?: number; departmentId?: number }): RegisterData => {
  return {
    userFullName: formData.userFullName,
    email: formData.email,
    password: formData.password,
    confirmPassword: formData.confirmPassword,
    schoolId: formData.schoolId || 1, // Kullanıcının seçtiği üniversite veya varsayılan
    departmentId: formData.departmentId || 1 // Kullanıcının seçtiği bölüm veya varsayılan
  };
};

/**
 * API yanıt tipi
 */
interface ApiResponse {
  success: boolean;
  message: string;
}

/**
 * Öğrenci kayıt işlemini gerçekleştiren fonksiyon
 * @param registerData Kayıt verileri
 * @param onSuccess Başarı callback'i
 * @param onError Hata callback'i
 */
export const ogrenciKayit = async (
  registerData: RegisterData,
  onSuccess: () => void,
  onError: (message: string) => void
): Promise<void> => {
  try {
    // API endpointi - Auth/register
    const endpoint = API_ENDPOINTS.AUTH.REGISTER;
    
    // API isteği gönder
    const response = await fetchApi<ApiResponse>(endpoint, 'POST', registerData);
    
    // Yanıt mesajını konsola yaz
    console.log('Kayıt Yanıtı:', response.message);
    
    // Başarılı sonuç
    onSuccess();
  } catch (error) {
    // Hata durumu
    if (error instanceof Error) {
      onError(error.message);
    } else {
      onError('Kayıt işlemi sırasında bir hata oluştu');
    }
  }
};

// =============== FORM VERİLERİ İŞLEMLERİ ===============

/**
 * Kayıt verileri için interface
 */
export interface KayitVerileri {
  schools: School[];
  departments: Department[];
}

/**
 * Tüm üniversiteleri getiren fonksiyon
 * @returns Üniversite listesi
 */
export const getUniversiteler = async (): Promise<School[]> => {
  try {
    // API'den üniversiteleri getir
    return await getSchools();
  } catch (error) {
    console.error('Üniversiteler getirilirken hata:', error);
    // Hata durumunda boş dizi dön
    return [];
  }
};

/**
 * Belirli bir üniversiteye ait bölümleri getiren fonksiyon
 * @param universiteId Üniversite ID'si
 * @returns Bölüm listesi
 */
export const getBolumlerByUniversite = async (universiteId: number): Promise<Department[]> => {
  try {
    // API'den seçilen üniversiteye ait bölümleri getir
    return await getDepartmentsBySchool(universiteId);
  } catch (error) {
    console.error(`${universiteId} ID'li üniversiteye ait bölümler getirilirken hata:`, error);
    // Hata durumunda boş dizi dön
    return [];
  }
};

/**
 * Kayıt formu için tüm verileri asenkron olarak getiren fonksiyon
 * @param selectedUniversityId Seçili üniversite ID'si - varsayılan olarak ilk üniversite
 * @returns Tüm kayıt verileri
 */
export const getFormVerileri = async (selectedUniversityId: number = 1): Promise<KayitVerileri> => {
  try {
    // Tüm üniversiteleri getir
    const universities = await getUniversiteler();
    
    // Seçili üniversite ID'si yoksa veya geçerli değilse, ilk üniversiteyi kullan
    const universityId = universities.length > 0 
      ? (universities.some(u => u.id === selectedUniversityId) ? selectedUniversityId : universities[0].id)
      : 1;
    
    // Seçili üniversiteye ait bölümleri getir
    const departments = await getBolumlerByUniversite(universityId);
    
    // Tüm verileri birleştir
    return {
      schools: universities,
      departments
    };
  } catch (error) {
    console.error('Form verileri getirilirken hata:', error);
    // Hata durumunda boş değerler dön
    return {
      schools: [],
      departments: []
    };
  }
};

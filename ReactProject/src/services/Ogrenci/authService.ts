import { fetchApi, API_ENDPOINTS } from '../../lib/api';
import { decodeToken } from '../../contexts/AuthContext';

// =============== GİRİŞ İLE İLGİLİ TANIMLAR ===============

// Giriş formundan alınacak veri tipi
export interface LoginFormData {
  email: string;
  password: string;
}

// API yanıt tipi
interface LoginResponse {
  token: string;
  // API diğer alanları da dönebilir
}

// Rol kontrolü yanıt tipi
export interface RoleCheckResult {
  isCorrectRole: boolean;
  token: string;
  redirectPath?: string;
  errorMessage?: string;
}

/**
 * Giriş formundaki verileri doğrular
 * @param data Form verileri
 * @returns Hata mesajı veya null
 */
export const validateLoginForm = (data: LoginFormData): string | null => {
  // Email kontrolü
  if (!data.email.trim()) {
    return 'Email alanı zorunludur';
  }
  
  // Basit email doğrulama
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(data.email)) {
    return 'Geçerli bir email adresi giriniz';
  }
  
  // Şifre kontrolü
  if (!data.password) {
    return 'Şifre alanı zorunludur';
  }
  
  if (data.password.length < 6) {
    return 'Şifre en az 6 karakter olmalıdır';
  }
  
  return null;
};

/**
 * Token içindeki rol bilgisini kontrol eder
 * @param token JWT token
 * @param expectedRole Beklenen rol
 * @returns Kontrol sonucu
 */
export const checkUserRole = (token: string, expectedRole: string): RoleCheckResult => {
  const userData = decodeToken(token);
  
  if (!userData) {
    return {
      isCorrectRole: false,
      token,
      errorMessage: 'Token çözümlenemedi'
    };
  }
  
  // Kullanıcı rolü beklenen rol ile uyuşuyor mu kontrol et
  if (userData.roles === expectedRole) {
    return {
      isCorrectRole: true,
      token
    };
  }
  
  // Rol uyuşmuyor, uygun yönlendirme yolunu belirle
  let redirectPath = '/portal/ogrenci';
  let errorMessage = 'Bu giriş sayfasına uygun rol bulunamadı.';
  
  if (userData.roles === 'Student') {
    redirectPath = '/portal/ogrenci';
    errorMessage = 'Öğrenci olarak giriş yapmalısınız.';
  } else if (userData.roles === 'Instructor') {
    redirectPath = '/portal/akademisyen';
    errorMessage = 'Akademisyen olarak giriş yapmalısınız.';
  }
  
  return {
    isCorrectRole: false,
    token,
    redirectPath,
    errorMessage
  };
};

/**
 * Öğrenci giriş işlemini gerçekleştiren fonksiyon
 * @param loginData Giriş verileri (email, password)
 * @param onSuccess Başarı callback'i - token parametresi ile
 * @param onError Hata callback'i
 * @param onWrongRole Yanlış rol callback'i
 */
export const ogrenciGiris = async (
  loginData: LoginFormData,
  onSuccess: (token: string) => void,
  onError: (message: string) => void,
  onWrongRole: (result: RoleCheckResult) => void
): Promise<void> => {
  try {
    // Form verilerini doğrula
    const validationError = validateLoginForm(loginData);
    if (validationError) {
      onError(validationError);
      return;
    }
    
    // API endpointi
    const endpoint = API_ENDPOINTS.AUTH.LOGIN;
    
    // API isteği gönder
    const response = await fetchApi<LoginResponse>(endpoint, 'POST', loginData);
    
    // Token kontrolü
    if (response && response.token) {
      // Rol kontrolü
      const roleCheck = checkUserRole(response.token, 'Student');
      
      if (roleCheck.isCorrectRole) {
        // Doğru rol, başarılı giriş
        onSuccess(response.token);
      } else {
        // Yanlış rol, yönlendirme yap
        onWrongRole(roleCheck);
      }
    } else {
      onError('Giriş yapılamadı: Token alınamadı');
    }
  } catch (error) {
    // Hata durumu
    if (error instanceof Error) {
      onError(error.message);
    } else {
      onError('Giriş sırasında beklenmeyen bir hata oluştu');
    }
    
    console.error('Giriş hatası:', error);
  }
};

/**
 * Akademisyen giriş işlemini gerçekleştiren fonksiyon
 * @param loginData Giriş verileri (email, password)
 * @param onSuccess Başarı callback'i - token parametresi ile
 * @param onError Hata callback'i
 * @param onWrongRole Yanlış rol callback'i
 */
export const akademisyenGiris = async (
  loginData: LoginFormData,
  onSuccess: (token: string) => void,
  onError: (message: string) => void,
  onWrongRole: (result: RoleCheckResult) => void
): Promise<void> => {
  try {
    // Form verilerini doğrula
    const validationError = validateLoginForm(loginData);
    if (validationError) {
      onError(validationError);
      return;
    }
    
    // API endpointi
    const endpoint = API_ENDPOINTS.AUTH.LOGIN;
    
    // API isteği gönder - role bilgisini ekleyerek
    const response = await fetchApi<LoginResponse>(
      endpoint, 
      'POST', 
      { ...loginData, role: 'Instructor' }
    );
    
    // Token kontrolü
    if (response && response.token) {
      // Rol kontrolü
      const roleCheck = checkUserRole(response.token, 'Instructor');
      
      if (roleCheck.isCorrectRole) {
        // Doğru rol, başarılı giriş
        onSuccess(response.token);
      } else {
        // Yanlış rol, yönlendirme yap
        onWrongRole(roleCheck);
      }
    } else {
      onError('Giriş yapılamadı: Token alınamadı');
    }
  } catch (error) {
    // Hata durumu
    if (error instanceof Error) {
      onError(error.message);
    } else {
      onError('Giriş sırasında beklenmeyen bir hata oluştu');
    }
    
    console.error('Akademisyen girişi hatası:', error);
  }
};

// =============== ÇIKIŞ İLE İLGİLİ TANIMLAR ===============

/**
 * API yanıt tipi
 */
interface LogoutResponse {
  success: boolean;
  message?: string;
}

/**
 * Öğrenci çıkış işlemini gerçekleştiren fonksiyon
 * @param token JWT token
 * @param onSuccess Başarı callback'i
 * @param onError Hata callback'i
 */
export const ogrenciCikis = async (
  token: string | null,
  onSuccess: () => void,
  onError: (message: string) => void
): Promise<void> => {
  try {
    // Token yoksa doğrudan başarılı kabul et
    if (!token) {
      console.warn('Çıkış yapılıyor: Token bulunamadı');
      onSuccess();
      return;
    }

    // API endpoint'i
    const endpoint = API_ENDPOINTS.AUTH.LOGOUT;
    
    // API'ye istek gönder - Headers'a token ekle
    await fetchApi<LogoutResponse>(
      endpoint, 
      'POST', 
      {}, // Boş payload
      {
        'Authorization': `Bearer ${token}`
      }
    );
    
    // LocalStorage'dan token ve kullanıcı bilgilerini temizle
    clearUserSession();
    
    // Başarılı sonuç
    onSuccess();
  } catch (error) {
    console.error('Çıkış hatası:', error);
    
    // Hata olsa bile kullanıcı oturumunu temizle
    clearUserSession();
    
    // Hata durumu
    if (error instanceof Error) {
      onError(error.message);
    } else {
      onError('Çıkış işlemi sırasında bir hata oluştu');
    }
    
    // Hata olsa bile başarılı kabul et
    onSuccess();
  }
};

/**
 * Kullanıcı oturum bilgilerini temizler
 */
export const clearUserSession = (): void => {
  // Tüm oturum verilerini temizle
  localStorage.removeItem('authToken');
  localStorage.removeItem('userId');
  localStorage.removeItem('userEmail');
  localStorage.removeItem('userRole');
};

// =============== ŞİFRE SIFIRLAMA İLE İLGİLİ TANIMLAR ===============

// API istek tipi
interface ForgotPasswordRequest {
  email: string;
}

// API yanıt tipi
interface ForgotPasswordResponse {
  success: boolean;
  message?: string;
}

/**
 * Şifre sıfırlama isteği gönderen fonksiyon
 * @param email Kullanıcı email adresi
 * @param onSuccess Başarı callback'i
 * @param onError Hata callback'i
 */
export const sifremiUnuttum = async (
  email: string,
  onSuccess: (message: string) => void,
  onError: (message: string) => void
): Promise<void> => {
  try {
    // Email validasyonu
    if (!email || !email.includes('@')) {
      onError('Lütfen geçerli bir e-posta adresi girin.');
      return;
    }
    
    // API endpointi
    const endpoint = API_ENDPOINTS.AUTH.FORGOT_PASSWORD;
    
    // İstek verisi
    const requestData: ForgotPasswordRequest = {
      email: email
    };
    
    // API isteği gönder
    const response = await fetchApi<ForgotPasswordResponse>(endpoint, 'POST', requestData);
    
    // Başarılı yanıt
    if (response.success) {
      onSuccess(response.message || 'Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.');
    } else {
      onError(response.message || 'Şifre sıfırlama isteği başarısız oldu.');
    }
  } catch (error) {
    // Hata durumu
    console.error('Şifre sıfırlama hatası:', error);
    
    if (error instanceof Error) {
      onError(error.message);
    } else {
      onError('Şifre sıfırlama işlemi sırasında bir hata oluştu.');
    }
  }
};

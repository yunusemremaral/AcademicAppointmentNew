import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';

// Token içinden çıkarılan kullanıcı bilgileri için tip tanımı
export interface UserData {
  sub: string;
  email: string;
  jti: string;
  username: string;
  userfullname: string;
  schoolId: string;
  departmentId: string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  roles: string;
  exp: number;
  iss: string;
  aud: string;
}

// Auth context için tip tanımı
interface AuthContextType {
  isAuthenticated: boolean;
  user: UserData | null;
  token: string | null;
  login: (token: string) => void;
  logout: () => void;
  loading: boolean;
}

// Varsayılan boş değerler
const defaultContext: AuthContextType = {
  isAuthenticated: false,
  user: null,
  token: null,
  login: () => {},
  logout: () => {},
  loading: true
};

// Context oluşturma
const AuthContext = createContext<AuthContextType>(defaultContext);

// Context hook'u
export const useAuth = () => useContext(AuthContext);

// Token'dan kullanıcı bilgilerini çıkaran yardımcı fonksiyon
export const decodeToken = (token: string): UserData | null => {
  try {
    // Token'ın payload kısmını al (2. parça)
    const base64Url = token.split('.')[1];
    // Base64Url'i Base64'e çevir
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    // Base64 decode ve JSON parse işlemleri
    const decodedData = JSON.parse(
      decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      )
    );
    
    return decodedData as UserData;
  } catch (error) {
    console.error('Token decode hatası:', error);
    return null;
  }
};

// Token geçerliliğini kontrol eden fonksiyon
export const isTokenValid = (token: string): boolean => {
  try {
    const userData = decodeToken(token);
    if (!userData) return false;
    
    // Token süresi dolmuş mu kontrol et (exp değeri saniye cinsinden)
    const currentTime = Math.floor(Date.now() / 1000);
    return userData.exp > currentTime;
  } catch (error) {
    return false;
  }
};

// LocalStorage anahtarları
export const STORAGE_KEYS = {
  TOKEN: 'authToken',
  USER_ID: 'userId',
  USER_EMAIL: 'userEmail',
  USER_ROLE: 'userRole'
};

// Provider bileşeni
export const AuthProvider: React.FC<{children: ReactNode}> = ({ children }) => {
  const [user, setUser] = useState<UserData | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);
  const [token, setToken] = useState<string | null>(null);

  // LocalStorage'a kullanıcı bilgilerini kaydet
  const saveUserData = (userData: UserData) => {
    localStorage.setItem(STORAGE_KEYS.USER_ID, userData.sub);
    localStorage.setItem(STORAGE_KEYS.USER_EMAIL, userData.email);
    localStorage.setItem(STORAGE_KEYS.USER_ROLE, userData.roles);
  };

  // LocalStorage'dan kullanıcı bilgilerini temizle
  const clearUserData = () => {
    Object.values(STORAGE_KEYS).forEach(key => {
      localStorage.removeItem(key);
    });
  };

  // Giriş işlemi
  const login = (token: string) => {
    localStorage.setItem(STORAGE_KEYS.TOKEN, token);
    setToken(token);
    
    const userData = decodeToken(token);
    if (userData) {
      setUser(userData);
      setIsAuthenticated(true);
      saveUserData(userData);
      
      // Rol bazlı yönlendirme
      if (userData.roles === 'Student') {
        // Sayfa yenileme olmadan URL değiştirme (React Router entegrasyonu olmadan)
        window.location.replace('/ogrenci/anasayfa');
      } else if (userData.roles === 'Instructor') {
        // Akademisyen rolü için
        window.location.replace('/akademisyen/anasayfa');
      } else {
        // Diğer roller için
        window.location.replace('/portal/ogrenci');
      }
    }
  };

  // Çıkış işlemi
  const logout = () => {
    clearUserData();
    setUser(null);
    setIsAuthenticated(false);
    setToken(null);
    // Sayfa yenileme olmadan URL değiştirme
    window.location.replace('/portal/ogrenci');
  };

  // Sayfa yüklendiğinde token kontrolü
  useEffect(() => {
    const initAuth = () => {
      const token = localStorage.getItem(STORAGE_KEYS.TOKEN);
      
      if (token && isTokenValid(token)) {
        setToken(token);
        const userData = decodeToken(token);
        if (userData) {
          setUser(userData);
          setIsAuthenticated(true);
          saveUserData(userData);
          
          // Eğer mevcut path kullanıcının rolüne uygun değilse otomatik yönlendirme yap
          const currentPath = window.location.pathname;
          if (userData.roles === 'Student' && currentPath.startsWith('/portal/')) {
            window.location.replace('/ogrenci/anasayfa');
          } else if (userData.roles === 'Instructor' && currentPath.startsWith('/portal/')) {
            window.location.replace('/akademisyen/anasayfa');
          }
        } else {
          clearUserData();
        }
      } else if (token) {
        // Token var ama geçersiz
        clearUserData();
      }
      
      setLoading(false);
    };

    initAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, token, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext; 
import { ReactNode, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import Loading from '../components/Loading';

interface AuthGuardProps {
  children: ReactNode;
}

/**
 * AuthGuard bileşeni
 * Oturum açan kullanıcının rotaları için koruma sağlar
 * Örneğin: Öğrenci rolündeki kullanıcıların portal sayfalarına erişimini engeller
 */
const AuthGuard: React.FC<AuthGuardProps> = ({ children }) => {
  const { isAuthenticated, user, loading } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  const currentPath = location.pathname;

  useEffect(() => {
    // Yükleme durumundaysa ve oturum kontrolü henüz tamamlanmadıysa işlem yapma
    if (loading) return;

    // Kullanıcı oturum açmışsa ve öğrenci rolündeyse
    if (isAuthenticated && user && user.roles === 'Student') {
      // Öğrenciler sadece /ogrenci/ ile başlayan sayfalara erişebilir
      if (!currentPath.startsWith('/ogrenci/') && 
          !currentPath.startsWith('/logout') &&
          !currentPath.startsWith('/')) {
        // Öğrenci ana sayfasına yönlendir - navigate ile
        navigate('/ogrenci/anasayfa', { replace: true });
      }
    }
    
    // Kullanıcı oturum açmışsa ve akademisyen rolündeyse
    if (isAuthenticated && user && user.roles === 'Instructor') {
      // Akademisyenler sadece /akademisyen/ ile başlayan sayfalara erişebilir
      if (!currentPath.startsWith('/akademisyen/') && 
          !currentPath.startsWith('/logout') &&
          !currentPath.startsWith('/')) {
        // Akademisyen ana sayfasına yönlendir - navigate ile
        navigate('/akademisyen/anasayfa', { replace: true });
      }
    }
    
    // Oturum açılmışsa ve portal sayfasına erişim var ise engellenecek
    if (isAuthenticated && currentPath.startsWith('/portal/')) {
      if (user?.roles === 'Student') {
        navigate('/ogrenci/anasayfa', { replace: true });
      } else if (user?.roles === 'Instructor') {
        navigate('/akademisyen/anasayfa', { replace: true });
      }
    }
  }, [loading, isAuthenticated, user, currentPath, navigate]);

  // Yükleme durumunda Loading bileşeni göster
  if (loading) {
    return <Loading text="Yetkilendirme kontrol ediliyor..." />;
  }

  // Herhangi bir yönlendirme yoksa çocukları göster
  return <>{children}</>;
};

export default AuthGuard; 
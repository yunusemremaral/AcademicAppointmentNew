import { Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { ReactNode, useEffect } from 'react';
import Loading from '../components/Loading';

interface PrivateRouteProps {
  children?: ReactNode;
  redirectPath?: string;
  allowedRoles?: string[];
}

/**
 * Koruma altındaki rotalar için kullanılan bileşen
 * Kullanıcı oturum açmamışsa yönlendirme yapar
 * Rol bazlı kısıtlama için allowedRoles parametresi kullanılabilir
 */
const PrivateRoute: React.FC<PrivateRouteProps> = ({
  children,
  redirectPath = '/portal/ogrenci',
  allowedRoles = [],
}) => {
  const { isAuthenticated, user, loading } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    // Yükleme durumunda işlem yapma
    if (loading) return;

    // Kullanıcı oturum açmamışsa yönlendir
    if (!isAuthenticated) {
      navigate(redirectPath, { replace: true });
      return;
    }

    // Rol bazlı kısıtlama varsa kontrol et
    if (allowedRoles.length > 0 && user) {
      const hasRequiredRole = allowedRoles.includes(user.roles);
      
      if (!hasRequiredRole) {
        // Kullanıcının rolüne uygun sayfaya yönlendir
        if (user.roles === 'Student') {
          navigate('/ogrenci/anasayfa', { replace: true });
        } else if (user.roles === 'Instructor') {
          navigate('/akademisyen/anasayfa', { replace: true });
        } else {
          navigate('/portal/ogrenci', { replace: true });
        }
      }
    }
  }, [loading, isAuthenticated, user, allowedRoles, redirectPath, navigate]);

  // Yükleme durumu devam ediyorsa bekleme göster
  if (loading) {
    return <Loading text="Sayfaya erişim kontrol ediliyor..." />;
  }

  // Erişim kontrolü geçtiyse içeriği göster
  return isAuthenticated ? <>{children || <Outlet />}</> : null;
};

export default PrivateRoute; 
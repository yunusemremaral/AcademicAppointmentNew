import React, { useState, useEffect } from 'react';
import { Outlet, useLocation, useNavigate } from 'react-router-dom';
import LeftPanel from './LeftPanel';
import RightPanel from './RightPanel';

// Form başlık ve alt başlıkları için veri yapısı
const formTitles: Record<string, { title: string; subtitle: string }> = {
  '/portal/ogrenci': {
    title: 'Öğrenci Girişi',
    subtitle: 'Öğrenci hesabınızla giriş yapın'
  },
  '/portal/ogrenci-kayit': {
    title: 'Öğrenci Kaydı',
    subtitle: 'Yeni bir öğrenci hesabı oluşturun'
  },
  '/portal/akademisyen': {
    title: 'Akademisyen Girişi',
    subtitle: 'Akademisyen hesabınızla giriş yapın'
  },
  '/portal/yonetici': {
    title: 'Yönetici Girişi',
    subtitle: 'Yönetici hesabınızla giriş yapın'
  },
  '/portal/sifre-sifirlama': {
    title: 'Şifre Sıfırlama',
    subtitle: 'Hesap şifrenizi sıfırlamak için mail adresinizi girin'
  }
};

const AuthLayout: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const pathname = location.pathname;
  
  // Mobil görünüm için state
  const [isMobile, setIsMobile] = useState(false);
  const [showRightPanel, setShowRightPanel] = useState(false);
  
  // Mobil yükseklik düzeltmesi için
  const [viewportHeight, setViewportHeight] = useState(window.innerHeight);
  
  // Ekran boyutunu izle
  useEffect(() => {
    const checkScreenSize = () => {
      setIsMobile(window.innerWidth < 768); // md breakpoint'i
      setViewportHeight(window.innerHeight); // Gerçek viewport yüksekliğini al
    };
    
    // İlk yüklemede kontrol et
    checkScreenSize();
    
    // Ekran boyutu değiştiğinde kontrol et
    window.addEventListener('resize', checkScreenSize);
    
    // Mobil modda sağ paneli temizle
    if (isMobile) {
      setShowRightPanel(false);
    }
    
    return () => window.removeEventListener('resize', checkScreenSize);
  }, [isMobile]);
  
  // Portal yönlendirme fonksiyonu
  const handlePortalNavigation = (portalPath: string) => {
    navigate(portalPath);
    setShowRightPanel(true);
  };
  
  // Başlık ve alt başlığı URL yoluna göre belirle
  const defaultTitle = { title: 'Giriş Yap', subtitle: 'Akademik Randevu Sistemine giriş yapın' };
  const { title, subtitle } = formTitles[pathname] || defaultTitle;

  // Portal'a geri dön
  const handleBackToPortal = () => {
    setShowRightPanel(false);
  };

  // Mobil cihazlarda gerçek viewport yüksekliğini kullan
  const containerStyle = {
    height: isMobile ? `${viewportHeight}px` : '100vh',
  };

  return (
    // Ana container - grid sistemi ile ekranı iki panele böler
    <div 
      className="grid grid-cols-1 md:grid-cols-2 font-inter overflow-hidden"
      style={containerStyle}
    >
      {/* Sol Panel - Masaüstünde her zaman, mobilde RightPanel gösterilmiyorsa göster */}
      {(!isMobile || (isMobile && !showRightPanel)) && (
        <div className="h-full bg-primary">
          <LeftPanel 
            currentPath={pathname}
            onPortalSelect={handlePortalNavigation}
            isMobile={isMobile}
          />
        </div>
      )}
      
      {/* Sağ Panel - Masaüstünde her zaman, mobilde sadece seçildiğinde göster */}
      {(!isMobile || (isMobile && showRightPanel)) && (
        <div className="h-full bg-gray-50 overflow-auto">
          <RightPanel 
            title={title} 
            subtitle={subtitle}
            onBackToPortal={handleBackToPortal}
            showBackButton={isMobile && showRightPanel}
          >
            <Outlet />
          </RightPanel>
        </div>
      )}
    </div>
  );
};

export default AuthLayout; 
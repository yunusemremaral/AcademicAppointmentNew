import React from 'react';
import { Link } from 'react-router-dom';
import { Users, Lock} from 'lucide-react';

// Varlık importları
import iosIcon from '../../assets/logo/MobileAppLogo/ios-ikon.png';
import androidIcon from '../../assets/logo/MobileAppLogo/android-ikon.png';

// Tip tanımlamaları
interface LeftPanelProps {
  currentPath: string;
  onPortalSelect?: (portalPath: string) => void;
  isMobile?: boolean;
}

interface PortalLinkProps {
  to: string;
  icon: React.ReactNode;
  label: string;
  isActive: boolean;
  onClick?: () => void;
}

interface MobileAppLinkProps {
  icon: string;
  label: string;
  alt: string;
}

// Yazı animasyonu için özel hook
const useTypingAnimation = (
  text: string, 
  typingSpeed = 110, 
  pauseDelay = 2000
) => {
  const [displayText, setDisplayText] = React.useState('');
  const [showCursor, setShowCursor] = React.useState(true);
  const cursorIntervalRef = React.useRef<NodeJS.Timeout | null>(null);
  const timeoutRef = React.useRef<NodeJS.Timeout | null>(null);
  
  React.useEffect(() => {
    let currentIndex = 0;
    let isDeleting = false;
    
    // İmleç yanıp sönme efekti
    cursorIntervalRef.current = setInterval(() => {
      setShowCursor(prev => !prev);
    }, 530);
    
    const type = () => {
      // Yazma/silme hızını belirle
      const typingDelay = isDeleting ? typingSpeed * 0.5 : typingSpeed;
      
      if (!isDeleting && currentIndex < text.length) {
        // Yazma modu
        setDisplayText(text.substring(0, currentIndex + 1));
        currentIndex++;
        timeoutRef.current = setTimeout(type, typingDelay);
      } else if (!isDeleting && currentIndex >= text.length) {
        // Yazma tamamlandı, duraklama ve silme
        isDeleting = true;
        timeoutRef.current = setTimeout(type, pauseDelay);
      } else if (isDeleting && currentIndex > 0) {
        // Silme modu
        setDisplayText(text.substring(0, currentIndex - 1));
        currentIndex--;
        timeoutRef.current = setTimeout(type, typingDelay);
      } else if (isDeleting && currentIndex <= 0) {
        // Silme tamamlandı, yeniden başlat
        isDeleting = false;
        timeoutRef.current = setTimeout(type, typingDelay * 2);
      }
    };
    
    // Animasyonu başlat
    timeoutRef.current = setTimeout(type, typingSpeed);
    
    // Temizleme fonksiyonu
    return () => {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
      if (cursorIntervalRef.current) {
        clearInterval(cursorIntervalRef.current);
      }
    };
  }, [text, typingSpeed, pauseDelay]);
  
  return { displayText, showCursor };
};

// Animasyonlu logo ve slogan bileşeni
const LogoSection = () => {
  const { displayText, showCursor } = useTypingAnimation('akademikrandevu.com');
  
  return (
    <div className="flex flex-col items-center text-white">
      <div className="text-center w-full">
        {/* Animasyon alanı */}
        <div className="h-10 flex items-center justify-center">
          <span className="text-transparent bg-clip-text bg-gradient-to-r from-blue-200 to-white text-3xl font-semibold drop-shadow-md">
            {displayText}
          </span>
          {showCursor && (
            <span className="w-0.5 h-6 bg-blue-300/80 ml-0.5 animate-pulse shadow-sm shadow-blue-300/20" />
          )}
        </div>
        
        {/* Slogan */}
        <div className="mt-3 h-6 flex items-center justify-center">
          <p className="text-sm text-blue-50/90 font-light tracking-wide">
          Tüm üniversiteler için hızlı, kolay ve merkezi randevu platformu.
          </p>
        </div>
      </div>
    </div>
  );
};

// Portal navigasyon bağlantı bileşeni
const PortalLink: React.FC<PortalLinkProps> = ({ to, icon, label, isActive, onClick }) => {
  // Stil sınıfları
  const baseClasses = "group flex items-center py-3 px-5 rounded-md transition-all duration-300 backdrop-blur-sm";
  const activeClasses = "bg-gradient-to-r from-white/15 to-blue-300/15 text-white shadow-sm shadow-blue-900/5 border border-white/20";
  const inactiveClasses = "bg-white/8 text-gray-100 hover:bg-white/10 border border-white/10 hover:border-white/15 hover:shadow-sm hover:shadow-blue-900/5 hover:bg-gradient-to-r hover:from-white/5 hover:to-blue-300/5";
  
  // Eğer onClick prop'u varsa, özel tıklama olayını işle
  if (onClick) {
    return (
      <button 
        onClick={onClick}
        className={`${baseClasses} ${isActive ? activeClasses : inactiveClasses} w-full text-left`}
      >
        {icon}
        <span className="font-medium text-sm tracking-wide">{label}</span>
      </button>
    );
  }
  
  // Normal Link bileşenini kullan
  return (
    <Link to={to} className={`${baseClasses} ${isActive ? activeClasses : inactiveClasses}`}>
      {icon}
      <span className="font-medium text-sm tracking-wide">{label}</span>
    </Link>
  );
};

// Mobil uygulama bağlantı bileşeni
const MobileAppLink: React.FC<MobileAppLinkProps> = ({ icon, label, alt }) => {
  return (
    <a 
      href="#" 
      className="group flex items-center bg-primary/10 hover:bg-white/85 px-3 py-2 rounded-md transition-all duration-300 border border-white/20 hover:border-white/25 text-xs hover:shadow-sm hover:shadow-blue-900/5"
    >
      <div className="w-4 h-4 mr-2 flex items-center justify-center rounded-full bg-white p-0.5">
        <img src={icon} alt={alt} className="w-3 h-3" />
      </div>
      <span className="text-white group-hover:text-primary ransition-colors">{label}</span>
    </a>
  );
};

// Portal ikonu için stil oluşturucu yardımcı fonksiyon
const getIconStyles = (isActive: boolean) => {
  return `mr-3 ${isActive ? 'text-blue-300/90' : 'text-gray-300'} group-hover:text-blue-300/90 transition-colors`;
};

// Mobil uygulama bağlantıları bölümü
const MobileAppSection = () => {
  const mobileAppLinks = [
    { icon: iosIcon, label: "iOS", alt: "iOS" },
    { icon: androidIcon, label: "Android", alt: "Android" }
  ];

  return (
    <div className="flex flex-col items-center justify-center text-white">
      <div className="flex flex-col items-center">
        <p className="text-xs mb-3 text-blue-100/70 tracking-wide font-light">Mobil uygulamalarımız</p>
        <div className="flex space-x-3">
          {mobileAppLinks.map((link, index) => (
            <MobileAppLink 
              key={index}
              icon={link.icon}
              label={link.label}
              alt={link.alt}
            />
          ))}
        </div>
      </div>
    </div>
  );
};

// Ana bileşen
const LeftPanel: React.FC<LeftPanelProps> = ({ currentPath, onPortalSelect, isMobile = false }) => {
  // Aktif sayfa kontrolü
  const isStudentPath = currentPath.includes('/portal/ogrenci');
  const isAcademicianPath = currentPath === '/portal/akademisyen';  
  // Portal navigasyon bağlantıları
  const portalLinks = [
    {
      to: "/portal/ogrenci",
      icon: <Users className={getIconStyles(isStudentPath)} size={18} strokeWidth={1.8} />,
      label: "Öğrenci Portal",
      isActive: isStudentPath
    },
    {
      to: "/portal/akademisyen",
      icon: <Lock className={getIconStyles(isAcademicianPath)} size={18} strokeWidth={1.8} />,
      label: "Akademisyen Portal",
      isActive: isAcademicianPath
    }
  ];
  
  return (
    <div className="bg-primary h-full flex flex-col overflow-hidden relative">
      {/* Arka plan vurgusu */}
      <div className="absolute bottom-0 left-0 right-0 h-24 opacity-20 bg-gradient-to-t from-blue-300/30 to-transparent pointer-events-none"></div>
      
      {/* ÜST KISIM - Logo ve slogan */}
      <div className="h-[120px] shrink-0 pt-8 px-8 z-10">
        <LogoSection />
      </div>
      
      {/* ORTA KISIM - Portal navigasyon */}
      <div className="flex-1 overflow-hidden py-8 px-8">
        <div className="h-full flex flex-col justify-center items-center text-white">
          <div className="w-full max-w-xs space-y-3">
            {portalLinks.map((link, index) => (
              <PortalLink 
                key={index}
                to={link.to}
                icon={link.icon}
                label={link.label}
                isActive={link.isActive}
                onClick={isMobile && onPortalSelect ? () => onPortalSelect(link.to) : undefined}
              />
            ))}
          </div>
        </div>
      </div>
      
      {/* ALT KISIM - Mobil uygulama bağlantıları */}
      <div className="h-[100px] shrink-0 px-8 py-4 z-10">
        <div className="h-full flex items-center justify-center">
          <div className="w-full max-w-xs">
            <MobileAppSection />
          </div>
        </div>
      </div>
    </div>
  );
};

export default LeftPanel;
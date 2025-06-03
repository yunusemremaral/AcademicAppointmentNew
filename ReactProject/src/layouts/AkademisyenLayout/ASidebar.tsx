import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { 
  Home, 
  UserRoundCog,
  Settings,
  LogOut,
  ChevronUp,
  Users,
  Dock
} from 'lucide-react';
import beunLogo from '../../assets/logo/SidebarLogo/beunlogo.png';
import { useAuth } from '../../contexts/AuthContext';

interface NavItemProps {
  icon: React.ReactNode;
  label: string;
  to: string;
  active?: boolean;
  onNavigate?: () => void;
}

interface ASidebarProps {
  onNavigate?: () => void;
}

// Akademisyen arayüzü için tema renkleri - Uygulama genelinde kullanılabilir
export const themeColors = {
  primary: {
    main: 'bg-primary text-white', // Ana arka plan rengi
    dark: 'bg-primary-dark', // Koyu arka plan rengi
    light: 'bg-primary-light', // Açık arka plan rengi
    hover: 'hover:bg-primary-dark/25', // Hover durumu rengi
    active: 'bg-primary shadow-sm ring-1 ring-white/10', // Aktif durum rengi
    text: 'text-primary', // Ana metin rengi
    textLight: 'text-white', // Açık metin rengi
    textMuted: 'text-gray-300', // Hafif metin rengi
    textDark: 'text-gray-700', // Koyu metin rengi
    border: 'border-white/5', // Kenarlık rengi
  },
  accent: {
    main: 'bg-gradient-to-r from-green-400/70 to-green-400/90', // Vurgu rengi (gradyan)
    text: 'text-primary', // Vurgu metni rengi
    hover: 'hover:shadow-md hover:ring-1 hover:ring-white/30', // Vurgu hover rengi
    border: 'border-white/10', // Vurgu kenarlığı
  },
  ui: {
    card: 'bg-white rounded-lg border border-gray-200/80 shadow-sm hover:shadow-md transition-all duration-300', // Kart stili
    divider: 'bg-gradient-to-r from-transparent via-gray-200 to-transparent', // Ayırıcı stili
    sectionDivider: 'bg-gradient-to-r from-transparent via-white/10 to-transparent', // Bölüm ayırıcı stili
    cardBody: 'p-4', // Kart içi padding
  }
};

// Stillendirme yardımcı fonksiyonları - Akademisyen arayüzü genelinde kullanılabilir
export const getStyles = {
  // Normal navigasyon öğeleri için aktif ve pasif stiller
  navItem: (active: boolean) => ({
    container: `flex items-center px-4 py-2.5 max-h-screen-740:py-1.5 text-xs font-medium mx-2 my-0.5 max-h-screen-740:my-0 rounded-md transition-all duration-200 ease-out ${
      active 
        ? 'text-white bg-primary shadow-sm ring-1 ring-white/10' 
        : 'text-gray-300 hover:text-white hover:bg-primary-dark/25 hover:ring-1 hover:ring-white/5'
    }`,
    icon: `mr-2.5 transition-colors duration-200 ease-out ${
      active ? 'text-white' : 'text-gray-400'
    }`,
    text: `transition-colors duration-200 ease-out ${
      active ? 'font-bold' : ''
    }`
  }),

  // Kartlar ve bölümler için stiller
  cards: {
    container: `${themeColors.ui.card}`,
    header: `px-4 py-3 ${themeColors.primary.main} backdrop-blur-sm`,
    body: `${themeColors.ui.cardBody}`,
    divider: `h-px ${themeColors.ui.divider} my-2`
  },
  
  // Bölüm başlıkları için stiller
  sectionTitle: `px-3 text-[11px] font-semibold text-gray-200 uppercase tracking-wider drop-shadow-sm`
};

// Sidebar başlık bileşeni
const NavSection = ({ title }: { title: string }) => (
  <div className="mt-6 max-h-screen-740:mt-2 max-h-screen-740:mb-1 px-2">
    <h2 className={getStyles.sectionTitle}>{title}</h2>
    <div className="h-px bg-gradient-to-r from-transparent via-white/10 to-transparent mt-1.5 mb-2.5 max-h-screen-740:mt-0.5 max-h-screen-740:mb-1"></div>
  </div>
);

// Sidebar navigasyon öğesi bileşeni
const NavItem = ({ icon, label, to, active, onNavigate }: NavItemProps) => {
  const styles = getStyles.navItem(active || false);
  
  const handleClick = () => {
    if (onNavigate) onNavigate();
  };
  
  return (
    <Link to={to} className={styles.container} onClick={handleClick}>
      <span className={styles.icon}>{icon}</span>
      <span className={styles.text}>{label}</span>
    </Link>
  );
};

// Hızlı Erişim Bölümü için bileşen
const HizliErisimBolumu = ({ onNavigate }: { onNavigate?: () => void }) => {
  const location = useLocation();
  const currentPath = location.pathname;
  
  // Aktif sayfa kontrolü için
  const isActive = (path: string) => currentPath === path;
  
  return (
    <>
      <NavSection title="HIZLI ERİŞİM" />
      <nav className="space-y-1 max-h-screen-740:space-y-0.5 mb-4 max-h-screen-740:mb-1.5">
        <NavItem 
          icon={<Users size={15} strokeWidth={1.5} />} 
          label="Öğrenci Listesi" 
          to="/akademisyen/ogrenciler" 
          active={isActive("/akademisyen/ogrenciler")}
          onNavigate={onNavigate}
        />
      </nav>
    </>
  );
};

// Üniversite ve site başlık bileşeni - birleştirilmiş tasarım
const HeaderSection = () => (
  <div className={`mx-2 mt-3 max-h-screen-740:mt-1.5 mb-3 max-h-screen-740:mb-1.5 overflow-hidden rounded-md shadow-md hover:shadow-lg transition-all duration-300`}>
    {/* Üniversite Başlık */}
    <div className="flex items-center justify-start px-4 py-2.5 max-h-screen-740:py-1.5 bg-white/95 backdrop-blur-sm relative">
      <div className="flex items-center justify-center w-5 h-5 max-h-screen-740:w-4 max-h-screen-740:h-4 rounded-full overflow-hidden ring-1 ring-gray-100/40 shadow-sm">
        <img 
          src={beunLogo} 
          alt="Bülent Ecevit Üniversitesi" 
          className="w-full h-full object-contain transform hover:scale-105 transition-transform duration-300"
        />
      </div>
      <div className="ml-2">
        <span className="text-primary text-xs font-semibold tracking-wide drop-shadow-sm">Bülent Ecevit Üniversitesi</span>
      </div>
    </div>
      {/* Site Başlık */}
      <div className={`flex items-center justify-end px-4 py-1.5 max-w-screen-768:py-1 ${themeColors.accent.main} border-t ${themeColors.accent.border} backdrop-blur-sm relative md:hidden`}>
      <span className="text-primary-dark text-[10px] font-medium tracking-wider drop-shadow-sm">
        akademikrandevu<span className="font-light">.com</span>
      </span>
    </div>
  </div>
);

// Profil menü bileşeni
const ProfileMenu = ({ onNavigate }: { onNavigate?: () => void }) => {
  const [isOpen, setIsOpen] = useState(false);
  const { user, logout } = useAuth();
  const [isLoggingOut, setIsLoggingOut] = useState(false);
  
  // Profil menüsü için kullanıcı adı
  const userFullName = user?.userfullname || 'Kullanıcı';
  
  const handleLogout = async () => {
    try {
      setIsLoggingOut(true);
      
      // Burada gerçek logout işlemleri yapılacak
      logout();
      if (onNavigate) onNavigate();
    } finally {
      setIsLoggingOut(false);
    }
  };
  
  return (
    <div className="mt-auto">
      <div className="mx-2 mb-3 max-h-screen-740:mb-1.5 overflow-hidden rounded-md shadow-md hover:shadow-lg transition-all duration-300">
        {/* Profil Butonu - Üniversite başlığa benzer tasarım */}
        <div className={`${themeColors.accent.main} w-full transition-all duration-200 ${isOpen ? 'rounded-t-md' : 'rounded-md'}`}>
          <button 
            onClick={() => setIsOpen(!isOpen)}
            className={`w-full flex items-center justify-between px-4 py-2.5 max-h-screen-740:py-1.5 text-xs font-semibold ${themeColors.accent.text} transition-all duration-200 ease-out hover:ring-1 hover:ring-white/20`}
          >
            <div className="flex items-center">
              <div className="flex items-center justify-center w-5 h-5 max-h-screen-740:w-4 max-h-screen-740:h-4 rounded-full bg-white/10 backdrop-blur-sm shadow-inner">
                <UserRoundCog size={14} strokeWidth={2} className="text-primary max-h-screen-740:scale-90" />
              </div>
              <span className="ml-2 tracking-wide truncate max-w-[140px]">{userFullName}</span>
            </div>
            <ChevronUp 
              size={14} 
              className={`text-primary transition-transform duration-200 ${isOpen ? 'transform rotate-180' : 'transform rotate-0'}`} 
            />
          </button>

        </div>
        
        {/* Dropdown menü - HeaderSection alt kısmına benzer gradient tasarım */}
        <div className={`overflow-hidden transition-all duration-200 ease-out ${
          isOpen ? 'max-h-24 max-h-screen-740:max-h-20 opacity-100' : 'max-h-0 opacity-0'
        }`}>
          <div className="bg-gradient-to-r from-primary-dark/30 to-primary-dark/50 border-t border-white/10">
            {/* Ayarlar */}
            <Link 
              to="/akademisyen/ayarlar"
              onClick={() => {
                if (onNavigate) onNavigate();
              }}
              className="flex items-center px-4 py-2 max-h-screen-740:py-1.5 text-xs text-white hover:bg-primary-dark/60 hover:pl-5 transition-all duration-200"
            >
              <Settings size={14} strokeWidth={1.75} className="mr-2.5 text-gray-200 max-h-screen-740:scale-90" />
              <span>Ayarlar</span>
            </Link>
            
            {/* Çıkış Yap */}
            <button
              onClick={handleLogout}
              disabled={isLoggingOut}
              className="w-full flex items-center px-4 py-2 max-h-screen-740:py-1.5 text-xs text-white hover:bg-primary-dark/60 hover:pl-5 transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isLoggingOut ? (
                <div className="mr-2.5 h-3.5 w-3.5 animate-spin rounded-full border-2 border-t-transparent border-white"></div>
              ) : (
                <LogOut size={14} strokeWidth={1.75} className="mr-2.5 text-gray-200 max-h-screen-740:scale-90" />
              )}
              <span>{isLoggingOut ? 'Çıkış Yapılıyor...' : 'Çıkış Yap'}</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

const ASidebar = ({ onNavigate }: ASidebarProps) => {
  // URL'den aktif sayfayı al
  const location = useLocation();
  const currentPath = location.pathname;
  
  // Aktif sayfaları belirle
  const isActive = (path: string) => currentPath === path;

  return (
    <div className={`flex flex-col h-full py-3 max-h-screen-740:py-2 px-2 font-poppins ${themeColors.primary.main} border-r ${themeColors.primary.border} shadow-lg`}>
      {/* Üniversite ve Site Başlık */}
      <HeaderSection />
      
      {/* Arayüz Bölümü */}
      <NavSection title="Arayüz" />
      <nav className="space-y-1 max-h-screen-740:space-y-0.5 mb-4 max-h-screen-740:mb-1.5">
        <NavItem 
          icon={<Home size={15} strokeWidth={1.5} />} 
          label="Anasayfa" 
          to="/akademisyen/anasayfa" 
          active={isActive("/akademisyen/anasayfa")} 
          onNavigate={onNavigate}
        />
        <NavItem 
          icon={<Dock size={15} strokeWidth={1.5} />} 
          label="Randevular" 
          to="/akademisyen/randevular" 
          active={isActive("/akademisyen/randevular")}
          onNavigate={onNavigate}
        />
      </nav>
      
      {/* Hızlı Erişim Bölümü */}
      <HizliErisimBolumu onNavigate={onNavigate} />
      
      {/* Profil Menü - En altta */}
      <ProfileMenu onNavigate={onNavigate} />
    </div>
  );
};

export default ASidebar;

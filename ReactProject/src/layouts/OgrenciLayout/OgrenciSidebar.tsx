import React, { useState, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { 
  Home, 
  LibraryBig, 
  Dock, 
  Book, 
  Search,
  UserRoundCog,
  Settings,
  LogOut,
  ChevronUp
} from 'lucide-react';
import beunLogo from '../../assets/logo/SidebarLogo/beunlogo.png';
import AkademisyenLinkItem from '../../components/Ogrenci/AkademisyenLinkItem';
import { getAkademisyenlerByBolumId, Akademisyen } from '../../services/Ogrenci/departmentService';
import { useAuth } from '../../contexts/AuthContext';
import { getUserOkulBolumBilgisi } from '../../services/Ogrenci/schoolDepartmentService';
import { ogrenciCikis } from '../../services/Ogrenci/authService';

interface NavItemProps {
  icon: React.ReactNode;
  label: string;
  to: string;
  active?: boolean;
  onNavigate?: () => void;
}

interface OgrenciSidebarProps {
  onNavigate?: () => void;
  openSearchModal?: () => void;
}

// Öğrenci arayüzü için tema renkleri - Uygulama genelinde kullanılabilir
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

// Stillendirme yardımcı fonksiyonları - Öğrenci arayüzü genelinde kullanılabilir
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

// Akademisyenler listesi bileşeni (dinamik veri ile)
const AkademisyenlerListesi = () => {
  const { user } = useAuth();
  const [akademisyenler, setAkademisyenler] = useState<Akademisyen[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    // Kullanıcı ve departmentId kontrolü
    if (!user || !user.departmentId) {
      setIsLoading(false);
      setError("Bölüm bilgisi bulunamadı");
      return;
    }
    
    const fetchAkademisyenler = async () => {
      try {
        // Kullanıcının departmentId'sine göre akademisyenleri getir
        const data = await getAkademisyenlerByBolumId(user.departmentId);
        
        // Akademisyen verilerinin doğru formatta olmasını sağla
        const formattedData = data.map(akademisyen => {
          // İsim formatını düzenle (ünvan eklemesi yapılabilir)
          // API'den gelen ismin önünde ünvan yoksa, API tarafından gelen başka bilgilere göre eklenebilir
          let fullName = akademisyen.fullName;
          
          // Eğer API'den gelen ismin önünde ünvan yoksa
          if (!fullName.includes("Prof.") && 
              !fullName.includes("Doç.") && 
              !fullName.includes("Dr.") &&
              !fullName.includes("Arş.") &&
              !fullName.includes("Öğr.")) {
            // Varsayılan olarak Arş. Gör. ekleyelim (Bu kısım API'deki gerçek veriye göre düzenlenmelidir)
            // Burada sadece örnek amaçlı ekliyoruz
            fullName = `Arş. Gör. ${fullName}`;
          }
          
          return {
            ...akademisyen,
            fullName
          };
        });
        
        setAkademisyenler(formattedData);
        setIsLoading(false);
      } catch (err) {
        console.error("Akademisyenler yüklenirken hata:", err);
        setError("Akademisyenler yüklenemedi");
        setIsLoading(false);
      }
    };
    
    fetchAkademisyenler();
  }, [user]);
  
  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-4">
        <div className="h-4 w-4 animate-spin rounded-full border-2 border-t-transparent border-white"></div>
        <span className="ml-2 text-xs text-gray-300">Yükleniyor...</span>
      </div>
    );
  }
  
  if (error) {
    return (
      <div className="text-xs text-gray-400 text-center py-3">
        {error}
      </div>
    );
  }
  
  if (akademisyenler.length === 0) {
    return (
      <div className="text-xs text-gray-400 text-center py-3">
        Bu bölüme ait akademisyen bulunamadı
      </div>
    );
  }
  
  return (
    <>
      {akademisyenler.map((akademisyen) => (
        <AkademisyenLinkItem 
          key={akademisyen.id}
          akademisyen={akademisyen}
          displayType="sidebar"
        />
      ))}
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
      
      // Token alma
      const token = localStorage.getItem('authToken');
      
      // API çıkış işlemini gerçekleştir
      await ogrenciCikis(
        token,
        // Başarı callback'i
        () => {
          // Context çıkış fonksiyonunu çağır
          logout();
          if (onNavigate) onNavigate();
        },
        // Hata callback'i
        (errorMessage) => {
          console.error('Çıkış hatası:', errorMessage);
          // Hata olsa bile context çıkış yap
          logout();
          if (onNavigate) onNavigate();
        }
      );
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
              to="/ogrenci/ayarlar"
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

// Akademisyenler listesi için dinamik yükseklik hesaplama fonksiyonu
const useResponsiveHeight = () => {
  const [akademisyenListHeight, setAkademisyenListHeight] = useState("8.5rem");
  
  useEffect(() => {
    const updateHeight = () => {
      if (window.innerHeight <= 740) {
        setAkademisyenListHeight("8rem"); // Mobil görünüm için daha fazla yükseltildi
      } else if (window.innerHeight <= 800) {
        setAkademisyenListHeight("8.5rem"); // Orta boyut için yükseltildi
      } else {
        setAkademisyenListHeight("9rem"); // Büyük ekranlar için de biraz yükseltildi
      }
    };
    
    updateHeight();
    window.addEventListener("resize", updateHeight);
    return () => window.removeEventListener("resize", updateHeight);
  }, []);
  
  return akademisyenListHeight;
};

// Hızlı Erişim Bölümü için bileşen
const HizliErisimBolumu = ({ onNavigate, openSearchModal }: { onNavigate?: () => void; openSearchModal?: () => void }) => {
  const { user } = useAuth();
  const [bolumBilgisi, setBolumBilgisi] = useState<{id: string, name: string, url: string} | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const location = useLocation();
  const currentPath = location.pathname;
  
  // Giriş yapan öğrencinin bölüm bilgisini al
  useEffect(() => {
    const fetchBolumBilgisi = async () => {
      try {
        if (user) {
          const { bolum } = await getUserOkulBolumBilgisi(user);
          setBolumBilgisi(bolum);
        } else {
          // Kullanıcı yoksa varsayılan değerler
          setBolumBilgisi({
            id: "1",
            name: "Bilgisayar Mühendisliği",
            url: "/ogrenci/bolumler/1"
          });
        }
      } catch (error) {
        console.error("Bölüm bilgisi yüklenirken hata:", error);
        // Hata durumunda varsayılan değerler
        setBolumBilgisi({
          id: "1",
          name: "Bilgisayar Mühendisliği",
          url: "/ogrenci/bolumler/1"
        });
      } finally {
        setIsLoading(false);
      }
    };
    
    fetchBolumBilgisi();
  }, [user]);
  
  // Aktif sayfa kontrolü için
  const isActive = (path: string) => currentPath === path;
  
  return (
    <>
      <NavSection title="HIZLI ERİŞİM" />
      <nav className="space-y-1 max-h-screen-740:space-y-0.5 mb-4 max-h-screen-740:mb-1.5">
        {isLoading ? (
          // Yükleniyor durumu
          <div className="flex items-center px-4 py-2.5 max-h-screen-740:py-1.5 text-xs font-medium mx-2 my-0.5 max-h-screen-740:my-0 text-gray-300">
            <div className="w-4 h-4 mr-2 border-2 border-t-transparent border-gray-300 rounded-full animate-spin"></div>
            <span>Yükleniyor...</span>
          </div>
        ) : bolumBilgisi ? (
          // Bölüm butonu - dinamik
          <NavItem 
            icon={<Book size={15} strokeWidth={1.5} />} 
            label={bolumBilgisi.name}
            to={bolumBilgisi.url}
            active={isActive(bolumBilgisi.url)}
            onNavigate={onNavigate}
          />
        ) : (
          // Fallback - varsayılan
          <NavItem 
            icon={<Book size={15} strokeWidth={1.5} />} 
            label="Bilgisayar Mühendisliği"
            to="/ogrenci/bolumler/1"
            active={isActive("/ogrenci/bolumler/1")}
            onNavigate={onNavigate}
          />
        )}
        
        {/* Akademisyen Bul butonu - artık modal açacak */}
        <div className={getStyles.navItem(isActive("/ogrenci/akademisyen-bul")).container}>
          <button 
            onClick={() => openSearchModal && openSearchModal()}
            className="flex items-center w-full"
          >
            <span className={getStyles.navItem(isActive("/ogrenci/akademisyen-bul")).icon}>
              <Search size={15} strokeWidth={1.5} />
            </span>
            <span className={getStyles.navItem(isActive("/ogrenci/akademisyen-bul")).text}>
              Akademisyen Bul
            </span>
          </button>
        </div>
      </nav>
    </>
  );
};

const OgrenciSidebar = ({ onNavigate, openSearchModal }: OgrenciSidebarProps) => {
  // URL'den aktif sayfayı al
  const location = useLocation();
  const currentPath = location.pathname;
  
  // Aktif sayfaları belirle
  const isActive = (path: string) => currentPath === path;
  
  // Akademisyenler listesi için dinamik yükseklik
  const akademisyenListHeight = useResponsiveHeight();


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
          to="/ogrenci/anasayfa" 
          active={isActive("/ogrenci/anasayfa")} 
          onNavigate={onNavigate}
        />
        <NavItem 
          icon={<LibraryBig size={15} strokeWidth={1.5} />} 
          label="Bölümler" 
          to="/ogrenci/bolumler" 
          active={isActive("/ogrenci/bolumler")} 
          onNavigate={onNavigate}
        />
        <NavItem 
          icon={<Dock size={15} strokeWidth={1.5} />} 
          label="Randevular" 
          to="/ogrenci/randevular" 
          active={isActive("/ogrenci/randevular")}
          onNavigate={onNavigate}
        />
      </nav>
      
      {/* Hızlı Erişim Bölümü - openSearchModal prop'unu geçiriyoruz */}
      <HizliErisimBolumu onNavigate={onNavigate} openSearchModal={openSearchModal} />
      
      {/* Bölüm Akademisyenleri */}
      <NavSection title="BÖLÜM AKADEMİSYENLERİ" />
      <div 
        className="space-y-1 max-h-screen-740:space-y-0.5 overflow-y-auto pr-1 mx-1 scrollbar-thin scrollbar-thumb-white/20 scrollbar-track-primary-dark/10 rounded-md"
        style={{ maxHeight: akademisyenListHeight }}
      >
        <AkademisyenlerListesi />
      </div>
      
      {/* Profil Menü - En altta */}
      <ProfileMenu onNavigate={onNavigate} />
    </div>
  );
};

export default OgrenciSidebar;

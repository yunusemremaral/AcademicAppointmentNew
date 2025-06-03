import React, { useState, useRef, useEffect } from 'react';
import { themeColors } from '../../layouts/OgrenciLayout/OgrenciSidebar';
import { Key, Mail, Bell, UserRoundCog, Shield, ChevronRight, Settings } from 'lucide-react';
import SifreDegistirme from '../../components/Ogrenci/Ayarlar/SifreDegistirme';
import EmailDegistirme from '../../components/Ogrenci/Ayarlar/EmailDegistirme';
import BildirimAyarlari from '../../components/Ogrenci/Ayarlar/BildirimAyarlari';
import { useAuth } from '../../contexts/AuthContext';

// Ayarlar sekmeleri için tip tanımlaması
type AyarSekmesi = 'sifre' | 'email' | 'bildirimler' | 'kisisel' | null;

const Ayarlar: React.FC = () => {
  const { user } = useAuth();
  // Aktif sekmeyi takip eden state - başlangıçta null (hiçbir sekme seçili değil)
  const [aktivSekme, setAktivSekme] = useState<AyarSekmesi>(null);
  // İçerik bölümü için ref
  const icerikRef = useRef<HTMLDivElement>(null);
  // Mobil görünüm kontrolü için state
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);

  // Ekran boyutu değişikliklerini izle
  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth < 768);
    };
    
    window.addEventListener('resize', handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  // Sekme değiştiğinde içeriğe kaydır (mobil görünümde)
  useEffect(() => {
    if (isMobile && aktivSekme && icerikRef.current) {
      // Kaydırma işlemi için timeout ekle (animasyon tamamlandıktan sonra)
      setTimeout(() => {
        icerikRef.current?.scrollIntoView({ 
          behavior: 'smooth', 
          block: 'start'
        });
      }, 100);
    }
  }, [aktivSekme, isMobile]);

  // Aktif içeriği seçme fonksiyonu
  const renderAktifIcerik = () => {
    switch (aktivSekme) {
      case 'sifre':
        return <SifreDegistirme />;
      case 'email':
        return <EmailDegistirme />;
      case 'bildirimler':
        return <BildirimAyarlari />;
      case 'kisisel':
        return (
          <div className="p-4 bg-white/5 rounded-xl backdrop-blur-sm border border-white/10 h-full">
            <div className="flex items-center mb-3">
              <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                <Shield size={16} className={themeColors.primary.textLight} />
              </div>
              <h3 className={`${themeColors.primary.textLight} font-medium text-base`}>Kişisel Bilgiler</h3>
            </div>
            <p className={`${themeColors.primary.textLight}/80 text-xs font-light mb-6`}>
              Bu bölüm yapım aşamasındadır. Yakında kullanıma açılacaktır.
            </p>
          </div>
        );
      default:
        // Hiçbir sekme seçili değilse boş içerik göster
        return (
          <div className="p-4 bg-white/5 rounded-xl backdrop-blur-sm border border-white/10 h-full flex flex-col items-center justify-center">
            <Settings size={40} className={`${themeColors.primary.textLight}/30 mb-3`} />
            <h3 className={`${themeColors.primary.textLight} text-base font-medium mb-2 text-center`}>
              Hesap Ayarları
            </h3>
            <p className={`${themeColors.primary.textLight}/70 text-xs font-light text-center max-w-md`}>
              {isMobile 
                ? "Ayarlarınızı görüntülemek için yukarıdaki menüden bir seçenek seçin." 
                : "Ayarlarınızı görüntülemek için soldaki menüden bir seçenek seçin."}
            </p>
          </div>
        );
    }
  };

  // Sekme değiştirme fonksiyonu
  const handleSekmeChange = (sekme: AyarSekmesi) => {
    // Eğer zaten seçili olan sekmeye tıklandıysa, seçimi kaldır
    if (aktivSekme === sekme) {
      setAktivSekme(null);
    } else {
      setAktivSekme(sekme);
    }
  };

  return (
    <div className="py-4 px-3 sm:p-5 lg:p-6 max-w-full font-poppins bg-gray-50/5">
      {/* Ayarlar Başlık Kartı - Bolumler.tsx ve Randevular.tsx ile uyumlu hale getirildi */}
      <div className={`p-3 sm:p-4 ${themeColors.primary.main} rounded-xl shadow-md mb-4 backdrop-blur-sm bg-opacity-95`}>
        <div className="flex items-center">
          <UserRoundCog size={16} className="text-white mr-2.5" />
          <h1 className="text-base sm:text-lg font-semibold text-white">
            Hesap Ayarları
            {user?.userfullname && (
              <>
                <span className="mx-1.5 opacity-75">|</span>
                <span className="font-normal">{user.userfullname}</span>
              </>
            )}
          </h1>
        </div>
        <p className="text-white/90 text-xs mt-1.5 max-w-2xl">
          Hesap ayarlarınızı ve tercihlerinizi bu sayfadan yönetebilirsiniz.
        </p>
      </div>

      {/* Ana İçerik Grid Düzeni */}
      <div className="grid grid-cols-1 md:grid-cols-12 gap-4">
        {/* Sol Menü - Ayar Sekmeleri */}
        <div className={`${themeColors.primary.main} rounded-xl overflow-hidden md:col-span-4 backdrop-blur-sm ${themeColors.primary.border} shadow-md`}>
          <div className="p-4 relative h-full">
            <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-15"></div>
            
            <div className="relative h-full flex flex-col">
              <div className="flex items-center mb-3">
                <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                  <Settings size={16} className={themeColors.primary.textLight} />
                </div>
                <h3 className={`${themeColors.primary.textLight} font-medium text-base`}>
                  Ayarlar Menüsü
                </h3>
              </div>
              
              {/* Menü içeriği - Sabit yükseklik ve scroll ekledim */}
              <div className="grid grid-cols-1 gap-2 overflow-y-auto max-h-[300px] md:max-h-[400px] pr-1 scrollbar-thin scrollbar-thumb-white/20 scrollbar-track-primary-dark/10">
                {/* Şifre Değiştirme */}
                <button 
                  onClick={() => handleSekmeChange('sifre')}
                  className={`flex items-center justify-between text-left p-3 rounded-lg backdrop-blur-sm ${aktivSekme === 'sifre' ? 'bg-white/15' : 'bg-white/5'} ${themeColors.primary.border} transition-colors duration-200`}
                >
                  <div className="flex items-center">
                    <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                      <Key size={14} className={themeColors.primary.textLight} />
                    </div>
                    <span className={`${themeColors.primary.textLight} text-xs font-medium`}>Şifre Değiştir</span>
                  </div>
                  <ChevronRight size={14} className={`${themeColors.primary.textLight}/60 transform transition-transform duration-200 ${aktivSekme === 'sifre' ? 'rotate-90' : ''}`} />
                </button>
                
                {/* Email Değiştirme */}
                <button 
                  onClick={() => handleSekmeChange('email')}
                  className={`flex items-center justify-between text-left p-3 rounded-lg backdrop-blur-sm ${aktivSekme === 'email' ? 'bg-white/15' : 'bg-white/5'} ${themeColors.primary.border} transition-colors duration-200`}
                >
                  <div className="flex items-center">
                    <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                      <Mail size={14} className={themeColors.primary.textLight} />
                    </div>
                    <span className={`${themeColors.primary.textLight} text-xs font-medium`}>E-posta Değiştir</span>
                  </div>
                  <ChevronRight size={14} className={`${themeColors.primary.textLight}/60 transform transition-transform duration-200 ${aktivSekme === 'email' ? 'rotate-90' : ''}`} />
                </button>
                
                {/* Bildirim Ayarları */}
                <button 
                  onClick={() => handleSekmeChange('bildirimler')}
                  className={`flex items-center justify-between text-left p-3 rounded-lg backdrop-blur-sm ${aktivSekme === 'bildirimler' ? 'bg-white/15' : 'bg-white/5'} ${themeColors.primary.border} transition-colors duration-200`}
                >
                  <div className="flex items-center">
                    <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                      <Bell size={14} className={themeColors.primary.textLight} />
                    </div>
                    <span className={`${themeColors.primary.textLight} text-xs font-medium`}>Bildirim Ayarları</span>
                  </div>
                  <ChevronRight size={14} className={`${themeColors.primary.textLight}/60 transform transition-transform duration-200 ${aktivSekme === 'bildirimler' ? 'rotate-90' : ''}`} />
                </button>
                
                {/* Kişisel Bilgiler */}
                <button 
                  onClick={() => handleSekmeChange('kisisel')}
                  className={`flex items-center justify-between text-left p-3 mb-3 rounded-lg backdrop-blur-sm ${aktivSekme === 'kisisel' ? 'bg-white/15' : 'bg-white/5'} ${themeColors.primary.border} transition-colors duration-200`}
                >
                  <div className="flex items-center">
                    <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                      <Shield size={14} className={themeColors.primary.textLight} />
                    </div>
                    <span className={`${themeColors.primary.textLight} text-xs font-medium`}>Kişisel Bilgiler</span>
                  </div>
                  <ChevronRight size={14} className={`${themeColors.primary.textLight}/60 transform transition-transform duration-200 ${aktivSekme === 'kisisel' ? 'rotate-90' : ''}`} />
                </button>
              </div>
              
              {/* Alt bilgi - Her zaman altta kalacak şekilde ayarlandı */}
              <div className={`mt-auto pt-3 p-3 bg-white/5 rounded-lg text-center ${themeColors.primary.border}`}>
                <p className={`${themeColors.primary.textLight}/80 text-[11px] font-light`}>
                  Son güncelleme: {new Date().toLocaleDateString('tr-TR')}
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Sağ İçerik - Seçilen Ayar İçeriği */}
        <div 
          ref={icerikRef} 
          className={`${themeColors.primary.main} rounded-xl overflow-hidden md:col-span-8 backdrop-blur-sm ${themeColors.primary.border} shadow-md scroll-mt-4`}
        >
          <div className="p-4 relative h-full">
            <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-15"></div>
            
            <div className="relative h-full">
              {renderAktifIcerik()}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Ayarlar;

import React from 'react';
import { useLocation } from 'react-router-dom';

interface AHeaderProps {
  isMobile?: boolean;
  onBackClick?: () => void;
}

// Sayfa başlık işaretleyicisi bileşeni - ASidebar'daki NavSection'a benzer
const HeaderTitle = ({ title, isMobile = false }: { title: string; isMobile?: boolean }) => (
  <div className="flex items-center">
    <h2 className={`font-semibold text-white tracking-wide drop-shadow-sm ${isMobile ? 'text-xs' : 'text-xs'}`}>{title}</h2>
  </div>
);

const AHeader: React.FC<AHeaderProps> = ({ 
  isMobile = false, 
  onBackClick 
}) => {
  const location = useLocation();
  
  // Sayfa başlığını belirle
  const getPageTitle = () => {
    const path = location.pathname;
    
    if (path.includes('anasayfa')) return 'Ana Sayfa';
    if (path.includes('bolumler')) return 'Bölümler';
    if (path.includes('randevular')) return 'Randevular';
    if (path.includes('ogrenciler')) return 'Öğrenciler';
    return 'Ana Sayfa';
  };
  
  return (
    <div className="w-full mt-0 md:mt-2 font-poppins">
      {/* Üst kısım */}
      <div className="flex flex-col rounded-lg border border-gray-200/80 shadow-sm overflow-hidden bg-white hover:shadow-md transition-all duration-300">
        {/* Header Başlık */}
        <div className="flex items-center px-4 py-3 bg-primary text-white backdrop-blur-sm">
          {isMobile ? (
            // Mobil düzen - üç bölümlü grid yapısı ile daha dengeli bir görünüm
            <div className="grid grid-cols-3 w-full items-center">
              {/* Sol: Geri butonu */}
              <div className="flex justify-start items-center">
                {onBackClick && (
                  <button 
                    onClick={onBackClick} 
                    className="text-white hover:text-gray-200 transition-colors"
                    aria-label="Geri dön"
                  >
                    <svg viewBox="0 0 32 32" width="18" height="18" className="text-white fill-current">
                      <path d="m28 4h-24a2 2 0 0 0 -2 2v20a2 2 0 0 0 2 2h24a2 2 0 0 0 2-2v-20a2 2 0 0 0 -2-2zm0 22h-16v-20h16z"/>
                      <path d="m0 0h32v32h-32z" fill="none"/>
                    </svg>
                  </button>
                )}
              </div>
              
              {/* Orta: Sayfa başlığı */}
              <div className="flex justify-center items-center">
                <HeaderTitle title={getPageTitle()} isMobile={true} />
              </div>
              
              {/* Sağ: Butonlar */}
              <div className="flex justify-end items-center space-x-1">
                {/* Boş alan, arama butonu kaldırıldı */}
              </div>
            </div>
          ) : (
            // Desktop düzeni
            <div className="flex items-center justify-between w-full">
              <HeaderTitle title={getPageTitle()} />
              
              <div className="text-[10px] font-medium tracking-wider opacity-80">
                akademikrandevu<span className="font-light">.com</span>
              </div>
            </div>
          )}
        </div>
        
        {/* Alt kısım - İşlevsel Alan (Sadece desktop görünümünde) */}
        {!isMobile && (
          <div className="flex items-center justify-between px-4 py-2.5 border-t border-gray-100">
            {/* Arama çubuğu kaldırıldı, yüksekliği korumak için boş alan */}
            <div className="w-full h-8 flex items-center justify-between">
              <div className="text-xs text-gray-400">
                {/* Yüksekliği korumak için boş alan */}
              </div>
            </div>
          </div>
        )}
      </div>
      
      {/* İnce Ayırıcı Çizgi - ASidebar'daki NavSection'a benzer */}
      <div className="h-px bg-gradient-to-r from-transparent via-gray-200 to-transparent mt-2"></div>
    </div>
  );
};

export default AHeader;

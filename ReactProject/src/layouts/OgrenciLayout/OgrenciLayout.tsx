import { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import OgrenciSidebar from './OgrenciSidebar';
import OgrenciHeader from './OgrenciHeader';
import OgrenciMain from './OgrenciMain';
import { SearchProvider, useSearch } from '../../contexts/SearchContext';
import SearchModal from '../../components/Ogrenci/Layout/OgrenciHeader/SearchModal';

const OgrenciLayout = () => {
  const location = useLocation();
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);
  const [showSidebar, setShowSidebar] = useState(true);
  const [isSearching, setIsSearching] = useState(false);
  
  // Ekran boyutu değiştiğinde mobil durumunu güncelle
  useEffect(() => {
    const handleResize = () => {
      const mobile = window.innerWidth < 768;
      setIsMobile(mobile);
      // Masaüstüne geçildiğinde sidebar'ı ve içeriği göster
      if (!mobile) {
        setShowSidebar(true);
      }
    };
    
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);
  
  // URL değişimini takip et
  useEffect(() => {
    // URL değişikliklerini izle (gelecekteki özellikler için)
    setIsSearching(false);
  }, [location.pathname]);
  
  // Sidebar'da link tıklaması için fonksiyon
  const handleNavigation = () => {
    if (isMobile) {
      setShowSidebar(false);
    }
  };
  
  // Mobil görünümde geri butonu
  const handleBackToMenu = () => {
    setShowSidebar(true);
  };
  
  // Arama işlemi gerçekleştiğinde
  const handleSearch = (query: string) => {
    console.log('Arama yapılıyor:', query);
    setIsSearching(true);
    // API entegrasyonu burada yapılacak
  };
  
  return (
    <SearchProvider>
      <OgrenciLayoutContent 
        isMobile={isMobile} 
        showSidebar={showSidebar} 
        isSearching={isSearching}
        handleNavigation={handleNavigation}
        handleBackToMenu={handleBackToMenu}
        handleSearch={handleSearch}
      />
    </SearchProvider>
  );
};

// SearchContext erişimi için iç bileşen
const OgrenciLayoutContent = ({ 
  isMobile, 
  showSidebar, 
  isSearching,
  handleNavigation,
  handleBackToMenu,
  handleSearch
}: { 
  isMobile: boolean;
  showSidebar: boolean;
  isSearching: boolean;
  handleNavigation: () => void;
  handleBackToMenu: () => void;
  handleSearch: (query: string) => void;
}) => {
  // SearchContext'ten modalı kontrol eden fonksiyonları al
  const { setIsModalOpen } = useSearch();
  
  // Akademisyen arama modalını açan fonksiyon
  const openSearchModal = () => {
    setIsModalOpen(true);
  };

  return (
    <div className="flex h-screen bg-white overflow-hidden">
      {/* Sol taraf - Sidebar */}
      <div className={`${isMobile ? 'fixed inset-0 z-30' : 'w-72'} h-full flex-shrink-0 transition-all duration-300 ease-in-out ${isMobile && !showSidebar ? '-translate-x-full' : 'translate-x-0'}`}>
        <OgrenciSidebar 
          onNavigate={handleNavigation} 
          openSearchModal={openSearchModal} 
        />
      </div>
      
      {/* Sağ taraf - Ana içerik alanı */}
      <div className={`flex-1 flex flex-col transition-all duration-300 ease-in-out ${isMobile ? 'w-full' : ''} ${isMobile && showSidebar ? 'opacity-0 pointer-events-none' : 'opacity-100 pointer-events-auto'}`}>
        {/* Header bölümü - Tam genişlik */}
        <div className={`${isMobile ? 'pt-3' : 'pt-2'} px-3`}>
          <OgrenciHeader 
            isMobile={isMobile} 
            onBackClick={isMobile && !showSidebar ? handleBackToMenu : undefined} 
          />
        </div>
        
        {/* Header ile içerik arasında boşluk - Mobilde daha az boşluk */}
        <div className="-mt-2 "></div>
        
        {/* Ana içerik bölümü - OgrenciMain */}
        <OgrenciMain isSearching={isSearching} />
      </div>
      
      {/* Global Arama Modal - Tüm çözünürlükler için */}
      <SearchModal onSubmit={handleSearch} />
    </div>
  );
};

export default OgrenciLayout;


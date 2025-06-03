import React, { useCallback, useMemo, memo } from 'react';
import { Search } from 'lucide-react';
import { motion } from 'framer-motion';
import { useSearch } from '../../../../contexts/SearchContext';
import Modal from './SearchModal';

// Arama çubuğu stilleri
const getSearchStyles = {
  searchButton: (isFocused: boolean) => ({
    container: `flex items-center bg-white border border-gray-200 rounded-md overflow-hidden transition-all duration-200 ease-out ${
      isFocused 
        ? 'ring-1 ring-gray-300' 
        : 'hover:border-gray-300'
    }`,
    icon: `transition-colors duration-200 ease-out ${
      isFocused ? 'text-gray-700' : 'text-gray-400'
    }`
  })
};

// SearchBar bileşenleri - Mobil ve Masaüstü için
interface SearchBarProps {
  placeholder?: string;
  isMobile?: boolean;
  onSubmit?: (query: string) => void;
  schoolId?: number; // Okul ID'si - arama için gerekli
}

// Mobil Arama Çubuğu Bileşeni - Sadece tetikleyici buton
const MobileSearchBar = memo<SearchBarProps>(() => {
  const { setIsModalOpen } = useSearch();

  // Mobil arama modalını aç
  const handleOpenMobileSearch = useCallback(() => {
    setIsModalOpen(true);
  }, [setIsModalOpen]);

  return (
    <motion.button 
      onClick={handleOpenMobileSearch}
      className="flex items-center justify-center h-8 w-8 rounded-md text-white hover:bg-white/20 transition-colors duration-200"
      aria-label="Akademisyen Ara"
      whileTap={{ scale: 0.9 }}
      whileHover={{ scale: 1.05 }}
    >
      <Search size={16} />
    </motion.button>
  );
});

// Masaüstü Arama Çubuğu Bileşeni
const DesktopSearchBar = memo<SearchBarProps>(({ 
  placeholder = "Akademisyen ara...",
  onSubmit,
  schoolId = 1 // Varsayılan okul ID'si
}) => {
  const { 
    searchQuery, 
    isFocused, 
    setIsModalOpen
  } = useSearch();
  
  // useMemo ile stil hesaplamasını optimize et
  const searchStyles = useMemo(() => 
    getSearchStyles.searchButton(isFocused),
  [isFocused]);

  // Modal açma işleyicisi
  const handleOpenModal = useCallback(() => {
    setIsModalOpen(true);
  }, [setIsModalOpen]);

  // Click işleyicisi - inputa tıklayınca modal aç
  const handleInputClick = useCallback(() => {
    handleOpenModal();
  }, [handleOpenModal]);

  return (
    <div className="relative desktop-search-container">
      {/* Arama input alanı - Sadece tetikleyici olarak kullanılıyor */}
      <form onSubmit={(e) => { e.preventDefault(); handleOpenModal(); }} className="relative w-full max-w-lg">
        <motion.div 
          className={searchStyles.container}
          whileFocus={{ scale: 1.01 }}
          transition={{ type: "spring", stiffness: 400, damping: 25 }}
          onClick={handleInputClick}
        >
          <div className="flex items-center justify-center h-8 w-8">
            <Search size={14} className={searchStyles.icon} />
          </div>
          <input
            type="text"
            placeholder={placeholder}
            className="flex-1 h-8 py-1.5 pr-3 outline-none text-xs font-medium text-gray-700 placeholder-gray-400 bg-transparent cursor-pointer"
            value={searchQuery}
            onFocus={handleInputClick}
            readOnly
          />
        </motion.div>
      </form>
      
      {/* Modal */}
      <Modal 
        placeholder={placeholder}
        onSubmit={onSubmit}
        schoolId={schoolId}
        isMobile={false}
      />
    </div>
  );
});

// Ana komponent - props'lara göre uygun arama çubuğunu dön
const SearchBar: React.FC<SearchBarProps> = (props) => {
  return props.isMobile ? <MobileSearchBar {...props} /> : <DesktopSearchBar {...props} />;
};

export default SearchBar;

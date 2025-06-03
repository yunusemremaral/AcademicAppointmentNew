import React, { useEffect, useCallback, memo } from 'react';
import { Search, X, Book, User } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';
import { useSearch } from '../../../../contexts/SearchContext';
import { useNavigate } from 'react-router-dom';
import { SearchResult } from '../../../../services/Ogrenci/SearchService';

// Animasyon varyantları
const modalVariants = {
  hidden: { 
    opacity: 0, 
    y: -20,
    scale: 0.95
  },
  visible: { 
    opacity: 1, 
    y: 0,
    scale: 1,
    transition: { 
      type: "spring", 
      damping: 25, 
      stiffness: 300
    }
  },
  exit: { 
    opacity: 0, 
    y: 10, 
    scale: 0.95,
    transition: { 
      duration: 0.2, 
      ease: "easeInOut" 
    }
  }
};

// Sonuçlar için animasyon varyantları
const resultsVariants = {
  hidden: { 
    opacity: 0, 
    y: -10,
    height: 0,
    scale: 0.98
  },
  visible: { 
    opacity: 1, 
    y: 0,
    height: 'auto',
    scale: 1,
    transition: { 
      type: "spring", 
      damping: 25, 
      stiffness: 300
    }
  },
  exit: { 
    opacity: 0, 
    y: -10,
    height: 0,
    scale: 0.98,
    transition: { 
      duration: 0.2 
    }
  }
};

const backdropVariants = {
  hidden: { opacity: 0 },
  visible: { opacity: 1 },
  exit: { opacity: 0 }
};

// Ortak modal içeriklerini oluşturan bileşen
interface ModalContentProps {
  title?: string;
  onClose: () => void;
  placeholder: string;
  searchQuery: string;
  handleSubmitSearch: (e: React.FormEvent) => void;
  handleInputChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  clearSearch: () => void;
  onClose2?: () => void;
  isMobile?: boolean;
}

export const ModalContent = memo<ModalContentProps>(({
  title = "Akademisyen Ara",
  onClose,
  placeholder,
  searchQuery,
  handleSubmitSearch,
  handleInputChange,
  clearSearch,
  onClose2,
}) => {
  return (
    <div className="overflow-hidden rounded-xl border border-primary/20 shadow-lg bg-white">
      {/* Başlık ve kapatma butonu */}
      <div className="flex items-center justify-between px-4 py-3 bg-primary text-white">
        <div className="flex items-center">
          <Search size={16} className="mr-2 text-white/80" />
          <h3 className="text-xs font-semibold tracking-wide">{title}</h3>
        </div>
        <motion.button 
          onClick={onClose}
          className="flex items-center justify-center h-7 w-7 rounded-full text-white/80 hover:text-white hover:bg-white/10 transition-colors"
          whileTap={{ scale: 0.9 }}
        >
          <X size={16} />
        </motion.button>
      </div>
      
      {/* Arama input alanı */}
      <form onSubmit={handleSubmitSearch} className="px-4 py-4 bg-gray-50">
        <div className="relative bg-white rounded-lg shadow-sm ring-1 ring-gray-300/50 overflow-hidden">
          <div className="absolute top-0 left-0 flex items-center justify-center h-9 w-9 text-gray-400">
            <Search size={15} />
          </div>
          <input
            type="text"
            placeholder={placeholder}
            className="w-full h-9 pl-9 pr-9 py-1 text-sm text-gray-700 placeholder-gray-400 bg-transparent outline-none"
            value={searchQuery}
            onChange={handleInputChange}
            autoFocus
          />
          {searchQuery && (
            <motion.button 
              type="button"
              onClick={clearSearch}
              className="absolute top-0 right-0 h-9 w-9 flex items-center justify-center text-gray-400 hover:text-gray-700"
              whileTap={{ scale: 0.9 }}
            >
              <X size={14} />
            </motion.button>
          )}
        </div>
        
        {/* Bilgi kartı */}
        <div className="mt-3 text-center bg-blue-50 rounded-lg p-3 border border-blue-100 shadow-sm">
          <p className="text-xs text-blue-700">
            Akademisyen araması için ismen veya bölüm adıyla arama yapabilirsiniz
          </p>
        </div>
        
        {/* Arama butonu */}
        <motion.button
          type="submit"
          className="mt-3 w-full bg-primary text-white py-2 rounded-lg text-sm font-medium shadow-sm hover:bg-primary-dark transition-colors"
          whileHover={{ scale: 1.01 }}
          whileTap={{ scale: 0.98 }}
          disabled={!searchQuery.trim()}
        >
          Ara
        </motion.button>
      </form>
      
      {/* Arama sonuçları */}
      <div className="px-4 pb-4">
        <SearchResults onClose={onClose2 || onClose} />
      </div>
    </div>
  );
});

// Arama sonuçları bileşeni
interface SearchResultsProps {
  onClose?: () => void;
}

// Memo ile gereksiz render'ları önle
export const SearchResults = memo<SearchResultsProps>(({ onClose }) => {
  const { results, isLoading, searchQuery, saveInstructorToLocalStorage, hasResults, showResults } = useSearch();
  const navigate = useNavigate();

  // Bölüme yönlendirme
  const handleDepartmentClick = useCallback((department: SearchResult) => {
    navigate(`/ogrenci/bolumler/${department.id}`);
    if (onClose) onClose();
  }, [navigate, onClose]);

  // Akademisyene yönlendirme
  const handleInstructorClick = useCallback((instructor: SearchResult) => {
    if (instructor.urlPath) {
      // Akademisyeni localStorage'e kaydet
      saveInstructorToLocalStorage(instructor);
      navigate(`/ogrenci/akademisyen/${instructor.urlPath}`);
      if (onClose) onClose();
    }
  }, [navigate, onClose, saveInstructorToLocalStorage]);

  // Sonuç yok ve yükleme durumu yoksa hiç gösterme
  if (!showResults || (!hasResults && !isLoading)) {
    return null;
  }

  return (
    <motion.div
      initial="hidden"
      animate="visible"
      exit="exit"
      variants={resultsVariants}
      className="bg-white shadow-lg rounded-md overflow-hidden border border-gray-200"
    >
      {isLoading ? (
        <div className="p-4 text-center">
          <div className="inline-block animate-spin rounded-full h-5 w-5 border-t-2 border-b-2 border-primary"></div>
          <p className="mt-2 text-sm text-gray-500">Arama yapılıyor...</p>
        </div>
      ) : (
        <div className="max-h-80 overflow-y-auto">
          {/* Sonuç yoksa bilgi mesajı */}
          {!hasResults && (
            <div className="p-4 text-center">
              <p className="text-sm text-gray-500">"{searchQuery}" için sonuç bulunamadı.</p>
            </div>
          )}

          {/* Bölüm sonuçları */}
          {results.departments.length > 0 && (
            <div>
              <div className="px-4 py-2 bg-gray-50 border-b border-gray-200">
                <div className="flex items-center text-xs font-semibold text-gray-500">
                  <Book size={14} className="mr-1.5" />
                  <span>Bölümler</span>
                </div>
              </div>
              <ul>
                {results.departments.map((dept) => (
                  <li key={`dept-${dept.id}`}>
                    <button
                      onClick={() => handleDepartmentClick(dept)}
                      className="w-full px-4 py-2 text-sm text-left hover:bg-gray-50 flex items-center transition-colors"
                    >
                      <span className="flex-1 truncate">{dept.name}</span>
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}

          {/* Akademisyen sonuçları */}
          {results.instructors.length > 0 && (
            <div>
              <div className="px-4 py-2 bg-gray-50 border-b border-gray-200">
                <div className="flex items-center text-xs font-semibold text-gray-500">
                  <User size={14} className="mr-1.5" />
                  <span>Akademisyenler</span>
                </div>
              </div>
              <ul>
                {results.instructors.map((instructor) => (
                  <li key={`instructor-${instructor.id}`}>
                    <button
                      onClick={() => handleInstructorClick(instructor)}
                      className="w-full px-4 py-2 text-sm text-left hover:bg-gray-50 flex items-center transition-colors"
                    >
                      {instructor.imageUrl && (
                        <div className="w-8 h-8 rounded-full overflow-hidden mr-3 flex-shrink-0">
                          <img 
                            src={instructor.imageUrl} 
                            alt={instructor.name}
                            className="w-full h-full object-cover"
                            loading="lazy"
                          />
                        </div>
                      )}
                      <div className="flex flex-col overflow-hidden">
                        <span className="font-medium truncate">{instructor.name}</span>
                        {instructor.departmentName && (
                          <span className="text-xs text-gray-500 truncate">{instructor.departmentName}</span>
                        )}
                      </div>
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      )}
    </motion.div>
  );
});

// SearchBar bileşenleri için gerekli props
interface SearchModalProps {
  placeholder?: string;
  onSubmit?: (query: string) => void;
  schoolId?: number; // Okul ID'si - arama için gerekli
  isMobile?: boolean;
}

// Ana Modal Bileşeni
const Modal = memo<SearchModalProps>(({ 
  placeholder = "Akademisyen ara...",
  onSubmit,
  schoolId = 1,
  isMobile = false
}) => {
  const { 
    searchQuery, 
    setSearchQuery, 
    clearSearch, 
    isModalOpen, 
    setIsModalOpen,
    handleSearch,
    setShowResults,
    setIsFocused
  } = useSearch();

  // Modal kapatma işlemi
  const handleCloseModal = useCallback(() => {
    setIsModalOpen(false);
    clearSearch();
  }, [setIsModalOpen, clearSearch]);

  // Arama işlemi
  const handleSubmitSearch = useCallback(async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (searchQuery.trim()) {
      await handleSearch(schoolId);
      
      if (onSubmit) {
        onSubmit(searchQuery);
      }
    }
  }, [searchQuery, handleSearch, schoolId, onSubmit]);

  // Input değişikliği işleyicisi
  const handleInputChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setSearchQuery(value);
    
    // En az 2 karakter girildiğinde aramaya başla
    if (value.trim().length >= 2) {
      handleSearch(schoolId);
    } else {
      setShowResults(false);
    }
  }, [setSearchQuery, handleSearch, setShowResults, schoolId]);

  // Dışarı tıklandığında sonuçları gizle
  useEffect(() => {
    if (!isModalOpen) return;
    
    const handleClickOutside = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      if (!target.closest('.search-modal-content')) {
        setShowResults(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [isModalOpen, setShowResults]);

  // Modal açıldığında/kapatıldığında focsu state'i güncelleyelim
  useEffect(() => {
    if (isModalOpen) {
      setIsFocused(true);
    } else {
      setIsFocused(false);
    }
  }, [isModalOpen, setIsFocused]);

  if (!isModalOpen) return null;

  // Modal sınıfı oluştur - mobil görünüm için farklı stiller
  const modalClass = isMobile 
    ? 'fixed inset-x-4 top-16 z-50 overflow-hidden rounded-xl search-modal-content' 
    : 'fixed inset-x-0 top-16 z-50 overflow-hidden rounded-xl search-modal-content mx-auto max-w-2xl';

  return (
    <AnimatePresence>
      <>
        {/* Backdrop / Arka plan overlay */}
        <motion.div
          className="fixed inset-0 bg-black/40 backdrop-blur-sm z-40"
          initial="hidden"
          animate="visible"
          exit="exit"
          variants={backdropVariants}
          onClick={handleCloseModal}
        />
        
        {/* Modal içerik */}
        <motion.div
          className={modalClass}
          initial="hidden"
          animate="visible"
          exit="exit"
          variants={modalVariants}
          onClick={e => e.stopPropagation()}
        >
          <ModalContent
            title="Akademisyen Ara"
            onClose={handleCloseModal}
            placeholder={placeholder}
            searchQuery={searchQuery}
            handleSubmitSearch={handleSubmitSearch}
            handleInputChange={handleInputChange}
            clearSearch={clearSearch}
            onClose2={handleCloseModal}
            isMobile={isMobile}
          />
        </motion.div>
      </>
    </AnimatePresence>
  );
});

export default Modal;

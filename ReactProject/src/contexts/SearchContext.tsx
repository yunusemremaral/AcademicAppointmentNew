import React, { createContext, useContext, useState, ReactNode } from 'react';
import { searchBySchoolId, SearchResult, SearchResults } from '../services/Ogrenci/SearchService';

// Akademisyen için localStorage anahtar sabiti
const INSTRUCTOR_STORAGE_KEY = 'recent_instructors';

interface SearchContextProps {
  searchQuery: string;
  setSearchQuery: React.Dispatch<React.SetStateAction<string>>;
  results: SearchResults;
  isLoading: boolean;
  isFocused: boolean;
  setIsFocused: React.Dispatch<React.SetStateAction<boolean>>;
  isModalOpen: boolean;
  setIsModalOpen: React.Dispatch<React.SetStateAction<boolean>>;
  showResults: boolean;
  setShowResults: React.Dispatch<React.SetStateAction<boolean>>;
  hasResults: boolean;
  clearSearch: () => void;
  handleSearch: (schoolId: number) => Promise<void>;
  saveInstructorToLocalStorage: (instructor: SearchResult) => void;
  getRecentInstructors: () => SearchResult[];
}

const SearchContext = createContext<SearchContextProps | undefined>(undefined);

export const SearchProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  // Arama durumu
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [results, setResults] = useState<SearchResults>({
    departments: [],
    instructors: [],
    query: ''
  });
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isFocused, setIsFocused] = useState<boolean>(false);
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [showResults, setShowResults] = useState<boolean>(false);
  
  // Sonuç var mı kontrolü
  const hasResults = results.departments.length > 0 || results.instructors.length > 0;

  // Aramayı temizle
  const clearSearch = () => {
    setSearchQuery('');
    setResults({
      departments: [],
      instructors: [],
      query: ''
    });
    setShowResults(false);
  };

  // Arama işlemi
  const handleSearch = async (schoolId: number) => {
    try {
      if (searchQuery.trim().length < 2) {
        setResults({
          departments: [],
          instructors: [],
          query: ''
        });
        setShowResults(false);
        return;
      }
      
      setIsLoading(true);
      setShowResults(true);
      
      // SearchService üzerinden arama yap
      const searchResults = await searchBySchoolId(schoolId, searchQuery);
      
      setResults(searchResults);
      setIsLoading(false);
    } catch (error) {
      console.error('Arama sırasında hata:', error);
      setIsLoading(false);
      setResults({
        departments: [],
        instructors: [],
        query: searchQuery
      });
    }
  };
  
  // Akademisyeni localStorage'a kaydet
  const saveInstructorToLocalStorage = (instructor: SearchResult) => {
    try {
      // Mevcut kayıtlı akademisyenleri al
      const storedInstructors = localStorage.getItem(INSTRUCTOR_STORAGE_KEY);
      let instructorArray: SearchResult[] = storedInstructors 
        ? JSON.parse(storedInstructors) 
        : [];
      
      // Aynı akademisyen zaten varsa, onu kaldır
      instructorArray = instructorArray.filter(item => item.id !== instructor.id);
      
      // En başa yeni akademisyeni ekle
      instructorArray.unshift(instructor);
      
      // Maximum 10 akademisyen tutalım
      if (instructorArray.length > 10) {
        instructorArray = instructorArray.slice(0, 10);
      }
      
      // Güncelle
      localStorage.setItem(INSTRUCTOR_STORAGE_KEY, JSON.stringify(instructorArray));
    } catch (error) {
      console.error('Akademisyen localStorage kaydı sırasında hata:', error);
    }
  };
  
  // Son görüntülenen akademisyenleri getir
  const getRecentInstructors = (): SearchResult[] => {
    try {
      const storedInstructors = localStorage.getItem(INSTRUCTOR_STORAGE_KEY);
      return storedInstructors ? JSON.parse(storedInstructors) : [];
    } catch (error) {
      console.error('LocalStorage okuma hatası:', error);
      return [];
    }
  };

  const value: SearchContextProps = {
    searchQuery,
    setSearchQuery,
    results,
    isLoading,
    isFocused,
    setIsFocused,
    isModalOpen,
    setIsModalOpen,
    showResults,
    setShowResults,
    hasResults,
    clearSearch,
    handleSearch,
    saveInstructorToLocalStorage,
    getRecentInstructors
  };

  return (
    <SearchContext.Provider value={value}>
      {children}
    </SearchContext.Provider>
  );
};

export const useSearch = () => {
  const context = useContext(SearchContext);
  
  if (context === undefined) {
    throw new Error('useSearch hook, SearchProvider içinde kullanılmalıdır');
  }
  
  return context;
}; 
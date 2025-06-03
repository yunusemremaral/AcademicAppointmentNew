import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Bolum, getBolumlerFromApi, getBolumlerBySchool } from '../../services/Ogrenci/departmentService';
import { Book, LibraryBig, Search, X } from 'lucide-react';
import { themeColors } from '../../layouts/OgrenciLayout/OgrenciSidebar';
import Loading from '../../components/Loading';
import { useAuth } from '../../contexts/AuthContext';

// Bölüm öğesi bileşeni
const BolumItem: React.FC<{ bolum: Bolum }> = ({ bolum }) => {
  return (
    <Link to={`/ogrenci/bolumler/${bolum.id}`} className="group">
      <div className="flex items-center p-3 rounded-md hover:bg-primary/5 transition-all duration-300 group-hover:translate-x-0.5">
        <Book size={18} className="text-primary/70 mr-3 group-hover:text-primary transition-colors duration-300" />
        <span className="text-sm text-gray-700 group-hover:text-primary transition-colors duration-300">{bolum.name}</span>
      </div>
    </Link>
  );
};

// Ana Bölümler sayfası
const Bolumler: React.FC = () => {
  const { user, isAuthenticated } = useAuth();
  const [bolumler, setBolumler] = useState<Bolum[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [searchInputFocused, setSearchInputFocused] = useState<boolean>(false);
  
  // Bölümleri yükle
  useEffect(() => {
    const loadData = async () => {
      try {
        setIsLoading(true);
        
        // Kullanıcı giriş yapmış ve schoolId varsa, o okula ait bölümleri getir
        if (isAuthenticated && user && user.schoolId) {
          // schoolId string olarak geldiği için number'a çevirmek gerekiyor
          const schoolId = parseInt(user.schoolId, 10);
          
          if (!isNaN(schoolId)) {
            // Öğrencinin okuluna göre bölümleri getir
            const data = await getBolumlerBySchool(schoolId);
            setBolumler(data);
          } else {
            // SchoolId geçerli bir sayı değilse, tüm bölümleri getir
            const data = await getBolumlerFromApi();
            setBolumler(data);
          }
        } else {
          // Kullanıcı giriş yapmamışsa veya schoolId yoksa, tüm bölümleri getir
          const data = await getBolumlerFromApi();
          setBolumler(data);
        }
      } catch (err) {
        setError('Bölüm bilgileri yüklenirken bir hata oluştu.');
        console.error('Veri yükleme hatası:', err);
      } finally {
        setIsLoading(false);
      }
    };
    
    loadData();
  }, [isAuthenticated, user]);
  
  // Arama işlevi
  const filteredBolumler = bolumler.filter(bolum => 
    bolum.name.toLowerCase().includes(searchQuery.toLowerCase())
  );
  
  // Arama çubuğu stilleri
  const searchStyles = {
    container: `flex items-center bg-white border border-gray-200 rounded-md overflow-hidden transition-all duration-200 ease-out ${
      searchInputFocused 
        ? 'ring-1 ring-gray-300' 
        : 'hover:border-gray-300'
    }`,
    icon: `transition-colors duration-200 ease-out ${
      searchInputFocused ? 'text-gray-700' : 'text-gray-400'
    }`
  };
  
  // Arama temizleme
  const clearSearch = () => {
    setSearchQuery('');
  };
  
  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
  };
  
  return (
    <div className="p-3 sm:p-4 md:p-5 max-w-full font-poppins pt-2">
      {/* Sayfa başlığı */}
      <div className={`p-4 ${themeColors.primary.main} rounded-xl shadow-md mb-4 backdrop-blur-sm bg-opacity-95`}>
        <div className="flex items-center">
          <LibraryBig size={18} className="text-white mr-2.5" />
          <h1 className="text-lg font-semibold text-white">
            Bölümler
          </h1>
        </div>
        <p className="text-white/90 text-xs mt-1.5 max-w-2xl">
          {isAuthenticated && user && user.schoolId ? (
            <>Üniversite bölümlerini görüntüleyin ve ilgili akademisyenlere ulaşın.</>
          ) : (
            <>Tüm bölümleri görüntüleyin ve ilgili akademisyenlere ulaşın.</>
          )}
        </p>
      </div>
      
      {/* Arama Alanı */}
      <div className="mb-4 sm:flex sm:items-center sm:justify-between border-b border-gray-100 pb-2.5">
        <div className="sm:w-2/3 md:w-1/2 lg:w-2/5 mb-3 sm:mb-0">
          <form onSubmit={handleSearch} className="relative">
            <div className={searchStyles.container}>
              <div className="flex items-center justify-center h-8 w-8">
                <Search size={14} className={searchStyles.icon} />
              </div>
              <input
                type="text"
                placeholder="Bölüm ara..."
                className="flex-1 h-8 py-1.5 pr-3 outline-none text-xs font-medium text-gray-700 placeholder-gray-400 bg-transparent"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onFocus={() => setSearchInputFocused(true)}
                onBlur={() => setSearchInputFocused(false)}
              />
              {searchQuery && (
                <button 
                  type="button"
                  onClick={clearSearch}
                  className="w-8 h-8 flex items-center justify-center text-gray-400 hover:text-gray-700 transition-colors"
                >
                  <X size={12} />
                </button>
              )}
            </div>
          </form>
          
          {/* Arama sonuç bilgisi */}
          {searchQuery && (
            <div className="mt-1.5 text-[10px] text-gray-500 pl-1">
              <span className="text-primary font-medium">"{searchQuery}"</span> için {filteredBolumler.length} bölüm bulundu.
            </div>
          )}
        </div>
      </div>
      
      {/* Yükleniyor durumu */}
      {isLoading && <Loading />}
      
      {/* Hata durumu */}
      {error && (
        <div className="bg-red-50 text-red-700 p-3 rounded-lg border border-red-200 mb-4 shadow-sm text-sm">
          <p className="flex items-center">
            <span className="mr-2">⚠️</span>
            {error}
          </p>
        </div>
      )}
      
      {/* İçerik - Bölümler Listesi */}
      {!isLoading && !error && (
        <div className="bg-white rounded-xl border border-gray-100 shadow-sm">
          {filteredBolumler.length > 0 ? (
            <div className="divide-y divide-gray-100">
              {filteredBolumler.map((bolum) => (
                <BolumItem key={bolum.id} bolum={bolum} />
              ))}
            </div>
          ) : (
            // Sonuç bulunamadı
            <div className="text-center py-10">
              <Book size={40} className="text-gray-200 mx-auto mb-3" />
              <h3 className="text-base font-medium text-gray-700 mb-1.5">
                {searchQuery 
                  ? 'Aramanızla eşleşen bölüm bulunamadı.' 
                  : 'Henüz hiç bölüm eklenmemiş.'}
              </h3>
              <p className="text-gray-500 text-xs max-w-md mx-auto">
                {searchQuery 
                  ? 'Farklı anahtar kelimelerle aramayı deneyin.' 
                  : 'Daha sonra tekrar kontrol edin.'}
              </p>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default Bolumler;

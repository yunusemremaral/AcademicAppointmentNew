import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Bolum, getBolumlerFromApi, getBolumlerBySchool } from '../../../services/Ogrenci/departmentService';
import { LibraryBig, ArrowRight, Book } from 'lucide-react';
import { themeColors } from '../../../layouts/OgrenciLayout/OgrenciSidebar';
import { useAuth } from '../../../contexts/AuthContext';

// Ana Sayfa için Bölümler Kartı Bileşeni
const AnasayfaBolumler: React.FC = () => {
  const { user, isAuthenticated } = useAuth();
  const [bolumler, setBolumler] = useState<Bolum[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  
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
            // Sadece ilk 3 bölümü al
            setBolumler(data.slice(0, 3));
          } else {
            // SchoolId geçerli bir sayı değilse, tüm bölümleri getir
            const data = await getBolumlerFromApi();
            // Sadece ilk 3 bölümü al
            setBolumler(data.slice(0, 3));
          }
        } else {
          // Kullanıcı giriş yapmamışsa veya schoolId yoksa, tüm bölümleri getir
          const data = await getBolumlerFromApi();
          // Sadece ilk 3 bölümü al
          setBolumler(data.slice(0, 3));
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

  return (
    <div className={`${themeColors.primary.main} rounded-xl overflow-hidden md:col-span-6 backdrop-blur-sm ${themeColors.primary.border} shadow-md h-full`}>
      <div className="p-4 relative h-full">
        <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-15"></div>
        
        <div className="relative h-full flex flex-col">
          {/* Başlık */}
          <div className="flex items-center mb-3">
            <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
              <LibraryBig size={16} className={themeColors.primary.textLight} />
            </div>
            <h3 className={`${themeColors.primary.textLight} font-medium text-base`}>Bölümler</h3>
          </div>
          
          {/* Bölümler Listesi */}
          <div className="grid grid-cols-1 gap-2 flex-grow">
            {isLoading ? (
              // Yükleniyor göstergesi
              <div className={`flex items-center justify-center bg-white/5 rounded-lg p-3 ${themeColors.primary.border} backdrop-blur-sm`}>
                <div className="h-4 w-4 animate-spin rounded-full border-2 border-t-transparent border-white"></div>
                <p className={`${themeColors.primary.textLight}/80 text-xs font-light ml-2`}>Bölümler yükleniyor...</p>
              </div>
            ) : error ? (
              // Hata durumu
              <div className={`bg-white/5 rounded-lg p-3 text-center ${themeColors.primary.border} backdrop-blur-sm`}>
                <p className={`${themeColors.primary.textLight}/80 text-xs font-light`}>{error}</p>
              </div>
            ) : bolumler.length > 0 ? (
              // Bölümler
              bolumler.map((bolum) => (
                <Link 
                  key={bolum.id}
                  to={`/ogrenci/bolumler/${bolum.id}`}
                  className={`bg-white/5 rounded-lg p-3 flex justify-between items-center ${themeColors.primary.border} backdrop-blur-sm hover:bg-white/8 transition-colors duration-200`}
                >
                  <div className="flex items-center">
                    <Book size={14} className={`${themeColors.primary.textLight}/70 mr-2`} />
                    <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>{bolum.name}</p>
                  </div>
                  <ArrowRight size={14} className={`${themeColors.primary.textLight}/60`} />
                </Link>
              ))
            ) : (
              // Bölüm bulunamadı
              <div className={`bg-white/5 rounded-lg p-3 text-center ${themeColors.primary.border} backdrop-blur-sm`}>
                <p className={`${themeColors.primary.textLight}/80 text-xs font-light`}>Henüz hiç bölüm eklenmemiş.</p>
              </div>
            )}
          </div>
          
          {/* Tümünü Görüntüle Butonu */}
          <div className="mt-3">
            <Link to="/ogrenci/bolumler" className={`w-full bg-white/8 hover:bg-white/12 transition-all duration-300 ${themeColors.primary.textLight} text-xs py-2 rounded-lg font-light flex items-center justify-center hover:shadow-sm hover:translate-y-[-1px] backdrop-blur-sm ${themeColors.primary.border}`}>
              Bölümler sayfasını ziyaret edin <ArrowRight size={14} className="ml-1.5 group-hover:ml-2 transition-all" />
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AnasayfaBolumler;

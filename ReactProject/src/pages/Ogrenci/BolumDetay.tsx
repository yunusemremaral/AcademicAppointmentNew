import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Akademisyen, getAkademisyenlerByBolumId, getBolumById } from '../../services/Ogrenci/departmentService';
import { Book, AlertCircle, Users } from 'lucide-react';
import { themeColors } from '../../layouts/OgrenciLayout/OgrenciSidebar';
import Loading from '../../components/Loading';
import AkademisyenKart from '../../components/Ogrenci/AkademisyenKart';

// Ana BolumDetay sayfası
const BolumDetay: React.FC = () => {
  const { bolumId } = useParams<{ bolumId: string }>();
  const [akademisyenler, setAkademisyenler] = useState<Akademisyen[]>([]);
  const [bolumAdi, setBolumAdi] = useState<string>('');
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    const loadData = async () => {
      if (!bolumId) {
        setError('Bölüm ID\'si bulunamadı.');
        setIsLoading(false);
        return;
      }
      
      try {
        setIsLoading(true);
        
        // Bölüm bilgilerini getir
        const bolum = await getBolumById(bolumId);
        if (bolum) {
          setBolumAdi(bolum.name);
          
          // Bölüme ait akademisyenleri getir
          const akademisyenlerData = await getAkademisyenlerByBolumId(bolumId);
          setAkademisyenler(akademisyenlerData);
        } else {
          setError('Bölüm bulunamadı.');
        }
      } catch (err) {
        setError('Bölüm detayları yüklenirken bir hata oluştu.');
        console.error('Veri yükleme hatası:', err);
      } finally {
        setIsLoading(false);
      }
    };
    
    loadData();
  }, [bolumId]);
  
  // Yükleniyor durumu
  if (isLoading) {
    return <Loading />;
  }
  
  // Hata durumu
  if (error) {
    return (
      <div className="p-5 flex justify-center items-center min-h-[300px]">
        <div className="text-center text-red-500">
          <AlertCircle size={32} className="mx-auto mb-2" />
          <p>{error}</p>
        </div>
      </div>
    );
  }
  
  return (
    <div className="p-3 sm:p-4 md:p-5 max-w-full font-poppins pt-2">
      {/* Bölüm başlığı */}
      <div className={`p-4 ${themeColors.primary.main} rounded-xl shadow-md mb-4 backdrop-blur-sm bg-opacity-95`}>
        <div className="flex items-center">
          <Book size={18} className="text-white mr-2.5" />
          <h1 className="text-lg font-semibold text-white">{bolumAdi}</h1>
        </div>
        <p className="text-white/90 text-xs mt-1.5 max-w-2xl">
          Bölüm akademisyenlerini görüntüleyin ve profil detaylarına ulaşın.
        </p>
      </div>
      
      {/* Akademisyenler listesi */}
      {akademisyenler.length > 0 ? (
        <div className="grid grid-cols-3 sm:grid-cols-4 md:grid-cols-5 lg:grid-cols-6 gap-3">
          {akademisyenler.map(akademisyen => (
            <AkademisyenKart key={akademisyen.id} akademisyen={akademisyen} />
          ))}
        </div>
      ) : (
        <div className="p-5 bg-gray-50 rounded-lg border border-gray-100 text-center">
          <Users size={32} className="mx-auto mb-2 text-gray-400" />
          <p className="text-sm text-gray-500">Bu bölüme ait akademisyen bilgisi bulunamadı.</p>
        </div>
      )}
    </div>
  );
};

export default BolumDetay;

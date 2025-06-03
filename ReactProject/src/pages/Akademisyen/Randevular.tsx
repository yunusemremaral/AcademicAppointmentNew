import { useState, useEffect, useMemo, useCallback } from 'react';
import { AlarmClockOff, Clock4, Dock } from 'lucide-react';
import Loading from '../../components/Loading';
import { useAuth, STORAGE_KEYS } from '../../contexts/AuthContext';
import { UiRandevu } from '../../types/randevu';
import { AlertCircle } from 'lucide-react';
import RandevuKart from '../../components/Akademisyen/Randevular/RandevuKart';
import EmptyState from '../../components/Akademisyen/Randevular/RandevuYok';
import { 
  getAcademicAppointments, 
  approveAppointment,
  cancelAppointment,
  completeAppointment,
  getRandevuDurumText,
  Randevu as ApiRandevu 
} from '../../services/Akademisyen/appointmentService';

// API Randevu verisini UI Randevu verisine dönüştürme fonksiyonu
const convertApiToUiRandevu = (apiRandevu: ApiRandevu): UiRandevu => {
  return {
    id: apiRandevu.id,
    akademisyen: {
      id: apiRandevu.studentUserId,
      name: apiRandevu.studentUser || "Öğrenci ismi alınamadı",
      fotograf_url: null,
      unvan: ""
    },
    tarih: apiRandevu.scheduledAt,
    konu: apiRandevu.subject,
    aciklama: apiRandevu.description || null,
    durum: getRandevuDurumText(apiRandevu.status)
  };
};

// Ana Randevular sayfası
const Randevular: React.FC = () => {
  const [randevular, setRandevular] = useState<UiRandevu[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  
  // AuthContext'ten kullanıcı bilgilerini al
  const { isAuthenticated, user } = useAuth();
  
  // Kullanıcı ID'sini alma fonksiyonu - useCallback ile optimize edildi
  const getCurrentUserId = useCallback((): string => {
    // Öncelikle context'ten kontrol et
    if (isAuthenticated && user?.sub) {
      return user.sub;
    }
    
    // Context boşsa localStorage'dan kontrol et
    const userId = localStorage.getItem(STORAGE_KEYS.USER_ID);
    if (userId) {
      return userId;
    }
    
    return "";
  }, [isAuthenticated, user]);
  
  // Randevu verilerini yükleme
  useEffect(() => {
    const fetchRandevular = async () => {
      try {
        setIsLoading(true);
        
        // Kullanıcı ID'sini al
        const academicId = getCurrentUserId();
        
        if (!academicId) {
          setError('Kullanıcı kimliği bulunamadı. Lütfen tekrar giriş yapın.');
          return;
        }
        
        // API'den randevuları getir
        const apiRandevular = await getAcademicAppointments(academicId);
        
        // API verisini UI verisine dönüştür
        const uiRandevular = apiRandevular.map(convertApiToUiRandevu);
        
        // State'i güncelle
        setRandevular(uiRandevular);
      } catch (err) {
        setError('Randevu bilgileri yüklenirken bir hata oluştu.');
        console.error('Veri yükleme hatası:', err);
      } finally {
        setIsLoading(false);
      }
    };
    
    fetchRandevular();
  }, [getCurrentUserId]);
  
  // Randevu işlemleri - useCallback ile optimize edildi
  const handleApproveRandevu = useCallback(async (id: number) => {
    if (window.confirm('Bu randevuyu onaylamak istediğinizden emin misiniz?')) {
      try {
        setIsLoading(true);
        // API isteği gönder
        const response = await approveAppointment(id);
        
        if (response.success) {
          // Yerel state'i güncelle
          setRandevular(prevRandevular => 
            prevRandevular.map(randevu => 
              randevu.id === id ? { ...randevu, durum: 'onaylandı' } : randevu
            )
          );
        } else {
          alert(`Randevu onaylanamadı: ${response.message || 'Bir hata oluştu'}`);
        }
      } catch (error) {
        console.error('Randevu onaylama hatası:', error);
        alert('Randevu onaylanırken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      } finally {
        setIsLoading(false);
      }
    }
  }, []);
  
  const handleRejectRandevu = useCallback(async (id: number) => {
    if (window.confirm('Bu randevuyu iptal etmek istediğinizden emin misiniz?')) {
      try {
        setIsLoading(true);
        // API isteği gönder
        const response = await cancelAppointment(id);
        
        if (response.success) {
          // Yerel state'i güncelle
          setRandevular(prevRandevular => 
            prevRandevular.map(randevu => 
              randevu.id === id ? { ...randevu, durum: 'iptal edildi' } : randevu
            )
          );
        } else {
          alert(`Randevu iptal edilemedi: ${response.message || 'Bir hata oluştu'}`);
        }
      } catch (error) {
        console.error('Randevu iptal etme hatası:', error);
        alert('Randevu iptal edilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      } finally {
        setIsLoading(false);
      }
    }
  }, []);
  
  const handleCompleteRandevu = useCallback(async (id: number) => {
    if (window.confirm('Bu randevuyu tamamlandı olarak işaretlemek istediğinizden emin misiniz?')) {
      try {
        setIsLoading(true);
        // API isteği gönder
        const response = await completeAppointment(id);
        
        if (response.success) {
          // Yerel state'i güncelle
          setRandevular(prevRandevular => 
            prevRandevular.map(randevu => 
              randevu.id === id ? { ...randevu, durum: 'tamamlandı' } : randevu
            )
          );
        } else {
          alert(`Randevu tamamlanamadı: ${response.message || 'Bir hata oluştu'}`);
        }
      } catch (error) {
        console.error('Randevu tamamlama hatası:', error);
        alert('Randevu tamamlanırken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      } finally {
        setIsLoading(false);
      }
    }
  }, []);
  
  const handleNoShowRandevu = useCallback(async (id: number) => {
    if (window.confirm('Öğrencinin randevuya katılmadığını onaylıyor musunuz?')) {
      try {
        setIsLoading(true);
        // API isteği gönder - öğrenci katılmadı durumu için iptal kodu kullanıyoruz
        const response = await cancelAppointment(id);
        
        if (response.success) {
          // Yerel state'i güncelle
          setRandevular(prevRandevular => 
            prevRandevular.map(randevu => 
              randevu.id === id ? { ...randevu, durum: 'iptal edildi' } : randevu
            )
          );
        } else {
          alert(`İşlem gerçekleştirilemedi: ${response.message || 'Bir hata oluştu'}`);
        }
      } catch (error) {
        console.error('Randevu katılmama durumu hatası:', error);
        alert('İşlem sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      } finally {
        setIsLoading(false);
      }
    }
  }, []);
  
  // Gelecek ve geçmiş randevular filtreleme - useMemo ile optimize edildi
  const { gelecekRandevular, gecmisRandevular } = useMemo(() => {
    const simdi = new Date();
    
    return {
      gelecekRandevular: randevular.filter(randevu => 
        new Date(randevu.tarih) > simdi && randevu.durum !== 'iptal edildi'
      ),
      gecmisRandevular: randevular.filter(randevu => 
        new Date(randevu.tarih) < simdi || randevu.durum === 'iptal edildi'
      )
    };
  }, [randevular]);
  
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
    <div className="p-2 sm:p-3 md:p-5 max-w-full font-poppins">
      {/* Sayfa başlığı */}
      <div className={`p-3 sm:p-4 bg-primary rounded-xl shadow-md mb-4 sm:mb-5 backdrop-blur-sm bg-opacity-95`}>
        <div className="flex items-center">
          <Dock size={16} className="text-white mr-2.5" />
          <h1 className="text-base sm:text-lg font-semibold text-white">Randevular</h1>
        </div>
        <p className="text-white/90 text-xs mt-1.5 max-w-2xl">
          Öğrencilerinizin randevu taleplerini görüntüleyin ve yönetin.
        </p>
      </div>
      
      {/* Randevu listesi boşsa */}
      {randevular.length === 0 && <EmptyState />}
      
      {/* Randevu listeleri */}
      {randevular.length > 0 && (
        <div className="space-y-5 sm:space-y-6">
          {/* Gelecek randevular */}
          {gelecekRandevular.length > 0 && (
            <div>
              <h2 className="text-sm font-medium text-gray-700 mb-4 flex items-center border-b border-gray-200 pb-2.5">
                <Clock4 size={15} className="mr-2 text-primary" />
                Yaklaşan Randevular
              </h2>
              <div className="space-y-3 mt-4">
                {gelecekRandevular.map(randevu => (
                  <RandevuKart
                    key={randevu.id}
                    randevu={randevu}
                    onApprove={handleApproveRandevu}
                    onReject={handleRejectRandevu}
                  />
                ))}
              </div>
            </div>
          )}
          
          {/* Geçmiş randevular */}
          {gecmisRandevular.length > 0 && (
            <div className="mt-8">
              <h2 className="text-sm font-medium text-gray-700 mb-4 flex items-center border-b border-gray-200 pb-2.5">
                <AlarmClockOff size={15} className="mr-2 text-primary" />
                Geçmiş Randevular
              </h2>
              <div className="space-y-3 opacity-90">
                {gecmisRandevular.map(randevu => (
                  <RandevuKart
                    key={randevu.id}
                    randevu={randevu}
                    onComplete={handleCompleteRandevu}
                    onNoShow={handleNoShowRandevu}
                  />
                ))}
              </div>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default Randevular;

import React, { useState, useEffect, useRef, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { 
  addDays, 
  startOfWeek, 
  addWeeks,
  isSameDay,
  isWeekend,
  isMonday,
  isTuesday,
  isWednesday,
  isThursday,
  isFriday
} from 'date-fns';
import type { Akademisyen } from '../../services/Ogrenci/departmentService';
import { getAkademisyenById } from '../../services/Ogrenci/departmentService';
import { createAppointment } from '../../services/Ogrenci/appointmentService';
import { User, AlertCircle } from 'lucide-react';
import { themeColors } from '../../layouts/OgrenciLayout/OgrenciSidebar';
import Loading from '../../components/Loading';
import { useAuth, STORAGE_KEYS } from '../../contexts/AuthContext';
import RandevuModal from '../../components/Ogrenci/RandevuModal';

// AkademisyenBilesen bileşenlerini import et
import {
  DayButton,
  AppointmentForm,
  WeekSelector,
  TimeSlotsGrid,
  createTimeSlots,
  TimeSlot
} from '../../components/Ogrenci/AkademisyenBilesen';

// localStorage anahtar sabiti
const LS_AKADEMISYEN_KEY = 'currentAkademisyen';

/**
 * Giriş yapmış kullanıcının ID'sini almak için fonksiyon
 * @param user Kullanıcı objesi
 * @param isAuthenticated Kimlik doğrulama durumu
 * @returns Kullanıcı ID'si veya boş string
 */
const getCurrentUserId = (user: any, isAuthenticated: boolean): string => {
  // Öncelikle context'ten kontrol et
  if (isAuthenticated && user?.sub) {
    return user.sub; // JWT token'ından alınan kullanıcı ID'si
  }
  
  // Context boşsa localStorage'dan kontrol et
  const userId = localStorage.getItem(STORAGE_KEYS.USER_ID);
  if (userId) {
    return userId;
  }
  
  // Kullanıcı ID'si bulunamadıysa hata fırlat
  console.error('Kullanıcı oturumu bulunamadı! Lütfen tekrar giriş yapın.');
  return "";
};

/**
 * Hafta içi günleri kontrol eden yardımcı fonksiyon
 * @param date Kontrol edilecek tarih
 * @returns Hafta içi mi durumu
 */
const isWeekday = (date: Date): boolean => {
  return isMonday(date) || isTuesday(date) || isWednesday(date) || isThursday(date) || isFriday(date);
};

/**
 * Ana Akademisyen sayfası
 */
const Akademisyen: React.FC = () => {
  const { urlPath } = useParams<{ urlPath: string }>();
  const [akademisyen, setAkademisyen] = useState<Akademisyen | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  
  // Auth context'ten kullanıcı bilgilerini al
  const { isAuthenticated, user } = useAuth();
  
  // Randevu durumu
  const [availableDays, setAvailableDays] = useState<Date[]>([]);
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  const [timeSlots, setTimeSlots] = useState<TimeSlot[]>([]);
  const [selectedTimeSlot, setSelectedTimeSlot] = useState<Date | null>(null);
  const [currentWeek, setCurrentWeek] = useState<'current' | 'next'>('current');
  
  // Randevu formu
  const [konu, setKonu] = useState<string>('');
  const [aciklama, setAciklama] = useState<string>('');
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [submitSuccess, setSubmitSuccess] = useState<boolean>(false);
  const [submitError, setSubmitError] = useState<string | null>(null);
  
  // Modal durumu
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [randevuBilgi, setRandevuBilgi] = useState<{
    tarih: Date;
    konu: string;
    aciklama?: string;
  } | null>(null);
  
  // Form alanına scroll için ref
  const formRef = useRef<HTMLDivElement>(null);
  
  /**
   * Akademisyen verilerini localStorage'dan yükleme
   */
  const loadStoredAkademisyen = useCallback(() => {
    try {
      const storedData = localStorage.getItem(LS_AKADEMISYEN_KEY);
      if (!storedData) return false;
      
      const parsedData = JSON.parse(storedData) as Akademisyen;
      
      // URL parametresi ile localStorage'daki veriyi karşılaştır
      if (parsedData.urlPath === urlPath || parsedData.id === urlPath) {
        console.log("Akademisyen verisi localStorage'dan yüklendi:", parsedData);
        setAkademisyen(parsedData);
        return true; // Başarıyla yüklendi
      }
      
      return false; // Veri eşleşmedi
    } catch (err) {
      console.error('localStorage verisi okunurken hata:', err);
      return false;
    }
  }, [urlPath]);
  
  /**
   * Haftayı değiştirme işlemi
   */
  const toggleWeek = useCallback((week: 'current' | 'next') => {
    // Hafta değiştiğinde seçili tarihi sıfırla
    setSelectedDate(null);
    setSelectedTimeSlot(null);
    setCurrentWeek(week);
  }, []);
  
  /**
   * Zaman dilimi seçme işlemi
   */
  const handleTimeSlotSelection = useCallback((time: Date) => {
    setSelectedTimeSlot(time);
  }, []);
  
  /**
   * Randevu oluşturma işlemi
   */
  const handleSubmit = useCallback(async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!selectedTimeSlot || !akademisyen) {
      setSubmitError('Lütfen bir randevu zamanı seçin.');
      return;
    }
    
    // Randevu konusu doğrulaması
    if (!konu.trim()) {
      setSubmitError('Lütfen randevu konusunu belirtin.');
      return;
    } else if (konu.trim().length < 3) {
      setSubmitError('Randevu konusu en az 3 karakter olmalıdır.');
      return;
    }
    
    try {
      setIsSubmitting(true);
      setSubmitError(null);
      
      // Modal için randevu bilgilerini hazırla
      setRandevuBilgi({
        tarih: new Date(selectedTimeSlot),
        konu: konu.trim(),
        aciklama: aciklama.trim() || " "
      });
      
      // Modalı göster
      setIsModalOpen(true);
      
      // Öğrenci ID'sini auth context ve localStorage'dan al
      const studentUserId = getCurrentUserId(user, isAuthenticated);
      
      // StudentUserId boşsa işlemi durdur
      if (!studentUserId) {
        setSubmitError('Kullanıcı ID bilgisi alınamadı. Lütfen tekrar giriş yapın.');
        console.error('Kullanıcı ID bilgisi boş!');
        return;
      }
      
      // Akademisyen ID kontrolü
      if (!akademisyen.id) {
        setSubmitError('Akademisyen bilgileri alınamadı.');
        console.error('Akademisyen ID bilgisi boş!');
        return;
      }
      
      // API çağrısını gerçekleştir
      const response = await createAppointment(
        akademisyen.id,
        studentUserId,
        selectedTimeSlot, // Date objesi olarak gönder
        konu.trim(),
        aciklama.trim() || " "
      );
      
      // API yanıtını kontrol et
      if (response.success) {
        // Başarılı olduğunda
        setSubmitSuccess(true);
        setKonu('');
        setAciklama('');
        setSelectedTimeSlot(null);
      } else {
        // API başarısız yanıt döndürdüyse
        setSubmitError(response.message || 'Randevu oluşturulurken bir hata oluştu.');
        console.error('Randevu oluşturma yanıtı başarısız:', response);
        // Hata durumunda modalı kapat
        setIsModalOpen(false);
      }
      
    } catch (error) {
      setSubmitError('Randevu oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.');
      console.error('Randevu oluşturma hatası:', error);
      // Hata durumunda modalı kapat
      setIsModalOpen(false);
    } finally {
      setIsSubmitting(false);
    }
  }, [akademisyen, selectedTimeSlot, konu, aciklama, user, isAuthenticated]);
  
  // Modalı kapatma fonksiyonu
  const handleCloseModal = useCallback(() => {
    setIsModalOpen(false);
    // Modalı kapattıktan bir süre sonra form alanını temizle ve başarı durumunu sıfırla
    setTimeout(() => {
      if (submitSuccess) {
        setSubmitSuccess(false);
      }
    }, 300);
  }, [submitSuccess]);
  
  // Seçilen zaman dilimi değiştiğinde form alanına otomatik kaydır
  useEffect(() => {
    if (selectedTimeSlot && formRef.current) {
      // Form render edildikten sonra kaydırma işlemini gerçekleştir
      const timer = setTimeout(() => {
        formRef.current?.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }, 100);
      
      return () => clearTimeout(timer);
    }
  }, [selectedTimeSlot]);
  
  // Akademisyen verilerini yükle
  useEffect(() => {
    const loadData = async () => {
      if (!urlPath) {
        setError('Akademisyen bilgisi bulunamadı.');
        setIsLoading(false);
        return;
      }
      
      try {
        setIsLoading(true);
        setError(null); // Önceki hataları temizle
        
        // Önce localStorage'daki veriyi kontrol et
        const isStoredDataAvailable = loadStoredAkademisyen();
        
        // Eğer localStorage'da veri yoksa API'den getir
        if (!isStoredDataAvailable) {
          console.log(`API'den akademisyen verisi getiriliyor: ${urlPath}`);
          
          // API'den akademisyen bilgilerini getir
          const akademisyenData = await getAkademisyenById(urlPath);
          
          if (akademisyenData) {
            console.log('API\'den akademisyen bilgileri başarıyla alındı:', akademisyenData);
            setAkademisyen(akademisyenData);
            
            // Yeni verileri localStorage'a kaydet
            localStorage.setItem(LS_AKADEMISYEN_KEY, JSON.stringify(akademisyenData));
          } else {
            console.error(`API'den akademisyen bilgileri alınamadı: ${urlPath}`);
            setError(`"${urlPath}" için akademisyen bilgileri bulunamadı.`);
            
            // Hata durumunda SearchContext'ten veri almayı deneyelim (eğer arama sonuçlarından geldiyse)
            try {
              const searchContextData = localStorage.getItem('recent_instructors');
              if (searchContextData) {
                const searchResults = JSON.parse(searchContextData);
                // SearchResult dizisinden eşleşen akademisyeni bul
                const matchedInstructor = searchResults.find((item: any) => 
                  (item.urlPath === urlPath) || (item.id === urlPath)
                );
                
                if (matchedInstructor) {
                  console.log('Akademisyen verisi SearchContext localStorage\'dan kurtarıldı:', matchedInstructor);
                  // SearchContext verisini Akademisyen formatına dönüştür
                  const akademisyen = {
                    id: matchedInstructor.id.toString(),
                    fullName: matchedInstructor.name,
                    bolum_id: matchedInstructor.departmentId || "0",
                    profileImage: matchedInstructor.imageUrl || null,
                    email: matchedInstructor.email || null,
                    urlPath: matchedInstructor.urlPath || null
                  };
                  
                  setAkademisyen(akademisyen);
                  
                  // Kurtarılan veriyi currentAkademisyen olarak kaydet
                  localStorage.setItem(LS_AKADEMISYEN_KEY, JSON.stringify(akademisyen));
                  
                  // Hata durumunu temizle
                  setError(null);
                }
              }
            } catch (recoveryError) {
              console.error('SearchContext verileri kurtarılırken hata:', recoveryError);
            }
          }
        }
      } catch (err) {
        console.error('Akademisyen bilgisi yüklenirken hata:', err);
        setError('Akademisyen bilgileri yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
        
        // Kritik hata durumunda da SearchContext'ten kurtarma deneyelim
        try {
          const searchContextData = localStorage.getItem('recent_instructors');
          if (searchContextData) {
            const searchResults = JSON.parse(searchContextData);
            const matchedInstructor = searchResults.find((item: any) => 
              (item.urlPath === urlPath) || (item.id === urlPath)
            );
            
            if (matchedInstructor) {
              console.log('Kritik hata sonrası akademisyen verisi kurtarıldı:', matchedInstructor);
              // SearchContext verisini Akademisyen formatına dönüştür
              const akademisyen = {
                id: matchedInstructor.id.toString(),
                fullName: matchedInstructor.name,
                bolum_id: matchedInstructor.departmentId || "0",
                profileImage: matchedInstructor.imageUrl || null,
                email: matchedInstructor.email || null,
                urlPath: matchedInstructor.urlPath || null
              };
              
              setAkademisyen(akademisyen);
              localStorage.setItem(LS_AKADEMISYEN_KEY, JSON.stringify(akademisyen));
              setError(null);
            }
          }
        } catch (recoveryError) {
          console.error('Kritik hata sonrası kurtarma işlemi başarısız:', recoveryError);
        }
      } finally {
        setIsLoading(false);
      }
    };
    
    loadData();
  }, [urlPath, loadStoredAkademisyen]);
  
  // Uygun günleri belirle (bu hafta ve sonraki hafta, sadece hafta içi)
  useEffect(() => {
    // Bugünün tarihini al
    const today = new Date();
    
    // Bugünün haftasonuna denk gelip gelmediğini kontrol et
    const isTodayWeekend = isWeekend(today);
    
    // Bu haftanın pazartesi günü
    const thisWeekStart = startOfWeek(today, { weekStartsOn: 1 }); // Pazartesi başlangıç
    
    // Eğer bugün haftasonuysa (cumartesi veya pazar), "Bu Hafta" olarak bir sonraki haftayı göster
    const adjustedThisWeekStart = isTodayWeekend 
      ? addWeeks(thisWeekStart, 1) // Haftasonu ise bir sonraki haftayı "Bu Hafta" olarak kullan
      : thisWeekStart; // Hafta içiyse normal bu haftayı kullan
    
    // Sonraki haftanın pazartesi günü (adjustedThisWeekStart'a göre)
    const nextWeekStart = addWeeks(adjustedThisWeekStart, 1);
    
    // Hangi haftanın gösterileceğini belirle
    const startDate = currentWeek === 'current' ? adjustedThisWeekStart : nextWeekStart;
    
    // Hafta içi günleri hesapla
    const days: Date[] = [];
    for (let day = 0; day < 7; day++) {
      const date = addDays(startDate, day);
      
      // Sadece hafta içi günleri ekle
      if (!isWeekend(date)) {
        days.push(date);
      }
    }
    
    setAvailableDays(days);
    
    // Uygun günleri seç (bugünden sonraki günler)
    const validDays = days.filter(day => {
      // Bugün ile aynı günde ise ve şu an saati çok geçse bile göster
      // Diğer türlü, bugünden sonraki tarihleri göster
      const isToday = isSameDay(day, today);
      const isFutureDay = day > today;
      
      return isToday || isFutureDay;
    });
    
    // Eğer geçerli günler varsa, ilk geçerli günü seç
    if (validDays.length > 0) {
      setSelectedDate(validDays[0]);
    } else if (days.length > 0) {
      // Yoksa normal şekilde ilk günü seç
      setSelectedDate(days[0]);
    }
  }, [currentWeek]);
  
  // Belirli bir günün etkin olup olmadığını kontrol et
  const isDayEnabled = useCallback((date: Date): boolean => {
    // Bugünün başlangıcını al (saat 00:00:00)
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    // Verilen tarihin başlangıcını al
    const checkDate = new Date(date);
    checkDate.setHours(0, 0, 0, 0);
    
    // Etkin olma koşulları:
    // 1. Hafta içi olmalı
    const isWeekdayCheck = isWeekday(date);
    
    // 2. Bugün veya sonrası olmalı
    const isTodayOrFuture = checkDate >= today;
    
    // 3. Randevu alınabilecek maksimum 4 hafta ileri tarih kontrolü (opsiyonel)
    const maxDate = new Date(today);
    maxDate.setDate(today.getDate() + 28); // 4 hafta ileri
    const isWithinRange = checkDate <= maxDate;
    
    return isWeekdayCheck && isTodayOrFuture && isWithinRange;
  }, []);
  
  // Seçilen tarihe ait randevu saatlerini güncelle
  useEffect(() => {
    if (selectedDate) {
      try {
        // Yeni tarih seçildiğinde zaman dilimlerini oluştur
        const slots = createTimeSlots(selectedDate);
        
        // Bugün seçildiyse, özel işlemler yapabiliriz
        const currentTime = new Date();
        const isToday = isSameDay(selectedDate, currentTime);
        
        // Sadece kullanılabilir saatleri filtrele
        const filteredSlots = slots.filter(slot => slot.isAvailable);
        
        // Filtrelenmiş zaman dilimlerini ayarla
        setTimeSlots(filteredSlots);
        
        // Eğer hiç uygun saat kalmadıysa bilgi mesajı göster
        if (filteredSlots.length === 0) {
          if (isToday) {
            setSubmitError('Bugün için randevu oluşturulabilecek uygun saat kalmamıştır. Lütfen başka bir gün seçin.');
          } else {
            setSubmitError('Bu gün için randevu oluşturulabilecek zaman dilimi bulunmamaktadır.');
          }
        } else {
          setSubmitError(null);
        }
        
        // Seçilen zaman dilimini sıfırla
        setSelectedTimeSlot(null);
      } catch (error) {
        console.error('Randevu saatleri oluşturulurken hata:', error);
        setSubmitError('Randevu saatleri yüklenirken bir hata oluştu. Lütfen sayfayı yenileyin.');
      }
    }
  }, [selectedDate]);
  
  // Akademisyen sayfası yüklendiğinde, hafta sonu ise otomatik olarak sonraki haftaya geç
  useEffect(() => {
    // Eğer bugün hafta sonu ise (cumartesi veya pazar), sonraki haftayı göster
    const currentDate = new Date();
    const isCurrentlyWeekend = isWeekend(currentDate);
    
    if (isCurrentlyWeekend) {
      setCurrentWeek('current'); // "Bu hafta" olarak görünecek ama aslında bir sonraki haftayı gösterir
    }
  }, []);
  
  // Yükleniyor durumu
  if (isLoading) {
    return <Loading />;
  }
  
  // Hata durumu
  if (error || !akademisyen) {
    return (
      <div className="p-5 flex justify-center items-center min-h-[300px]">
        <div className="text-center text-red-500">
          <AlertCircle size={32} className="mx-auto mb-2" />
          <p>{error || 'Akademisyen bilgisi bulunamadı.'}</p>
        </div>
      </div>
    );
  }
  
  return (
    <div className="p-3 sm:p-4 md:p-5 max-w-full font-poppins">
      {/* Akademisyen bilgileri başlığı */}
      <div className={`p-4 ${themeColors.primary.main} rounded-xl shadow-md mb-4 backdrop-blur-sm bg-opacity-95`}>
        <div className="flex items-center">
          <User size={18} className="text-white mr-2.5" />
          <h1 className="text-lg font-semibold text-white">{akademisyen.fullName}</h1>
        </div>
        <p className="text-white/90 text-xs mt-1.5 max-w-2xl">
          Akademisyen ile randevu oluşturmak için uygun bir gün ve saat seçin.
        </p>
      </div>
      
      {/* Randevu bölümü */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
        {/* Hafta seçimi başlık çubuğu */}
        <WeekSelector
          currentWeek={currentWeek}
          toggleWeek={toggleWeek}
          thisWeekStart={availableDays.length > 0 ? availableDays[0] : new Date()}
          nextWeekStart={currentWeek === 'current' && availableDays.length > 0 ? addWeeks(availableDays[0], 1) : availableDays.length > 0 ? availableDays[0] : new Date()}
        />
        
        {/* Tarih seçimi */}
        <div className="p-4 border-b border-gray-100 bg-gray-50">
          <p className="text-xs text-gray-500 mb-3">Randevu için bir gün seçin:</p>
          
          <div className="grid grid-cols-5 gap-2">
            {availableDays.map((day, index) => (
              <DayButton
                key={index}
                date={day}
                isSelected={selectedDate ? isSameDay(day, selectedDate) : false}
                onClick={() => setSelectedDate(day)}
                isDisabled={!isDayEnabled(day)}
              />
            ))}
          </div>
        </div>
        
        {/* Seçilen gün ve saatler */}
        {selectedDate && (
          <TimeSlotsGrid
            selectedDate={selectedDate}
            timeSlots={timeSlots}
            selectedTimeSlot={selectedTimeSlot}
            onTimeSlotSelect={handleTimeSlotSelection}
          />
        )}
        
        {/* Randevu oluşturma formu */}
        {selectedTimeSlot && (
          <AppointmentForm
            konu={konu}
            setKonu={setKonu}
            aciklama={aciklama}
            setAciklama={setAciklama}
            selectedTimeSlot={selectedTimeSlot}
            submitError={submitError}
            setSubmitError={setSubmitError}
            isSubmitting={isSubmitting}
            submitSuccess={submitSuccess}
            handleSubmit={handleSubmit}
            formRef={formRef}
          />
        )}
      </div>
      
      {/* Randevu Modal */}
      <RandevuModal
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        academisyenName={akademisyen?.fullName || ''}
        randevuBilgi={randevuBilgi}
        loading={isSubmitting}
        success={submitSuccess}
      />
    </div>
  );
};

export default Akademisyen; 
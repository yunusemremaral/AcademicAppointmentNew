import React, { useState, useEffect } from 'react';
import { format } from 'date-fns';
import { tr } from 'date-fns/locale';
import { Dock, Clock, Check, AlertCircle, X, ChevronRight, Calendar } from 'lucide-react';
import { themeColors } from '../../../layouts/OgrenciLayout/OgrenciSidebar';
import { useAuth, STORAGE_KEYS } from '../../../contexts/AuthContext';
import { 
  getStudentAppointments, 
  getRandevuDurumText, 
  Randevu as ApiRandevu
} from '../../../services/Ogrenci/appointmentService';
import { Link } from 'react-router-dom';

// Akademisyen formatla yardımcı fonksiyonu
const formatAkademisyenIsmi = (fullName: string) => {
  if (!fullName) return { unvan: "", isim: "İsim Bilgisi Yok" };
  
  // İsimleri parçalara ayıralım ve unvan/isim şeklinde formatlayalım
  const parts = fullName.split(" ");
  
  // Unvan kısmını belirleyelim (Dr. Öğr. Üyesi, Prof. Dr. vb.)
  let unvan = "";
  let isim = "";
  
  if (fullName.includes("Prof. Dr.")) {
    unvan = "Prof. Dr.";
    isim = parts.slice(2).join(" ");
  } else if (fullName.includes("Doç. Dr.")) {
    unvan = "Doç. Dr.";
    isim = parts.slice(2).join(" ");
  } else if (fullName.includes("Dr. Öğr. Üyesi")) {
    unvan = "Dr. Öğr. Üyesi";
    isim = parts.slice(3).join(" ");
  } else if (fullName.includes("Arş. Görevlisi")) {
    unvan = "Arş. Gör.";
    isim = parts.slice(2).join(" ");
  } else {
    // Varsayılan format
    unvan = parts[0];
    isim = parts.slice(1).join(" ");
  }
  
  return { unvan, isim };
};

// İsim baş harflerini alma fonksiyonu
const getInitials = (fullName: string) => {
  if (!fullName) return "??";
  
  const { isim } = formatAkademisyenIsmi(fullName);
  
  return isim
    .split(' ')
    .map(name => name.charAt(0))
    .join('')
    .toUpperCase()
    .substring(0, 2);
};

// UI için kullanılan Randevu arayüzü
interface UiRandevu {
  id: number;
  akademisyen: {
    id: string;
    name: string;
    fotograf_url: string | null;
    unvan: string;
  };
  tarih: string; // ISO formatında tarih string
  konu: string;
  aciklama: string | null;
  durum: 'onaylandı' | 'beklemede' | 'iptal edildi' | 'tamamlandı';
}

// API Randevu verisini UI Randevu verisine dönüştürme
const convertApiToUiRandevu = (apiRandevu: ApiRandevu): UiRandevu => {
  const { unvan } = formatAkademisyenIsmi(apiRandevu.academicUser || "");
  
  return {
    id: apiRandevu.id,
    akademisyen: {
      id: apiRandevu.academicUserId,
      name: apiRandevu.academicUser || "İsim bilgisi alınamadı",
      fotograf_url: null,
      unvan: unvan
    },
    tarih: apiRandevu.scheduledAt,
    konu: apiRandevu.subject,
    aciklama: apiRandevu.description || null,
    durum: getRandevuDurumText(apiRandevu.status)
  };
};

// Randevu durumuna göre renk ve simge belirleme
const getRandevuStatusInfo = (durum: UiRandevu['durum']) => {
  switch (durum) {
    case 'onaylandı':
      return {
        label: 'Onaylandı',
        bgColor: 'bg-green-50',
        textColor: 'text-green-700',
        borderColor: 'border-green-200',
        icon: <Check size={14} className="mr-1" />
      };
    case 'beklemede':
      return {
        label: 'Beklemede',
        bgColor: 'bg-blue-50',
        textColor: 'text-blue-700',
        borderColor: 'border-blue-200',
        icon: <Clock size={14} className="mr-1" />
      };
    case 'iptal edildi':
      return {
        label: 'İptal Edildi',
        bgColor: 'bg-red-50',
        textColor: 'text-red-700',
        borderColor: 'border-red-200',
        icon: <X size={14} className="mr-1" />
      };
    case 'tamamlandı':
      return {
        label: 'Tamamlandı',
        bgColor: 'bg-gray-100',
        textColor: 'text-gray-700',
        borderColor: 'border-gray-200',
        icon: <Check size={14} className="mr-1" />
      };
    default:
      return {
        label: 'Bilinmiyor',
        bgColor: 'bg-gray-100',
        textColor: 'text-gray-700',
        borderColor: 'border-gray-200',
        icon: <AlertCircle size={14} className="mr-1" />
      };
  }
};

// Yaklaşan Randevu Satırı Bileşeni
const RandevuSatiri: React.FC<{ randevu: UiRandevu }> = ({ randevu }) => {
  const { unvan, isim } = formatAkademisyenIsmi(randevu.akademisyen.name);
  const initials = getInitials(randevu.akademisyen.name);
  const tarihDate = new Date(randevu.tarih);
  const { label, bgColor, textColor, icon } = getRandevuStatusInfo(randevu.durum);
  
  // Bugünden kaç gün kaldığını hesapla
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  const randevuDate = new Date(tarihDate);
  randevuDate.setHours(0, 0, 0, 0);
  
  const gunFarki = Math.round((randevuDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
  const gunDurumu = gunFarki === 0 
    ? 'Bugün' 
    : gunFarki === 1 
      ? 'Yarın' 
      : `${gunFarki} gün`;
  
  return (
    <div className="flex items-center bg-white/10 backdrop-blur-sm rounded-xl p-3 hover:bg-white/15 transition-all duration-300 hover:translate-x-1 group shadow-sm border border-white/5">
      {/* İnisiyaller kısmı */}
      <div className="w-8 h-8 rounded-full bg-white/20 flex items-center justify-center text-white text-xs font-medium mr-3 flex-shrink-0 shadow-inner group-hover:bg-white/30 transition-all duration-300">
        {initials}
      </div>
      
      {/* Orta bilgi kısmı */}
      <div className="flex-grow min-w-0">
        <div className="flex items-baseline justify-between">
          <p className="text-white/90 text-xs font-medium truncate">{unvan} {isim}</p>
          <span className={`${textColor} ${bgColor} text-[10px] font-medium rounded-full px-2 py-0.5 ml-1 flex items-center flex-shrink-0 shadow-sm`}>
            {icon}
            {label}
          </span>
        </div>
        
        <div className="flex items-center justify-between mt-1">
          <span className="text-[11px] text-white/75 flex items-center">
            <Clock size={12} className="mr-1.5" />
            {format(tarihDate, 'd MMM, HH:mm', { locale: tr })}
          </span>
          <span className="text-[11px] text-white/75 bg-white/5 px-2 py-0.5 rounded-full">{gunDurumu}</span>
        </div>
      </div>
    </div>
  );
};

// Ana AnasayfaRandevu bileşeni
const AnasayfaRandevu: React.FC = () => {
  const [randevular, setRandevular] = useState<UiRandevu[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  
  // AuthContext'ten kullanıcı bilgilerini al
  const { isAuthenticated, user } = useAuth();
  
  // Kullanıcı ID'sini alma fonksiyonu
  const getCurrentUserId = (): string => {
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
  };
  
  // Randevu verilerini yükle
  useEffect(() => {
    const fetchRandevular = async () => {
      try {
        setIsLoading(true);
        
        // Kullanıcı ID'sini al
        const studentId = getCurrentUserId();
        
        if (!studentId) {
          setError('Kullanıcı kimliği bulunamadı.');
          return;
        }
        
        // API'den randevuları getir
        const apiRandevular = await getStudentAppointments(studentId);
        
        // API verisini UI verisine dönüştür
        const uiRandevular = apiRandevular.map(convertApiToUiRandevu);
        
        // State'i güncelle
        setRandevular(uiRandevular);
      } catch (err) {
        setError('Randevu bilgileri yüklenemedi.');
        console.error('Veri yükleme hatası:', err);
      } finally {
        setIsLoading(false);
      }
    };
    
    fetchRandevular();
  }, [isAuthenticated, user]);
  
  // Tüm yaklaşan randevuları filtrele
  const tumYaklasanRandevular = randevular
    .filter(randevu => 
      new Date(randevu.tarih) > new Date() && 
      (randevu.durum === 'onaylandı' || randevu.durum === 'beklemede'));
  
  // En yakın 3 randevuyu al
  const enYakinRandevular = [...tumYaklasanRandevular]
    .sort((a, b) => new Date(a.tarih).getTime() - new Date(b.tarih).getTime())
    .slice(0, 3);
  
  return (
    <div className={`${themeColors.primary.main} rounded-xl overflow-hidden backdrop-blur-sm ${themeColors.primary.border} shadow-md h-full`}>
      <div className="p-4 relative h-full">
        <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-15"></div>
        
        <div className="relative h-full flex flex-col">
          {/* Başlık */}
          <div className="flex items-center mb-3">
            <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
              <Dock size={16} className={themeColors.primary.textLight} />
            </div>
            <h3 className={`${themeColors.primary.textLight} font-medium text-base`}>Yaklaşan Randevularınız</h3>
          </div>
          
          {/* Yükleniyor */}
          {isLoading && (
            <div className="flex flex-col items-center justify-center text-center flex-grow py-8">
              <div className="w-8 h-8 rounded-full border-2 border-white/10 border-t-white/60 animate-spin mb-3"></div>
              <p className="text-white/70 text-sm">Randevularınız yükleniyor...</p>
            </div>
          )}
          
          {/* Hata mesajı */}
          {!isLoading && error && (
            <div className="flex flex-col items-center justify-center text-center flex-grow py-8">
              <div className="bg-white/10 rounded-full p-3 mb-3">
                <AlertCircle size={22} className="text-white/70" />
              </div>
              <p className="text-white/70 text-sm">{error}</p>
            </div>
          )}
          
          {/* Randevu bulunamadı */}
          {!isLoading && !error && enYakinRandevular.length === 0 && (
            <div className="flex flex-col items-center justify-center text-center flex-grow py-8">
              <div className="bg-white/10 rounded-full p-3 mb-3">
                <Calendar size={22} className="text-white/70" />
              </div>
              <p className="text-white/70 text-sm">Yaklaşan randevunuz bulunmuyor</p>
              <p className="text-white/50 text-xs mt-1">Randevu oluşturmak için "Randevu Oluştur" sayfasını ziyaret edin</p>
            </div>
          )}
          
          {/* Randevu listesi */}
          {!isLoading && !error && enYakinRandevular.length > 0 && (
            <div className="flex flex-col gap-3 flex-grow">
              {enYakinRandevular.map((randevu) => (
                <RandevuSatiri key={randevu.id} randevu={randevu} />
              ))}
            </div>
          )}
          
          {/* Alt buton */}
          {!isLoading && !error && tumYaklasanRandevular.length > 0 && (
            <div className="mt-3">
              <Link to="/ogrenci/randevular" className={`w-full bg-white/8 hover:bg-white/12 transition-all duration-300 ${themeColors.primary.textLight} text-xs py-2 rounded-lg font-light flex items-center justify-center hover:shadow-sm hover:translate-y-[-1px] backdrop-blur-sm ${themeColors.primary.border}`}>
                Tüm randevularınızı görüntüleyin <ChevronRight size={14} className="ml-1.5 group-hover:ml-2 transition-all" />
              </Link>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AnasayfaRandevu;

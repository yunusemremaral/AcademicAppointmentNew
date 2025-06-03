import React from 'react';
import { useNavigate } from 'react-router-dom';
import { ChevronRight, User } from 'lucide-react';
import { Akademisyen } from '../../services/Ogrenci/departmentService';

// AkademisyenLinkItem bileşeni tipleri
interface AkademisyenLinkItemProps {
  akademisyen: Akademisyen;
  displayType?: 'sidebar' | 'card';
  onNavigate?: () => void;
}

/**
 * Akademisyen bağlantısı için ortak bileşen
 * Hem sidebar hem de kart görünümünde kullanılabilir
 */
const AkademisyenLinkItem: React.FC<AkademisyenLinkItemProps> = ({ 
  akademisyen, 
  displayType = 'sidebar', 
  onNavigate 
}) => {
  const navigate = useNavigate();
  const defaultImage = 'https://cdn-icons-png.flaticon.com/512/3135/3135715.png';
  const imageUrl = akademisyen.profileImage || defaultImage;
  
  // İsim formatını profesyonel şekilde parçalara ayır
  const formatAcademicianName = (name: string) => {
    // İsimleri parçalara ayıralım ve unvan/isim şeklinde formatlayalım
    const parts = name.split(" ");
    
    // Unvan kısmını belirleyelim (Dr. Öğr. Üyesi, Prof. Dr. vb.)
    let title = "";
    let firstName = "";
    let lastName = "";
    
    // Ünvan tespit etme
    if (name.includes("Prof. Dr.")) {
      title = "Prof. Dr.";
      firstName = parts.slice(2, -1).join(" ");
      lastName = parts[parts.length - 1];
    } else if (name.includes("Doç. Dr.")) {
      title = "Doç. Dr.";
      firstName = parts.slice(2, -1).join(" ");
      lastName = parts[parts.length - 1];
    } else if (name.includes("Dr. Öğr. Üyesi")) {
      title = "Dr. Öğr. Üyesi";
      firstName = parts.slice(3, -1).join(" ");
      lastName = parts[parts.length - 1];
    } else if (name.includes("Arş. Gör.") || name.includes("Arş. Görevlisi") || name.includes("Araş. Gör.") || name.includes("Araştırma Görevlisi")) {
      title = "Arş. Gör.";
      // Farklı ifade biçimlerine göre başlangıç indeksini ayarla
      let startIndex = 2;
      if (name.includes("Araştırma Görevlisi")) startIndex = 2;
      else if (name.includes("Arş. Görevlisi")) startIndex = 2;
      else if (name.includes("Arş. Gör.")) startIndex = 2;
      else if (name.includes("Araş. Gör.")) startIndex = 2;
      
      firstName = parts.slice(startIndex, -1).join(" ");
      lastName = parts[parts.length - 1];
    } else if (name.includes("Öğr. Gör.") || name.includes("Öğretim Görevlisi")) {
      title = "Öğr. Gör.";
      let startIndex = name.includes("Öğretim Görevlisi") ? 2 : 2;
      firstName = parts.slice(startIndex, -1).join(" ");
      lastName = parts[parts.length - 1];
    } else if (name.includes("Dr.")) {
      title = "Dr.";
      firstName = parts.slice(1, -1).join(" ");
      lastName = parts[parts.length - 1];
    } else {
      // Varsayılan format - ünvan belirlenemedi
      // Eğer isim birden fazla kelimeden oluşuyorsa
      if (parts.length > 1) {
        firstName = parts.slice(0, -1).join(" ");
        lastName = parts[parts.length - 1];
        title = ""; // Ünvan yok
      } else {
        firstName = name;
        lastName = "";
        title = "";
      }
    }
    
    return { title, fullName: `${firstName} ${lastName}`.trim() };
  };

  // Akademisyen verilerini LocalStorage'a kaydet ve yönlendir
  const handleAkademisyenClick = () => {
    // Akademisyen bilgilerini localStorage'a kaydet
    localStorage.setItem('currentAkademisyen', JSON.stringify(akademisyen));
    
    // URL path'i öncelikli kullan, yoksa ID'yi kullan
    const navigationPath = akademisyen.urlPath || akademisyen.id;
    
    // Eğer onNavigate fonksiyonu varsa çağır (mobile menu için)
    if (onNavigate) {
      onNavigate();
    }
    
    // Akademisyen sayfasına yönlendir
    navigate(`/ogrenci/akademisyen/${navigationPath}`);
  };
  
  // Sidebar görünümünde akademisyen öğesi
  if (displayType === 'sidebar') {
    const { title, fullName } = formatAcademicianName(akademisyen.fullName);
    
    return (
      <div 
        onClick={handleAkademisyenClick} 
        className="group flex items-center px-3 py-1.5 max-h-screen-740:py-1 text-xs font-medium mx-2 my-0.5 max-h-screen-740:my-0 text-gray-300 hover:text-white hover:bg-primary-dark/25 rounded-md transition-all duration-200 hover:ring-1 hover:ring-white/5 cursor-pointer"
      >
        <div className="flex items-center min-w-0 flex-1">
          <div className="w-5 h-5 max-h-screen-740:w-4 max-h-screen-740:h-4 flex items-center justify-center text-gray-400 flex-shrink-0">
            <User size={14} className="flex-shrink-0 max-h-screen-740:scale-90" />
          </div>
          <div className="ml-2 flex flex-col overflow-hidden">
            {title ? (
              <span className="text-[9.5px] text-gray-400 leading-tight">{title}</span>
            ) : null}
            <span className={`truncate font-medium text-gray-200 ${!title ? 'mt-1' : ''}`}>{fullName}</span>
          </div>
        </div>
        <ChevronRight size={12} className="text-gray-500 flex-shrink-0 ml-1 opacity-60 group-hover:opacity-100 transition-all duration-200 group-hover:translate-x-0.5" />
      </div>
    );
  }
  
  // Kart görünümünde akademisyen öğesi (default)
  return (
    <div 
      onClick={handleAkademisyenClick}
      className="group cursor-pointer"
    >
      <div className="bg-white rounded-lg overflow-hidden shadow-sm hover:shadow-md transition-all duration-300 border border-gray-100 group-hover:translate-y-[-2px]">
        {/* Akademisyen fotoğrafı */}
        <div className="relative pb-[100%] h-0 overflow-hidden bg-gray-100">
          <img 
            src={imageUrl} 
            alt={`${akademisyen.fullName}`} 
            className="absolute inset-0 w-full h-full object-cover transition-opacity duration-300"
            onError={(e) => {
              (e.target as HTMLImageElement).src = defaultImage;
            }}
          />
          {/* İsim overlay */}
          <div className="absolute inset-x-0 bottom-0 p-2 bg-gradient-to-t from-primary/90 to-primary/10 text-white">
            <div className="text-xs font-medium truncate">{akademisyen.fullName}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AkademisyenLinkItem; 
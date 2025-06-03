import  { memo } from 'react';
import { format } from 'date-fns';
import { tr } from 'date-fns/locale';
import { CalendarClock, Check, AlertCircle, X, Clock } from 'lucide-react';
import { UiRandevu } from '../../../types/randevu';
import { formatAkademisyenIsmi, getInitials } from '../../../utils/formatters';

interface RandevuKartProps {
  randevu: UiRandevu;
  onDeleteClick: (id: number) => void;
}

// Randevu durumuna göre renk ve simge belirleme
const getRandevuStatusInfo = (durum: UiRandevu['durum'], gecmisTarih = false) => {
  // Geçmiş tarihli ve beklemede olan randevular için özel durum
  if (gecmisTarih && durum === 'beklemede') {
    return {
      label: 'Gerçekleşmedi',
      bgColor: 'bg-amber-50',
      textColor: 'text-amber-700', 
      borderColor: 'border-amber-200',
      icon: <AlertCircle size={14} className="mr-1.5" />
    };
  }
  
  // Normal durumlar
  switch (durum) {
    case 'onaylandı':
      return {
        label: 'Onaylandı',
        bgColor: 'bg-green-50',
        textColor: 'text-green-700',
        borderColor: 'border-green-200',
        icon: <Check size={14} className="mr-1.5" />
      };
    case 'beklemede':
      return {
        label: 'Beklemede',
        bgColor: 'bg-blue-50',
        textColor: 'text-blue-700',
        borderColor: 'border-blue-200',
        icon: <Clock size={14} className="mr-1.5" />
      };
    case 'iptal edildi':
      return {
        label: 'İptal Edildi',
        bgColor: 'bg-red-50',
        textColor: 'text-red-700',
        borderColor: 'border-red-200',
        icon: <X size={14} className="mr-1.5" />
      };
    case 'tamamlandı':
      return {
        label: 'Tamamlandı',
        bgColor: 'bg-gray-100',
        textColor: 'text-gray-700',
        borderColor: 'border-gray-200',
        icon: <Check size={14} className="mr-1.5" />
      };
    default:
      return {
        label: 'Bilinmiyor',
        bgColor: 'bg-gray-100',
        textColor: 'text-gray-700',
        borderColor: 'border-gray-200',
        icon: <AlertCircle size={14} className="mr-1.5" />
      };
  }
};

const RandevuKart = memo<RandevuKartProps>(({ randevu, onDeleteClick }) => {
  const tarihDate = new Date(randevu.tarih);
  const gecmisTarih = tarihDate.getTime() < Date.now();
  const { unvan, isim } = formatAkademisyenIsmi(randevu.akademisyen.name);
  const initials = getInitials(randevu.akademisyen.name);
  
  // Durum bilgisini bir kez hesapla
  const statusInfo = getRandevuStatusInfo(randevu.durum, gecmisTarih);
  const { label, bgColor, textColor, borderColor, icon } = statusInfo;
  
  // İptal Et butonunu render et
  const renderIptalButton = () => {
    if (!gecmisTarih && randevu.durum !== 'iptal edildi' && randevu.durum !== 'tamamlandı') {
      return (
        <div className="flex justify-end">
          <button 
            onClick={() => onDeleteClick(randevu.id)}
            className="text-xs bg-red-50 hover:bg-red-100 text-red-700 px-3 py-1.5 rounded-md flex items-center transition-colors"
            title="Randevuyu iptal et"
          >
            <X size={13} className="mr-1.5" /> İptal Et
          </button>
        </div>
      );
    }
    return null;
  };
  
  return (
    <div 
      className={`group border ${borderColor} rounded-lg overflow-hidden bg-white shadow-sm
      transition-all duration-300 hover:shadow-md hover:bg-gray-50/30 mb-1`}
    >
      {/* Ana içerik - Responsive düzen için flex-wrap */}
      <div className="flex flex-wrap items-center px-3.5 py-3 w-full">
        {/* Akademisyen baş harfleri - Görsel iyileştirmelerle */}
        <div className="flex-shrink-0 mr-3 relative order-1">
          <div className="w-10 h-10 rounded-full overflow-hidden bg-gradient-to-br from-neutral-900 to-neutral-700 flex items-center justify-center shadow-sm group-hover:shadow">
            <span className="text-white font-medium text-sm">{initials}</span>
          </div>
        </div>
        
        {/* Akademisyen ve konu bilgisi - Geliştirilmiş tipografi */}
        <div className="flex-grow min-w-[150px] mr-auto order-2 sm:max-w-[40%]">
          <div className="flex items-baseline">
            <p className="text-xs font-medium text-gray-900 truncate group-hover:text-primary/90 transition-colors">
              {unvan} {isim}
            </p>
          </div>
          <p className="text-xs text-primary/80 truncate group-hover:text-primary/90 transition-colors">
            {randevu.konu}
          </p>
        </div>
        
        {/* Sağ taraftaki öğeleri grup olarak sabitleyip hizalıyoruz */}
        <div className="flex items-center justify-end space-x-2.5 w-full sm:w-auto order-4 sm:order-3 mt-2.5 sm:mt-0">
          {/* Tarih bilgisi - İyileştirilmiş tasarım */}
          <div className="flex items-center text-xs text-gray-500 border-l border-gray-100 pl-3 w-[120px] group-hover:text-gray-700 transition-colors">
            <CalendarClock size={13} className="flex-shrink-0 mr-1.5 text-primary/80 group-hover:text-primary transition-colors" />
            <div className="flex flex-col">
              <span className="whitespace-nowrap font-medium">{format(tarihDate, 'd MMMM', { locale: tr })}</span>
              <span className="text-[10px] text-gray-400 group-hover:text-gray-500 transition-colors">{format(tarihDate, 'HH:mm')}</span>
            </div>
          </div>
          
          {/* Durum etiketi - Genişletilmiş genişlik, yazıların tam görünmesi için */}
          <div className={`px-2.5 py-1 rounded-full text-xs font-medium flex items-center justify-center ${bgColor} ${textColor} w-[115px] group-hover:shadow-sm transition-all`}>
            {icon}
            <span className="whitespace-nowrap">{label}</span>
          </div>
        </div>
      </div>
      
      {/* Açıklama ve İptal Et butonu grid içinde yan yana */}
      {randevu.aciklama && (
        <div className="border-t border-gray-100 px-3.5 py-2 bg-gray-50/80 group-hover:bg-gray-50">
          <div className="grid grid-cols-12 gap-2 items-center">
            {/* Açıklama - 8/12 genişlikte */}
            <div className="col-span-12 md:col-span-8">
              <p className="text-xs text-gray-600 italic group-hover:text-gray-700 transition-colors">
                {randevu.aciklama}
              </p>
            </div>
            
            {/* İptal Et butonu - 4/12 genişlikte */}
            {renderIptalButton() && (
              <div className="col-span-12 md:col-span-4 mt-2 md:mt-0">
                {renderIptalButton()}
              </div>
            )}
          </div>
        </div>
      )}
      
      {/* Açıklama yoksa sadece İptal Et butonu */}
      {!randevu.aciklama && renderIptalButton() && (
        <div className="border-t border-gray-100 px-3.5 py-2 bg-gray-50 flex justify-end">
          {renderIptalButton()}
        </div>
      )}
    </div>
  );
});

// Görüntü adı oluştur
RandevuKart.displayName = 'RandevuKart';

export default RandevuKart;

import React from 'react';
import { format, isSameDay, addDays } from 'date-fns';
import { tr } from 'date-fns/locale';

// Tip tanımlamaları
export interface TimeSlot {
  time: Date;
  formattedTime: string;
  isAvailable: boolean;
}

// Randevu saati oluşturma yardımcı fonksiyonu
export const createTimeSlots = (selectedDate: Date): TimeSlot[] => {
  const slots = [];
  // Seçilen tarihin kopyasını oluştur
  const dateToUse = new Date(selectedDate);
  // Tarih kısmını koru, saati sıfırla
  dateToUse.setHours(0, 0, 0, 0);
  
  // Kullanıcının yerel saatinde (Türkiye - UTC+3) randevu saatleri oluştur
  // 9:00'dan 16:30'a kadar yarım saatlik dilimler
  for (let hour = 9; hour <= 16; hour++) {
    for (let minute of [0, 30]) {
      // 16:30'dan sonrasını dahil etme
      if (hour === 16 && minute > 30) continue;
      
      // Her seferinde temiz bir kopya oluştur
      const timeSlot = new Date(dateToUse);
      // Saati ayarla - Bu Türkiye yerel saatidir (UTC+3)
      timeSlot.setHours(hour, minute, 0, 0);
      
      // Tüm zaman dilimleri varsayılan olarak kullanılabilir
      slots.push({
        time: timeSlot,
        formattedTime: format(timeSlot, 'HH:mm'),
        isAvailable: true
      });
    }
  }
  return slots;
};

// Randevu kartı bileşeni
export const AppointmentSlot: React.FC<{
  time: string;
  isAvailable: boolean;
  onClick: () => void;
  isSelected: boolean;
}> = ({ time, isAvailable, onClick, isSelected }) => {
  if (!isAvailable) {
    return (
      <div className="bg-gray-100 text-gray-400 p-3 rounded-md text-center cursor-not-allowed">
        <p className="text-xs">{time}</p>
        <p className="text-[10px]">Uygun Değil</p>
      </div>
    );
  }

  return (
    <div 
      onClick={onClick}
      className={`p-3 rounded-md text-center cursor-pointer transition-all duration-300 ${
        isSelected 
          ? 'ring-2 ring-primary border-transparent shadow-md bg-primary/10 scale-105' 
          : 'bg-gradient-to-r from-green-400/70 to-green-400/90 text-white hover:shadow-md hover:scale-105'
      }`}
    >
      <p className="text-xs font-medium">{time}</p>
      <p className="text-[10px]">Müsait</p>
    </div>
  );
};

// Gün butonları
export const DayButton: React.FC<{
  date: Date;
  isSelected: boolean;
  onClick: () => void;
  isDisabled: boolean;
}> = ({ date, isSelected, onClick, isDisabled }) => {
  const dayName = format(date, 'EEEE', { locale: tr });
  const dayNumber = format(date, 'd');
  const monthName = format(date, 'LLL', { locale: tr });
  
  // Bugünkü tarihin günü ile karşılaştır
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  const checkDate = new Date(date);
  checkDate.setHours(0, 0, 0, 0);
  const isToday = checkDate.getTime() === today.getTime();
  
  return (
    <button
      onClick={onClick}
      disabled={isDisabled}
      className={`p-2 rounded-lg border transition-all duration-300 ${
        isSelected
          ? 'bg-primary text-white border-primary'
          : isDisabled
            ? 'bg-gray-100 text-gray-400 border-gray-200 cursor-not-allowed'
            : isToday
              ? 'bg-blue-50 text-gray-700 border-blue-200 hover:border-blue-300 hover:bg-blue-100'
              : 'bg-white text-gray-700 border-gray-200 hover:border-gray-300 hover:bg-gray-50'
      } flex flex-col items-center justify-center w-full`}
    >
      <span className="text-[10px] font-medium capitalize">{dayName}</span>
      <span className={`text-xl font-bold ${isSelected ? 'text-white' : isToday ? 'text-blue-600' : 'text-gray-800'}`}>{dayNumber}</span>
      <span className="text-[10px] capitalize">{monthName}</span>
      {isToday && !isSelected && !isDisabled && (
        <span className="bg-blue-500 w-1.5 h-1.5 rounded-full absolute -bottom-0.5"></span>
      )}
    </button>
  );
};

// Randevu form alanı bileşeni
export const AppointmentForm: React.FC<{
  konu: string;
  setKonu: (value: string) => void;
  aciklama: string;
  setAciklama: (value: string) => void;
  selectedTimeSlot: Date;
  submitError: string | null;
  setSubmitError: (error: string | null) => void;
  isSubmitting: boolean;
  submitSuccess: boolean;
  handleSubmit: (e: React.FormEvent) => Promise<void>;
  formRef: React.RefObject<any>;
}> = ({
  konu,
  setKonu,
  aciklama,
  setAciklama,
  selectedTimeSlot,
  submitError,
  setSubmitError,
  isSubmitting,
  submitSuccess,
  handleSubmit,
  formRef
}) => {
  return (
    <div ref={formRef} className="p-4 border-t border-gray-100 bg-gray-50">
      <div className="flex items-center mb-4">
        <FileText size={14} className="text-primary mr-1.5" />
        <h3 className="font-medium text-sm text-gray-700">
          Randevu Bilgileri
        </h3>
      </div>
      
      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <label className="text-xs text-gray-700 mb-1 block">
            Randevu Konusu<span className="text-red-500 ml-0.5">*</span>
          </label>
          <input
            type="text"
            value={konu}
            onChange={(e) => {
              setKonu(e.target.value);
              
              // Eğer yazılmaya başlandığında hata mesajı varsa temizleyelim
              if (submitError && e.target.value.trim()) {
                setSubmitError(null);
              }
            }}
            className={`w-full px-3 py-2 border ${
              submitError && !konu.trim() 
                ? 'border-red-300 bg-red-50/50 focus:ring-red-200' 
                : 'border-gray-200 focus:ring-primary/20'
            } rounded-md text-sm focus:outline-none focus:ring-2 transition-colors`}
            placeholder="Randevu konusunu yazın"
            required
            minLength={3}
            maxLength={60}
          />
          {submitError && !konu.trim() && (
            <p className="text-red-500 text-xs mt-1">Randevu konusu boş bırakılamaz</p>
          )}
        </div>
        
        <div>
          <label className="text-xs text-gray-700 mb-1 block">Açıklama (İsteğe bağlı)</label>
          <textarea
            value={aciklama}
            onChange={(e) => setAciklama(e.target.value)}
            className="w-full px-3 py-2 border border-gray-200 rounded-md text-sm h-24 resize-none"
            placeholder="Randevu ile ilgili detayları yazın"
          />
        </div>
        
        <div className="pt-3 flex items-center justify-between">
          <div className="flex items-center text-sm text-gray-600">
            <Dock size={14} className="mr-1.5 text-primary" />
            <span>
              {format(selectedTimeSlot, 'd MMMM', { locale: tr })}, {' '}
              {format(selectedTimeSlot, 'HH:mm')}
            </span>
          </div>
          
          <button
            type="submit"
            disabled={isSubmitting || !konu.trim()}
            className="px-4 py-2 bg-primary text-white rounded-md text-sm font-medium 
              hover:bg-primary-dark focus:outline-none focus:ring-2 focus:ring-primary/30 
              transition-colors disabled:opacity-70 disabled:cursor-not-allowed
              flex items-center"
          >
            {isSubmitting ? (
              <>
                <div className="animate-spin h-4 w-4 border-2 border-white border-t-transparent rounded-full mr-2"></div>
                <span>İşleniyor...</span>
              </>
            ) : (
              'Randevu Oluştur'
            )}
          </button>
        </div>
        
        {/* Başarı mesajı */}
        {submitSuccess && (
          <div className="bg-green-50 text-green-700 p-3 rounded-md text-sm flex items-start">
            <Check size={16} className="mr-2 flex-shrink-0 mt-0.5" />
            <span>Randevunuz başarıyla oluşturuldu!</span>
          </div>
        )}
        
        {/* Hata mesajı */}
        {submitError && (
          <div className="bg-red-50 text-red-700 p-3 rounded-md text-sm flex items-start">
            <AlertCircle size={16} className="mr-2 flex-shrink-0 mt-0.5" />
            <span>{submitError}</span>
          </div>
        )}
      </form>
    </div>
  );
};

// WeekSelector bileşeni
export const WeekSelector: React.FC<{
  currentWeek: 'current' | 'next';
  toggleWeek: (week: 'current' | 'next') => void;
  thisWeekStart?: Date;
  nextWeekStart?: Date;
}> = ({ currentWeek, toggleWeek, thisWeekStart, nextWeekStart }) => {
  // Tarih aralıklarını oluştur
  const formatWeekRange = (startDate: Date) => {
    if (!startDate) return '';
    
    // Pazartesi-Cuma aralığını göster
    const endDate = addDays(startDate, 4); // Pazartesi + 4 gün = Cuma
    return `${format(startDate, 'd', { locale: tr })}-${format(endDate, 'd MMM', { locale: tr })}`;
  };

  // Aktif haftanın başlangıç tarihi
  const activeWeekStart = currentWeek === 'current' 
    ? thisWeekStart 
    : nextWeekStart;
  
  // Tarih bilgisi gösterme
  const showDateRange = thisWeekStart && nextWeekStart;
  
  return (
    <div className="p-4 border-b border-gray-100 flex items-center justify-between">
      <button 
        onClick={() => toggleWeek('current')} 
        disabled={currentWeek === 'current'}
        className={`p-2 rounded-full ${currentWeek === 'current' ? 'text-gray-400 cursor-not-allowed' : 'text-primary hover:bg-primary/5'} transition-colors`}
      >
        <ChevronLeft size={18} />
      </button>
      
      <div className="flex items-center">
        <h2 className="font-medium text-gray-800">
          {currentWeek === 'current' ? 'Bu Hafta' : 'Sonraki Hafta'}
          {showDateRange && activeWeekStart && (
            <span className="text-xs ml-2 text-gray-500">
              ({formatWeekRange(activeWeekStart)})
            </span>
          )}
        </h2>
      </div>
      
      <button 
        onClick={() => toggleWeek('next')} 
        disabled={currentWeek === 'next'}
        className={`p-2 rounded-full ${currentWeek === 'next' ? 'text-gray-400 cursor-not-allowed' : 'text-primary hover:bg-primary/5'} transition-colors`}
      >
        <ChevronRight size={18} />
      </button>
    </div>
  );
};

// TimeSlotsGrid bileşeni
export const TimeSlotsGrid: React.FC<{
  selectedDate: Date;
  timeSlots: TimeSlot[];
  selectedTimeSlot: Date | null;
  onTimeSlotSelect: (time: Date) => void;
}> = ({ selectedDate, timeSlots, selectedTimeSlot, onTimeSlotSelect }) => {
  return (
    <div className="p-4">
      <div className="flex items-center mb-3">
        <Clock size={14} className="text-primary mr-1.5" />
        <h3 className="font-medium text-sm text-gray-700">
          {format(selectedDate, "d MMMM EEEE", { locale: tr })}
        </h3>
      </div>
      
      {timeSlots.length > 0 ? (
        <div className="grid grid-cols-4 sm:grid-cols-6 md:grid-cols-8 gap-2">
          {timeSlots.map((slot, index) => (
            <AppointmentSlot
              key={index}
              time={slot.formattedTime}
              isAvailable={slot.isAvailable}
              onClick={() => onTimeSlotSelect(slot.time)}
              isSelected={selectedTimeSlot ? isSameDay(slot.time, selectedTimeSlot) && 
                slot.time.getHours() === selectedTimeSlot.getHours() && 
                slot.time.getMinutes() === selectedTimeSlot.getMinutes() : false}
            />
          ))}
        </div>
      ) : (
        <div className="bg-amber-50 text-amber-800 p-4 rounded-lg text-center text-sm flex items-center justify-center">
          <AlertCircle size={18} className="mr-1.5" />
          <span>Bu tarih için uygun randevu saati bulunmamaktadır.</span>
        </div>
      )}
    </div>
  );
};

// Lucide ikonları için import
import { FileText, Dock, Check, AlertCircle, ChevronLeft, ChevronRight, Clock } from 'lucide-react';

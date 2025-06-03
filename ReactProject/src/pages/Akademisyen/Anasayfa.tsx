import React from 'react';
import { themeColors } from '../../layouts/AkademisyenLayout/ASidebar';
import { Route, ArrowRight } from 'lucide-react';
import siluetImage from '../../assets/siluet.jpg';

// Akademisyen için bileşenler
import AnasayfaRandevu from '../../components/Akademisyen/Anasayfa/AnasayfaRandevu';
import PlatformTanitim from '../../components/Akademisyen/Anasayfa/PlatformTanitim';

const Anasayfa: React.FC = () => {
  return (
    <div className="py-4 px-3 sm:p-5 lg:p-6 max-w-full font-poppins bg-gray-50/5">
      {/* Ana Hoş Geldiniz Kartı - Modern kurumsal tasarım */}
      <div className="mb-4">
        <div className={`${themeColors.primary.main} rounded-xl overflow-hidden relative backdrop-blur-sm ${themeColors.primary.border} shadow-lg`}>
          {/* Silüet Arka Planı */}
          <div className="absolute inset-0 z-0">
            <div className="absolute inset-0 bg-black/80"></div>
            <img 
              src={siluetImage} 
              alt="" 
              className="h-full w-full object-contain object-right opacity-30 mix-blend-soft-light brightness-70 contrast-110 scale-100"
            />
          </div>

          <div className="absolute inset-0 bg-gradient-to-br from-white/5 via-white/3 to-transparent opacity-15 z-10"></div>
          
          {/* Dekoratif arka plan elementleri - İnce, zarif şekiller */}
          <div className="absolute -right-10 -top-10 w-32 h-32 bg-white/5 rounded-full blur-2xl z-10"></div>
          <div className="absolute right-1/4 bottom-0 w-20 h-20 bg-white/5 rounded-full blur-xl z-10"></div>
          <div className="absolute left-1/3 top-1/2 w-12 h-12 bg-white/5 rounded-full blur-lg z-10"></div>
          <div className="absolute left-1/4 bottom-8 w-24 h-24 bg-white/5 rounded-full blur-xl z-10"></div>
          
          <div className="p-5 sm:p-6 lg:p-8 relative z-20">
            <div className="relative max-w-2xl">
              <h1 className="text-lg sm:text-xl lg:text-2xl font-medium text-white mb-3 tracking-tight">
                Akademik Randevu Platformu
              </h1>
              <p className="text-gray-200/90 text-xs sm:text-sm leading-relaxed font-light max-w-lg">
                Akademik Randevu Platformu ile öğrencilerinizin randevu taleplerini sistematik bir şekilde yönetebilir, 
                görüşme programınızı etkili biçimde organize edebilir ve tüm akademik danışmanlık süreçlerinizi merkezi olarak takip edebilirsiniz.
              </p>
              <button className={`mt-4 self-start bg-white/15 hover:bg-white/20 ${themeColors.primary.textLight} text-xs py-2 px-5 rounded-lg flex items-center transition-all duration-300 hover:shadow-md hover:translate-x-0.5 backdrop-blur-sm ${themeColors.primary.border}`}>
                Randevuları Görüntüle <ArrowRight size={14} className="ml-1.5 group-hover:ml-2 transition-all" />
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* İçerik Grid Düzeni - Zarif kurumsal tasarım */}
      <div className="grid grid-cols-1 md:grid-cols-12 gap-4">
        {/* Yaklaşan Randevular Kartı - AnasayfaRandevu bileşeni 
            Mobilde 2. sırada görünecek (order-1), masaüstünde sağda */}
        <div className="md:col-span-6 order-1 md:order-2">
          <AnasayfaRandevu />
        </div>

        {/* Randevu Süreci Kartı - İnce, zarif tasarım
            Mobilde 3. sırada görünecek (order-2), masaüstünde solda */}
        <div className={`${themeColors.primary.main} rounded-xl overflow-hidden md:col-span-6 order-2 md:order-1 backdrop-blur-sm ${themeColors.primary.border} shadow-md`}>
          <div className="p-4 relative h-full">
            <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-15"></div>
            
            <div className="relative h-full flex flex-col">
              <div className="flex items-center mb-3">
                <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
                  <Route size={16} className={themeColors.primary.textLight} />
                </div>
                <h3 className={`${themeColors.primary.textLight} font-medium text-base`}>Randevu Süreci</h3>
              </div>
              
              <div className="grid grid-cols-1 gap-2 flex-grow">
                <div className={`flex items-center bg-white/5 rounded-lg p-3 ${themeColors.primary.border} backdrop-blur-sm`}>
                  <div className="flex-shrink-0 w-6 h-6 rounded-full bg-white/10 flex items-center justify-center text-white text-xs mr-2 border border-white/10 shadow-inner">
                    <span>1</span>
                  </div>
                  <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>Öğrenciler tarafından oluşturulan randevu talepleri size mail bildirimi ile iletilir ve "Randevular" sayfasında listelenir.</p>
                </div>
                <div className={`flex items-center bg-white/5 rounded-lg p-3 ${themeColors.primary.border} backdrop-blur-sm`}>
                  <div className="flex-shrink-0 w-6 h-6 rounded-full bg-white/10 flex items-center justify-center text-white text-xs mr-2 border border-white/10 shadow-inner">
                    <span>2</span>
                  </div>
                  <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>Gelen talepleri inceleyerek akademik program ve uygunluk durumunuza göre onaylayabilir veya reddedebilirsiniz.</p>
                </div>
                <div className={`flex items-center bg-white/5 rounded-lg p-3 ${themeColors.primary.border} backdrop-blur-sm`}>
                  <div className="flex-shrink-0 w-6 h-6 rounded-full bg-white/10 flex items-center justify-center text-white text-xs mr-2 border border-white/10 shadow-inner">
                    <span>3</span>
                  </div>
                  <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>Verdiğiniz cevap öğrenciye otomatik olarak mail ile bildirilir ve sistemde anlık olarak güncellenir.</p>
                </div>
              </div>
              
              <div className={`bg-amber-50 mt-3 rounded-lg p-3 border border-amber-200/50 shadow-sm`}>
                <div className="flex items-start">
                  <p className={`text-[11px] text-amber-800 leading-relaxed`}>
                    Onaylanan bir randevuya öğrencinin katılmaması durumunda, yoklama durumunu 'Katılmadı' olarak işaretleyebilirsiniz. Bu işlem, öğrencinin 14 gün süresince yeni randevu oluşturma hakkını kısıtlayacaktır.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Platform Tanıtımı Kartı - Mobilde en altta */}
        <div className="md:col-span-12 order-3">
          <PlatformTanitim />
        </div>
      </div>
    </div>
  );
};

export default Anasayfa;

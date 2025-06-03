import React from 'react';
import { themeColors } from '../../layouts/OgrenciLayout/OgrenciSidebar';
import { Route, ArrowRight } from 'lucide-react';
import AnasayfaRandevu from '../../components/Ogrenci/Anasayfa/AnasayfaRandevu';
import AnasayfaBolumler from '../../components/Ogrenci/Anasayfa/AnasayfaBolumler';
import PlatformTanitim from '../../components/Ogrenci/Anasayfa/PlatformTanitim';
import siluetImage from '../../assets/siluet.jpg';

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
                Akademik Randevu Platformu sayesinde, akademisyenlerle görüşme taleplerinizi birkaç adımda iletebilir, randevu onay süreçlerini takip edebilir ve tüm görüşmelerinizi kolayca organize edebilirsiniz.
              </p>
              <button className={`mt-4 self-start bg-white/15 hover:bg-white/20 ${themeColors.primary.textLight} text-xs py-2 px-5 rounded-lg flex items-center transition-all duration-300 hover:shadow-md hover:translate-x-0.5 backdrop-blur-sm ${themeColors.primary.border}`}>
                Hemen Başla <ArrowRight size={14} className="ml-1.5 group-hover:ml-2 transition-all" />
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* İçerik Grid Düzeni - Zarif kurumsal tasarım */}
      <div className="grid grid-cols-1 md:grid-cols-12 gap-4">
        {/* Randevu Süreci Kartı - İnce, zarif tasarım */}
        <div className={`${themeColors.primary.main} rounded-xl overflow-hidden md:col-span-6 backdrop-blur-sm ${themeColors.primary.border} shadow-md`}>
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
                  <div className={`w-6 h-6 rounded-full bg-white/10 flex items-center justify-center ${themeColors.primary.textLight} text-xs font-light mr-2 ${themeColors.primary.border}`}>1</div>
                  <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>Randevu oluştur</p>
                </div>
                <div className={`flex items-center bg-white/5 rounded-lg p-3 ${themeColors.primary.border} backdrop-blur-sm`}>
                  <div className={`w-6 h-6 rounded-full bg-white/10 flex items-center justify-center ${themeColors.primary.textLight} text-xs font-light mr-2 ${themeColors.primary.border}`}>2</div>
                  <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>Akademisyen yanıtını bekle.</p>
                </div>
                <div className={`flex items-center bg-white/5 rounded-lg p-3 ${themeColors.primary.border} backdrop-blur-sm`}>
                  <div className={`w-6 h-6 rounded-full bg-white/10 flex items-center justify-center ${themeColors.primary.textLight} text-xs font-light mr-2 ${themeColors.primary.border}`}>3</div>
                  <p className={`${themeColors.primary.textLight}/90 text-xs font-light`}>Onay veya ret bilgisi mailinize iletilir, randevular kısmında da görünür.</p>
                </div>
              </div>
              
              <div className={`bg-amber-50 mt-3 rounded-lg p-2 text-center`}>
                <p className={`text-[11px] text-amber-800 leading-relaxed `}>Onaylanan bir randevuya katılmamanız durumunda, yeni randevu oluşturma hakkınız 14 gün süreyle kısıtlanır.</p>
              </div>
            </div>
          </div>
        </div>

        {/* Yaklaşan Randevular Kartı - AnasayfaRandevu bileşeni */}
        <div className="md:col-span-6">
          <AnasayfaRandevu />
        </div>

        {/* Bölümler Kartı - Dinamik AnasayfaBolumler bileşeni */}
        <AnasayfaBolumler />

        {/* Platform Tanıtımı Kartı - Bileşen olarak eklendi */}
        <div className="md:col-span-6">
          <PlatformTanitim />
        </div>
      </div>
    </div>
  );
};

export default Anasayfa;

import React from 'react';
import { Video } from 'lucide-react';
import { themeColors } from '../../../layouts/OgrenciLayout/OgrenciSidebar';

const PlatformTanitim: React.FC = () => {
  return (
    <div className={`${themeColors.primary.main} rounded-xl overflow-hidden md:col-span-6 backdrop-blur-sm ${themeColors.primary.border} shadow-md`}>
      <div className="p-4 relative h-full">
        <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-15"></div>
        
        <div className="relative h-full flex flex-col">
          <div className="flex items-center mb-3">
            <div className={`bg-white/8 p-1.5 rounded-lg mr-2 shadow-sm ${themeColors.primary.border}`}>
              <Video size={16} className={themeColors.primary.textLight} />
            </div>
            <h3 className={`${themeColors.primary.textLight} font-medium text-base`}>Platform Tanıtımı</h3>
          </div>
          
          <div className={`bg-gradient-to-br from-white/5 to-white/2 rounded-lg p-4 flex items-center justify-center flex-grow backdrop-blur-sm ${themeColors.primary.border}`}>
            <div className="text-center">
              <div className={`bg-white/8 hover:bg-white/15 p-3 rounded-full inline-block mb-3 cursor-pointer transition-all duration-300 hover:shadow-md ${themeColors.primary.border}`}>
                <Video size={20} className={themeColors.primary.textLight} />
              </div>
              <p className={`${themeColors.primary.textLight}/80 text-[11px] font-light max-w-xs mx-auto leading-relaxed`}>Platformun nasıl kullanılacağını görmek için tanıtım videosunu izleyin</p>
              <button className={`mt-3 px-4 py-2 bg-white/8 hover:bg-white/12 transition-all duration-300 ${themeColors.primary.textLight} text-xs rounded-lg font-light hover:shadow-sm hover:translate-y-[-1px] backdrop-blur-sm ${themeColors.primary.border}`}>
                Video İzle
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PlatformTanitim;

import React, { ReactNode } from 'react';
import { ChevronLeft } from 'lucide-react';

// Types
interface RightPanelProps {
  title: string;
  subtitle: string;
  children: ReactNode;
  onBackToPortal?: () => void;
  showBackButton?: boolean;
}

// Help link component
const HelpLink: React.FC = () => {
  return (
    <a 
      href="#" 
      className="inline-flex items-center text-primary hover:text-primary-dark transition-colors duration-300 text-sm font-medium"
    >
      <span>Bize Ulaşın</span>
    </a>
  );
};

// Back to Portal button component
const BackToPortalButton: React.FC<{ onClick: () => void }> = ({ onClick }) => {
  return (
    <button 
      onClick={onClick}
      className="flex items-center text-gray-600 hover:text-gray-800 transition-colors group absolute top-4 left-4 z-10"
    >
      <ChevronLeft size={18} className="group-hover:-translate-x-0.5 transition-transform" />
      <span className="text-sm font-medium ml-1">Portal'a Geri Dön</span>
    </button>
  );
};

// Title section component
const TitleSection: React.FC<{ title: string, subtitle: string }> = ({ title, subtitle }) => {
  return (
    <div className="text-center mb-6">
      <h2 className="text-2xl font-bold text-gray-800 drop-shadow-sm relative">
        {title}
      </h2>
      <p className="text-gray-600 text-sm tracking-wide max-w-xs mx-auto">{subtitle}</p>
    </div>
  );
};

// Help section component with fixed height
const HelpSection: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center text-gray-500">
      <div className="flex flex-col items-center">
        <p className="text-xs mb-3 tracking-wide font-light">Sorun mu yaşıyorsunuz?</p>
        <div className="group px-4 py-1.5 rounded-md transition-all duration-300 hover:bg-gray-100 border border-transparent hover:border-gray-200">
          <HelpLink />
        </div>
      </div>
    </div>
  );
};

// Main component
const RightPanel: React.FC<RightPanelProps> = ({ 
  title, 
  subtitle, 
  children, 
  onBackToPortal,
  showBackButton = false
}) => {
  return (
    <div className="bg-gray-50 h-full flex flex-col overflow-hidden relative">
      {/* ÜST KISIM - Boş alan ve geri düğmesi */}
      <div className="h-[120px] shrink-0 pt-8 px-8 relative z-10">
        {/* Geri düğmesi sadece mobilde gösterilir */}
        {showBackButton && onBackToPortal && (
          <BackToPortalButton onClick={onBackToPortal} />
        )}
      </div>
      
      {/* ORTA KISIM - Kaydırılabilir form alanı */}
      <div className="flex-1 overflow-auto px-8 mask-fade-bottom flex flex-col">
        <div className="my-auto py-8">
          <div className="w-full max-w-md mx-auto">
            <TitleSection title={title} subtitle={subtitle} />
            <div className="relative">
              {children}
            </div>
          </div>
        </div>
      </div>
      
      {/* ALT KISIM - Yardım bölümü */}
      <div className="h-[100px] shrink-0 px-8 py-4 bg-gray-50 z-10">
        <div className="h-full flex items-center justify-center">
          <div className="w-full max-w-xs">
            <HelpSection />
          </div>
        </div>
      </div>
    </div>
  );
};

export default RightPanel;
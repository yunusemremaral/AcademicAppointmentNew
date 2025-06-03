import React from 'react';

// Genel amaçlı yükleniyor bileşeni
const Loading: React.FC<{ text?: string; minHeight?: string }> = ({ 
  text = "Yükleniyor...",
  minHeight = "300px" 
}) => {
  return (
    <div className={`flex justify-center items-center py-10`} style={{ minHeight }}>
      <div className="text-center">
        <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-primary mx-auto mb-2"></div>
        <span className="text-sm text-gray-600">{text}</span>
      </div>
    </div>
  );
};

export default Loading;

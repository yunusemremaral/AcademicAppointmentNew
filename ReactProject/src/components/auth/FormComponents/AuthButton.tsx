import React from 'react';

interface AuthButtonProps {
  type?: 'button' | 'submit' | 'reset';
  onClick?: () => void;
  disabled?: boolean;
  loading?: boolean;
  redirecting?: boolean;
  className?: string;
  children?: React.ReactNode;
}

/**
 * Yetkilendirme formları için özel buton bileşeni
 * Loading ve redirecting durumları için uygun görünümler içerir
 */
const AuthButton: React.FC<AuthButtonProps> = ({
  type = 'submit',
  onClick,
  disabled = false,
  loading = false,
  redirecting = false,
  className = '',
  children
}) => {
  const baseClasses = 'w-full bg-primary text-white py-2.5 px-4 rounded-md hover:bg-gray-700 focus:ring-2 focus:ring-gray-800/50 active:bg-gray-900 transition-all font-medium shadow-sm disabled:opacity-70 disabled:cursor-not-allowed';
  
  // Loading ve redirecting durumlarını hesapla
  const isDisabled = disabled || loading || redirecting;
  
  // Buton içeriği
  const getButtonContent = () => {
    if (loading) {
      return (
        <span className="flex items-center justify-center">
          <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          Giriş Yapılıyor...
        </span>
      );
    }
    
    if (redirecting) {
      return (
        <span className="flex items-center justify-center">
          <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          Yönlendiriliyor...
        </span>
      );
    }
    
    return children || 'Giriş Yap';
  };
  
  return (
    <button 
      type={type}
      onClick={onClick}
      disabled={isDisabled}
      className={`${baseClasses} ${className}`}
    >
      {getButtonContent()}
    </button>
  );
};

export default AuthButton; 
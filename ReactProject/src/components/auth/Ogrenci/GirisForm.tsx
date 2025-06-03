import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ogrenciGiris, LoginFormData, RoleCheckResult } from '../../../services/Ogrenci/authService';
import { useAuth } from '../../../contexts/AuthContext';
import EmailAutocomplete from '../FormComponents/EmailAutocomplete';
import AuthButton from '../FormComponents/AuthButton';

// Oturum işlemlerini kullanacak bileşen
const GirisForm: React.FC = () => {
  const { login } = useAuth();
  const navigate = useNavigate();

  // Form durumları
  const [formData, setFormData] = useState<LoginFormData>({
    email: '',
    password: ''
  });
  
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [redirecting, setRedirecting] = useState(false);
  
  // Form değişikliklerini yakala
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };
  
  // Email değişikliğini yakala
  const handleEmailChange = (email: string) => {
    setFormData(prev => ({ ...prev, email }));
  };
  
  // Yanlış rol durumunda yapılacak işlemler
  const handleWrongRole = (result: RoleCheckResult) => {
    setError(result.errorMessage || 'Bu giriş formuna erişim yetkiniz yok.');
    setRedirecting(true);
    
    // 2 saniye sonra yönlendirme yap
    setTimeout(() => {
      if (result.redirectPath) {
        navigate(result.redirectPath);
      }
    }, 2000);
  };

  // Form gönderme işlemi
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Form doğrulama
    if (!formData.email || !formData.password) {
      setError('Lütfen tüm alanları doldurun');
      return;
    }

    try {
      setLoading(true);
      setError(null);
      setRedirecting(false);
      
      // API isteğini gönder
      await ogrenciGiris(
        formData,
        // Başarı callback'i - AuthContext'teki login fonksiyonunu kullan
        (token: string) => {
          login(token); // AuthContext'in login metodu sayfa yönlendirmesini de yapacak
        },
        // Hata callback'i
        (errorMessage: string) => {
          setError(errorMessage);
          setLoading(false);
        },
        // Yanlış rol callback'i
        handleWrongRole
      );
    } catch (error) {
      setError('Giriş sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      setLoading(false);
    }
  };

  return (
    <div>
      {/* Giriş/Kayıt Geçiş Butonları */}
      <div className="flex mb-7 border border-gray-200 rounded-md p-1 bg-gray-50 shadow-sm">
        <Link 
          to="/portal/ogrenci"
          className="flex-1 py-2 px-4 rounded-md bg-white text-center text-gray-800 font-medium text-sm shadow-sm border border-gray-200"
        >
          Giriş
        </Link>
        <Link 
          to="/portal/ogrenci-kayit"
          className="flex-1 py-2 px-4 rounded-md text-center text-gray-600 font-medium text-sm hover:bg-gray-100 transition-colors"
        >
          Kayıt
        </Link>
      </div>
      
      {/* Hata Mesajı */}
      {error && (
        <div className={`bg-red-50 text-red-600 p-3 rounded-md mb-4 text-sm border border-red-100 ${redirecting ? 'animate-pulse' : ''}`}>
          {error}
          {redirecting && (
            <div className="mt-1 text-xs">
              Yönlendiriliyorsunuz...
            </div>
          )}
        </div>
      )}
      
      <form onSubmit={handleSubmit} className="space-y-5">
        {/* Email Alanı */}
        <div className="form-group">
          <label className="block text-gray-700 text-sm font-medium mb-1.5">Mail</label>
          <EmailAutocomplete 
            value={formData.email}
            onChange={handleEmailChange}
            disabled={loading || redirecting}
              placeholder="Okul numaranızı veya mail adresinizi girin" 
            required
          />
        </div>
        
        {/* Şifre Alanı */}
        <div className="form-group">
          <label className="block text-gray-700 text-sm font-medium mb-1.5">Şifre</label>
          <div className="relative">
            <input 
              type="password" 
              name="password"
              value={formData.password}
              onChange={handleChange}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
              placeholder="Şifrenizi girin" 
              disabled={loading || redirecting}
              autoComplete="current-password"
              required
            />
          </div>
          <div className="flex justify-end mt-1.5">
            <Link to="/portal/sifre-sifirlama" className="text-xs text-gray-500 hover:text-primary transition-colors">
              Şifremi unuttum
            </Link>
          </div>
        </div>
        
        {/* Giriş Butonu */}
        <div className="mt-7">
          <AuthButton
            loading={loading}
            redirecting={redirecting}
            disabled={loading || redirecting}
          >
            Giriş Yap
          </AuthButton>
        </div>
      </form>
    </div>
  );
};

// Ana bileşen
const OgrenciGiris: React.FC = () => {
  return <GirisForm />;
};

export default OgrenciGiris; 
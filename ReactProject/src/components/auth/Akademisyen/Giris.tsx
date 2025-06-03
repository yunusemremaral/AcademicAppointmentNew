import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../../contexts/AuthContext';
import { akademisyenGiris, LoginFormData, RoleCheckResult } from '../../../services/Ogrenci/authService';
import EmailAutocomplete from '../FormComponents/EmailAutocomplete';
import AuthButton from '../FormComponents/AuthButton';

const AkademisyenGiris: React.FC = () => {
  const [formData, setFormData] = useState<LoginFormData>({
    email: '',
    password: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [redirecting, setRedirecting] = useState(false);
  
  const { login } = useAuth();
  const navigate = useNavigate();
  
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
  
  // Form gönderildiğinde çalışacak fonksiyon
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Form doğrulama
    if (!formData.email.trim() || !formData.password.trim()) {
      setError('Lütfen tüm alanları doldurun');
      return;
    }
    
    setLoading(true);
    setError('');
    setRedirecting(false);
    
    try {
      // Akademisyen giriş servisini çağır
      await akademisyenGiris(
        formData,
        // Başarı callback'i - token AuthContext'e iletilecek
        (token) => {
          login(token);
        },
        // Hata callback'i
        (errorMessage) => {
          setError(errorMessage);
          setLoading(false);
        },
        // Yanlış rol callback'i
        handleWrongRole
      );
    } catch (err: any) {
      setError(err.message || 'Giriş yapılırken bir hata oluştu');
      setLoading(false);
    }
  };
  
  return (
    <div className="space-y-5">
      {/* Hata mesajı */}
      {error && (
        <div className={`p-3 bg-red-50 border border-red-200 text-red-500 rounded-md text-sm ${redirecting ? 'animate-pulse' : ''}`}>
          {error}
          {redirecting && (
            <div className="mt-1 text-xs">
              Yönlendiriliyorsunuz...
            </div>
          )}
        </div>
      )}
      
      <form onSubmit={handleSubmit}>
      {/* Mail Alanı */}
      <div className="form-group">
        <label className="block text-gray-700 text-sm font-medium mb-1.5">Mail</label>
          <EmailAutocomplete 
            value={formData.email}
            onChange={handleEmailChange}
            disabled={loading || redirecting}
            placeholder="Mail adresinizi girin" 
            required
          />
      </div>
      
      {/* Şifre Alanı */}
        <div className="form-group mt-4">
        <label className="block text-gray-700 text-sm font-medium mb-1.5">Şifre</label>
        <div className="relative">
          <input 
            type="password" 
              name="password"
            className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
            placeholder="Şifrenizi girin" 
              value={formData.password}
              onChange={handleChange}
              disabled={loading || redirecting}
              required
              autoComplete="current-password"
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

export default AkademisyenGiris; 
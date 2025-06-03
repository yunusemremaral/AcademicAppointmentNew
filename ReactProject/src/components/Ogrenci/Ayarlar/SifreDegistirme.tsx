import React, { useState, useEffect } from 'react';
import { themeColors } from '../../../layouts/OgrenciLayout/OgrenciSidebar';
import { Lock, Eye, EyeOff, Save, AlertCircle } from 'lucide-react';
import { resetPassword } from '../../../services/Ogrenci/settingService';
import { useAuth } from '../../../contexts/AuthContext';

const SifreDegistirme: React.FC = () => {
  const { user, token } = useAuth();
  const [formData, setFormData] = useState({
    yeniSifre: '',
    yeniSifreTekrar: ''
  });
  
  const [showPasswords, setShowPasswords] = useState({
    yeniSifre: false,
    yeniSifreTekrar: false
  });
  
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formMessage, setFormMessage] = useState<{type: 'success' | 'error', text: string} | null>(null);

  // Token ve kullanıcı kontrolü
  useEffect(() => {
    if (!token || !user?.email) {
      setFormMessage({
        type: 'error',
        text: 'Oturum bilgileriniz bulunamadı. Lütfen yeniden giriş yapın.'
      });
    }
  }, [token, user]);

  // Form değişikliklerini takip eden fonksiyon
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    // Form mesajını temizle
    if (formMessage) {
      setFormMessage(null);
    }
  };

  // Şifre görünürlüğünü değiştiren fonksiyon
  const togglePasswordVisibility = (field: keyof typeof showPasswords) => {
    setShowPasswords(prev => ({
      ...prev,
      [field]: !prev[field]
    }));
  };

  // Form gönderim fonksiyonu
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Form doğrulama
    if (!formData.yeniSifre || !formData.yeniSifreTekrar) {
      setFormMessage({
        type: 'error',
        text: 'Lütfen tüm alanları doldurun.'
      });
      return;
    }
    
    if (formData.yeniSifre !== formData.yeniSifreTekrar) {
      setFormMessage({
        type: 'error',
        text: 'Yeni şifreler eşleşmiyor.'
      });
      return;
    }
    
    if (formData.yeniSifre.length < 6) {
      setFormMessage({
        type: 'error',
        text: 'Yeni şifre en az 6 karakter olmalıdır.'
      });
      return;
    }
    
    // Token ve kullanıcı kontrolü
    if (!token || !user?.email) {
      setFormMessage({
        type: 'error',
        text: 'Oturum bilgileriniz bulunamadı. Lütfen yeniden giriş yapın.'
      });
      return;
    }
    
    // API çağrısı
    setIsSubmitting(true);
    
    try {
      // Şifre değiştirme API çağrısı
      const response = await resetPassword(
        user.email,
        token,
        formData.yeniSifre
      );
      
      if (response.success) {
        setFormMessage({
          type: 'success',
          text: 'Şifreniz başarıyla değiştirildi.'
        });
        
        // Form verilerini temizle
        setFormData({
          yeniSifre: '',
          yeniSifreTekrar: ''
        });
      } else {
        throw new Error(response.message || 'Şifre değiştirme işlemi başarısız oldu.');
      }
    } catch (error) {
      setFormMessage({
        type: 'error',
        text: error instanceof Error ? error.message : 'Şifre değiştirme işlemi sırasında bir hata oluştu.'
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="p-4 bg-white/5 rounded-xl backdrop-blur-sm border border-white/10 h-full">
      <div className="flex items-center mb-4">
        <div className={`p-1.5 rounded-md mr-3 bg-white/10 ${themeColors.primary.border}`}>
          <Lock size={16} className={themeColors.primary.textLight} />
        </div>
        <h3 className={`${themeColors.primary.textLight} text-base font-medium`}>Şifre Değiştir</h3>
      </div>
      
      <p className={`${themeColors.primary.textLight}/80 text-xs font-light mb-6`}>
        Güvenliğiniz için şifrenizi düzenli olarak değiştirmenizi öneririz. Şifreniz en az 6 karakter uzunluğunda olmalıdır.
      </p>
      
      {/* Oturum kontrolü */}
      {(!token || !user?.email) ? (
        <div className="bg-red-500/20 border border-red-500/30 p-4 rounded-lg mb-4 flex items-start">
          <AlertCircle size={16} className="text-red-300 mr-2 mt-0.5 flex-shrink-0" />
          <p className="text-red-200 text-xs">
            Oturum bilgilerinize erişilemiyor. Lütfen sayfayı yenileyip tekrar deneyin veya yeniden giriş yapın.
          </p>
        </div>
      ) : null}
      
      {formMessage && (
        <div className={`p-3 rounded-lg mb-4 text-xs ${
          formMessage.type === 'success' 
            ? 'bg-green-500/20 border border-green-500/30 text-green-200' 
            : 'bg-red-500/20 border border-red-500/30 text-red-200'
        }`}>
          {formMessage.text}
        </div>
      )}
      
      <form onSubmit={handleSubmit}>
        <div className="space-y-4">
          {/* Yeni Şifre */}
          <div>
            <label htmlFor="yeniSifre" className={`block ${themeColors.primary.textLight}/90 text-xs font-medium mb-1.5`}>
              Yeni Şifre
            </label>
            <div className="relative">
              <input
                type={showPasswords.yeniSifre ? "text" : "password"}
                id="yeniSifre"
                name="yeniSifre"
                value={formData.yeniSifre}
                onChange={handleChange}
                className="w-full bg-white/10 border border-white/10 rounded-lg px-3 py-2 text-xs text-white placeholder-white/50 focus:outline-none focus:ring-1 focus:ring-white/20"
                placeholder="Yeni şifrenizi girin"
                disabled={!token || !user?.email}
              />
              <button
                type="button"
                onClick={() => togglePasswordVisibility('yeniSifre')}
                className="absolute right-2 top-1/2 transform -translate-y-1/2 text-white/50 hover:text-white/80"
                disabled={!token || !user?.email}
              >
                {showPasswords.yeniSifre ? (
                  <EyeOff size={14} />
                ) : (
                  <Eye size={14} />
                )}
              </button>
            </div>
          </div>
          
          {/* Yeni Şifre Tekrar */}
          <div>
            <label htmlFor="yeniSifreTekrar" className={`block ${themeColors.primary.textLight}/90 text-xs font-medium mb-1.5`}>
              Yeni Şifre (Tekrar)
            </label>
            <div className="relative">
              <input
                type={showPasswords.yeniSifreTekrar ? "text" : "password"}
                id="yeniSifreTekrar"
                name="yeniSifreTekrar"
                value={formData.yeniSifreTekrar}
                onChange={handleChange}
                className="w-full bg-white/10 border border-white/10 rounded-lg px-3 py-2 text-xs text-white placeholder-white/50 focus:outline-none focus:ring-1 focus:ring-white/20"
                placeholder="Yeni şifrenizi tekrar girin"
                disabled={!token || !user?.email}
              />
              <button
                type="button"
                onClick={() => togglePasswordVisibility('yeniSifreTekrar')}
                className="absolute right-2 top-1/2 transform -translate-y-1/2 text-white/50 hover:text-white/80"
                disabled={!token || !user?.email}
              >
                {showPasswords.yeniSifreTekrar ? (
                  <EyeOff size={14} />
                ) : (
                  <Eye size={14} />
                )}
              </button>
            </div>
          </div>
          
          {/* Kullanıcı bilgileri - Sadece gösterim amaçlı */}
          {user?.email && (
            <div className="pt-2 pb-1 border-t border-white/10">
              <p className={`${themeColors.primary.textLight}/70 text-[11px] font-light`}>
                <strong>Bağlı hesap:</strong> {user.email}
              </p>
            </div>
          )}
          
          {/* Kaydet Butonu */}
          <div className="pt-2">
            <button
              type="submit"
              disabled={isSubmitting || !token || !user?.email}
              className={`w-full bg-white/15 hover:bg-white/20 ${themeColors.primary.textLight} text-xs py-2.5 px-4 rounded-lg font-medium flex items-center justify-center hover:shadow-sm transition-all duration-300 ${themeColors.primary.border} disabled:opacity-50 disabled:cursor-not-allowed`}
            >
              {isSubmitting ? (
                <>
                  <div className="h-3.5 w-3.5 animate-spin rounded-full border-2 border-t-transparent border-white mr-2"></div>
                  Kaydediliyor...
                </>
              ) : (
                <>
                  <Save size={14} className="mr-1.5" />
                  Şifreyi Değiştir
                </>
              )}
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default SifreDegistirme; 
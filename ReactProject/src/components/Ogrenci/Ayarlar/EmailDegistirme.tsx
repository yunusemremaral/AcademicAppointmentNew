import React, { useState } from 'react';
import { themeColors } from '../../../layouts/OgrenciLayout/OgrenciSidebar';
import { Mail, Save } from 'lucide-react';
import { useAuth } from '../../../contexts/AuthContext';

const EmailDegistirme: React.FC = () => {
  const { user } = useAuth();
  
  const [formData, setFormData] = useState({
    mevcutEmail: user?.email || '',
    yeniEmail: '',
    sifre: ''
  });
  
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formMessage, setFormMessage] = useState<{type: 'success' | 'error', text: string} | null>(null);

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

  // Form gönderim fonksiyonu
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    // Form doğrulama
    if (!formData.yeniEmail || !formData.sifre) {
      setFormMessage({
        type: 'error',
        text: 'Lütfen tüm alanları doldurun.'
      });
      return;
    }
    
    // Email formatı doğrulama
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.yeniEmail)) {
      setFormMessage({
        type: 'error',
        text: 'Lütfen geçerli bir e-posta adresi girin.'
      });
      return;
    }
    
    // Mevcut email ile yeni email aynı mı kontrolü
    if (formData.mevcutEmail === formData.yeniEmail) {
      setFormMessage({
        type: 'error',
        text: 'Yeni e-posta adresi mevcut adresinizle aynı olamaz.'
      });
      return;
    }
    
    // API çağrısı simülasyonu
    setIsSubmitting(true);
    
    // API çağrısı burada yapılacak
    setTimeout(() => {
      setIsSubmitting(false);
      setFormMessage({
        type: 'success',
        text: 'E-posta adresiniz başarıyla değiştirildi. Yeni adresinize doğrulama maili gönderildi.'
      });
      
      // Form verilerini temizle
      setFormData(prev => ({
        ...prev,
        yeniEmail: '',
        sifre: ''
      }));
    }, 1500);
  };

  return (
    <div className="p-4 bg-white/5 rounded-xl backdrop-blur-sm border border-white/10 h-full">
      <div className="flex items-center mb-4">
        <div className={`p-1.5 rounded-md mr-3 bg-white/10 ${themeColors.primary.border}`}>
          <Mail size={16} className={themeColors.primary.textLight} />
        </div>
        <h3 className={`${themeColors.primary.textLight} text-base font-medium`}>E-posta Değiştir</h3>
      </div>
      
      <p className={`${themeColors.primary.textLight}/80 text-xs font-light mb-6`}>
        E-posta adresinizi değiştirdiğinizde, yeni adresinize bir doğrulama bağlantısı gönderilecektir. 
        Değişikliğin tamamlanması için bu bağlantıya tıklamanız gerekecektir.
      </p>
      
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
          {/* Mevcut Email */}
          <div>
            <label htmlFor="mevcutEmail" className={`block ${themeColors.primary.textLight}/90 text-xs font-medium mb-1.5`}>
              Mevcut E-posta Adresi
            </label>
            <input
              type="email"
              id="mevcutEmail"
              name="mevcutEmail"
              value={formData.mevcutEmail}
              disabled
              className="w-full bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-xs text-white/70 focus:outline-none cursor-not-allowed"
            />
            <p className="text-white/50 text-[10px] mt-1">Bu alan değiştirilemez.</p>
          </div>
          
          {/* Yeni Email */}
          <div>
            <label htmlFor="yeniEmail" className={`block ${themeColors.primary.textLight}/90 text-xs font-medium mb-1.5`}>
              Yeni E-posta Adresi
            </label>
            <input
              type="email"
              id="yeniEmail"
              name="yeniEmail"
              value={formData.yeniEmail}
              onChange={handleChange}
              className="w-full bg-white/10 border border-white/10 rounded-lg px-3 py-2 text-xs text-white placeholder-white/50 focus:outline-none focus:ring-1 focus:ring-white/20"
              placeholder="Yeni e-posta adresinizi girin"
            />
          </div>
          
          {/* Şifre Doğrulama */}
          <div>
            <label htmlFor="sifre" className={`block ${themeColors.primary.textLight}/90 text-xs font-medium mb-1.5`}>
              Şifreniz (Doğrulama için)
            </label>
            <input
              type="password"
              id="sifre"
              name="sifre"
              value={formData.sifre}
              onChange={handleChange}
              className="w-full bg-white/10 border border-white/10 rounded-lg px-3 py-2 text-xs text-white placeholder-white/50 focus:outline-none focus:ring-1 focus:ring-white/20"
              placeholder="Güvenlik için şifrenizi girin"
            />
          </div>
          
          {/* Kaydet Butonu */}
          <div className="pt-2">
            <button
              type="submit"
              disabled={isSubmitting}
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
                  E-posta Adresini Değiştir
                </>
              )}
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default EmailDegistirme; 
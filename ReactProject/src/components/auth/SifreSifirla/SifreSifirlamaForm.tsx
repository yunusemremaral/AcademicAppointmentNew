import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { sifremiUnuttum } from '../../../services/Ogrenci/authService';

const SifreSifirlama: React.FC = () => {
  // Form state
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  
  // Email autocomplete için state
  const [emailInput, setEmailInput] = useState('');
  const [domains] = useState<string[]>([
    'mf.karaelmas.edu.tr',
    'gmail.com'
  ]);
  const [domainSuggestions, setDomainSuggestions] = useState<string[]>([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  
  // Email input değiştiğinde domain önerilerini filtrele
  useEffect(() => {
    const atIndex = emailInput.indexOf('@');
    
    if (atIndex > 0) {
      const prefix = emailInput.substring(0, atIndex);
      const typedDomain = emailInput.substring(atIndex + 1).toLowerCase();
      
      // Domain önerilerini filtrele
      const filteredDomains = domains.filter(domain => 
        domain.toLowerCase().startsWith(typedDomain)
      );
      
      setDomainSuggestions(filteredDomains.map(domain => `${prefix}@${domain}`));
      setShowSuggestions(filteredDomains.length > 0);
    } else {
      setShowSuggestions(false);
    }
  }, [emailInput, domains]);
  
  // Email input değişikliğini yakala
  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmailInput(e.target.value);
    setEmail(e.target.value);
  };
  
  // Domain önerisi seçildiğinde
  const handleSelectDomain = (email: string) => {
    setEmailInput(email);
    setEmail(email);
    setShowSuggestions(false);
  };
  
  // Form gönderimi
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // Başarı ve hata mesajlarını temizle
    setSuccess(null);
    setError(null);
    
    // Email kontrolü
    if (!email.trim()) {
      setError('Lütfen e-posta adresinizi girin');
      return;
    }
    
    try {
      setLoading(true);
      
      // Şifre sıfırlama API isteğini gönder
      await sifremiUnuttum(
        email,
        // Başarı callback'i
        (message) => {
          setSuccess(message);
          setEmailInput(''); // Formu temizle
          setEmail('');
        },
        // Hata callback'i
        (message) => {
          setError(message);
        }
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-5">
      {/* Açıklama Metni */}
      <div className="mb-2">
        <p className="text-sm text-gray-600">
          Şifrenizi sıfırlamak için kayıtlı mail adresinizi girin. Şifre sıfırlama bağlantısı mail adresinize gönderilecektir.
        </p>
      </div>
      
      {/* Başarı Mesajı */}
      {success && (
        <div className="bg-green-50 text-green-600 p-3 rounded-md mb-4 text-sm border border-green-100">
          {success}
        </div>
      )}
      
      {/* Hata Mesajı */}
      {error && (
        <div className="bg-red-50 text-red-600 p-3 rounded-md mb-4 text-sm border border-red-100">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit}>
        {/* Mail Alanı */}
        <div className="form-group">
          <label className="block text-gray-700 text-sm font-medium mb-1.5">Mail</label>
          <div className="relative">
            <input 
              type="email" 
              className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
              placeholder="Mail adresinizi girin" 
              value={emailInput}
              onChange={handleEmailChange}
              onBlur={() => setTimeout(() => setShowSuggestions(false), 200)}
              disabled={loading}
              autoComplete="email"
            />
            
            {/* Email Autocomplete Dropdown */}
            {showSuggestions && (
              <div className="absolute z-10 w-full mt-1 bg-white border border-gray-300 rounded-md shadow-lg max-h-60 overflow-auto">
                <ul className="py-1">
                  {domainSuggestions.map((suggestion, index) => (
                    <li 
                      key={index} 
                      className="px-4 py-2 text-gray-800 hover:bg-gray-100 cursor-pointer"
                      onClick={() => handleSelectDomain(suggestion)}
                    >
                      {suggestion}
                    </li>
                  ))}
                </ul>
              </div>
            )}
          </div>
        </div>
        
        {/* Gönder Butonu */}
        <div className="mt-7">
          <button 
            type="submit"
            className="w-full bg-primary text-white py-2.5 px-4 rounded-md hover:bg-gray-700 focus:ring-2 focus:ring-gray-800/50 active:bg-gray-900 transition-all font-medium shadow-sm disabled:opacity-70 disabled:cursor-not-allowed"
            disabled={loading}
          >
            {loading ? (
              <span className="flex items-center justify-center">
                <span className="mr-2 h-4 w-4 animate-spin rounded-full border-2 border-t-transparent border-white"></span>
                İşleniyor...
              </span>
            ) : (
              'Şifre Sıfırlama Bağlantısı Gönder'
            )}
          </button>
        </div>
      </form>
      
      {/* Giriş sayfasına dönüş linki */}
      <div className="text-center mt-6 pt-4 border-t border-gray-100">
        <Link 
          to="/portal/ogrenci" 
          className="text-gray-600 hover:text-primary text-sm font-medium transition-colors inline-flex items-center"
        >
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="mr-1.5">
            <path d="m15 18-6-6 6-6"/>
          </svg>
          Giriş sayfasına dön
        </Link>
      </div>
    </div>
  );
};

export default SifreSifirlama;

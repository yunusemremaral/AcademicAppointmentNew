import React, { useState, useEffect, FormEvent, ChangeEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { 
  ogrenciKayit, 
  validateForm, 
  createRegisterData,
  FormData,
  getFormVerileri,
  getBolumlerByUniversite
} from '../../../services/Ogrenci/formDataService';
import { School, Department } from '../../../lib/api';

const OgrenciKayit: React.FC = () => {
  const navigate = useNavigate();
  
  // Form state
  const [formData, setFormData] = useState<FormData & { schoolId?: number; departmentId?: number }>({
    userFullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    schoolId: 1,    // Varsayılan üniversite ID
    departmentId: 1 // Varsayılan bölüm ID
  });
  
  // Üniversite ve bölüm verileri için state
  const [okullar, setOkullar] = useState<School[]>([]);
  const [bolumler, setBolumler] = useState<Department[]>([]);
  const [dataLoading, setDataLoading] = useState(true);
  
  // Autocomplete özelliği için domain listesi
  const [domains] = useState<string[]>([
    'mf.karaelmas.edu.tr',
    'gmail.com'
  ]);
  
  // Email autocomplete için state
  const [emailInput, setEmailInput] = useState('');
  const [domainSuggestions, setDomainSuggestions] = useState<string[]>([]);
  const [showSuggestions, setShowSuggestions] = useState(false);
  
  // İşlem durumları
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  
  // Sayfa yüklendiğinde form verilerini getir
  useEffect(() => {
    const fetchFormData = async () => {
      try {
        setDataLoading(true);
        // Tüm form verilerini tek seferde getir
        const formVerileri = await getFormVerileri();
        
        setOkullar(formVerileri.schools);
        setBolumler(formVerileri.departments);
        
        // İlk üniversitenin ve bölümün ID'lerini al
        const defaultSchoolId = formVerileri.schools.length > 0 ? formVerileri.schools[0].id : 1;
        const defaultDepartmentId = formVerileri.departments.length > 0 ? formVerileri.departments[0].id : 1;
        
        // Formda varsayılan değerleri ayarla
        setFormData(prev => ({
          ...prev,
          schoolId: defaultSchoolId,
          departmentId: defaultDepartmentId
        }));
      } catch (err) {
        console.error("Form verileri yüklenirken hata:", err);
        setError("Üniversite ve bölüm bilgileri yüklenemedi");
      } finally {
        setDataLoading(false);
      }
    };
    
    fetchFormData();
  }, []);
  
  // Üniversite değiştiğinde ilgili bölümleri getir
  const handleUniversiteChange = async (universiteId: number) => {
    try {
      setDataLoading(true);
      // Seçilen üniversiteye ait bölümleri getir
      const departments = await getBolumlerByUniversite(universiteId);
      setBolumler(departments);
      
      // İlk bölümü seç veya varsayılan olarak 1 kullan
      const defaultDepartmentId = departments.length > 0 ? departments[0].id : 1;
      
      // Formda üniversite ve bölüm değerlerini güncelle
      setFormData(prev => ({
        ...prev,
        schoolId: universiteId,
        departmentId: defaultDepartmentId
      }));
    } catch (err) {
      console.error(`${universiteId} ID'li üniversiteye ait bölümler yüklenirken hata:`, err);
      setError("Bölüm bilgileri yüklenemedi");
    } finally {
      setDataLoading(false);
    }
  };
  
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
  
  // Form değişikliklerini yakala
  const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    
    if (name === 'email') {
      setEmailInput(value);
    }
    
    if (name === 'schoolId') {
      // Üniversite seçildiğinde bölümleri güncelle
      handleUniversiteChange(Number(value));
    } else {
      // Diğer form alanlarını güncelle
      setFormData(prev => ({ 
        ...prev, 
        [name]: name === 'schoolId' || name === 'departmentId' ? Number(value) : value 
      }));
    }
  };
  
  // Domain önerisi seçildiğinde
  const handleSelectDomain = (email: string) => {
    setEmailInput(email);
    setFormData(prev => ({ ...prev, email }));
    setShowSuggestions(false);
  };
  
  // Form gönderme işlemi
  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    
    // Form doğrulama
    const validationError = validateForm(formData);
    if (validationError) {
      setError(validationError);
      return;
    }
    
    try {
      setLoading(true);
      setError(null);
      
      // createRegisterData fonksiyonunu kullanarak kayıt verisi oluştur
      const kayitVerisi = createRegisterData(formData);
      
      // API kaydı gerçekleştir
      await ogrenciKayit(
        kayitVerisi,
        // Başarı callback'i
        () => {
          setSuccess(true);
          setSuccessMessage('Kayıt işleminiz başarıyla tamamlandı. Email onay bağlantısı gönderildi.');
          // 2 saniye sonra giriş sayfasına yönlendir
          setTimeout(() => {
            navigate('/portal/ogrenci');
          }, 2000);
        },
        // Hata callback'i
        (errorMessage: string) => {
          setError(errorMessage);
        }
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      {/* Giriş/Kayıt Geçiş Butonları */}
      <div className="flex mb-7 border border-gray-200 rounded-md p-1 bg-gray-50 shadow-sm">
        <Link 
          to="/portal/ogrenci"
          className="flex-1 py-2 px-4 rounded-md text-center text-gray-600 font-medium text-sm hover:bg-gray-100 transition-colors"
        >
          Giriş
        </Link>
        <Link 
          to="/portal/ogrenci-kayit"
          className="flex-1 py-2 px-4 rounded-md bg-white text-center text-gray-800 font-medium text-sm shadow-sm border border-gray-200"
        >
          Kayıt
        </Link>
      </div>
      
      {/* Başarı Mesajı */}
      {success && (
        <div className="bg-green-50 text-green-600 p-3 rounded-md mb-4 text-sm border border-green-100">
          {successMessage}
        </div>
      )}
      
      {/* Hata Mesajı */}
      {error && (
        <div className="bg-red-50 text-red-600 p-3 rounded-md mb-4 text-sm border border-red-100">
          {error}
        </div>
      )}
      
      <form onSubmit={handleSubmit} className="space-y-4" autoComplete="off">
        {/* Kişisel Bilgiler Grubu */}
        <div className="form-group">
          <label className="block text-gray-700 text-sm font-medium mb-1.5">Ad Soyad</label>
          <div className="relative">
            <input 
              type="text" 
              name="userFullName"
              value={formData.userFullName}
              onChange={handleChange}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
              placeholder="Adınızı ve soyadınızı girin" 
              disabled={loading}
              autoComplete="name"
            />
          </div>
        </div>
        
        {/* Email Alanı */}
        <div className="form-group">
          <label className="block text-gray-700 text-sm font-medium mb-1.5">Mail</label>
          <div className="relative">
            <input 
              type="email" 
              name="email"
              value={emailInput}
              onChange={handleChange}
              onBlur={() => setTimeout(() => setShowSuggestions(false), 200)}
              className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
              placeholder="ornek@universite.edu.tr" 
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
        
        {/* Kurum Bilgileri - Dinamik Değerler */}
        <div className="space-y-4">
          {/* Üniversite Seçimi */}
          <div className="form-group">
            <label className="block text-gray-700 text-sm font-medium mb-1.5">Üniversite</label>
            <div className="relative">
              <select 
                name="schoolId"
                value={formData.schoolId}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60 appearance-none"
                disabled={loading || dataLoading || okullar.length === 0}
              >
                {dataLoading ? (
                  <option value="">Yükleniyor...</option>
                ) : okullar.length === 0 ? (
                  <option value="">Üniversite bulunamadı</option>
                ) : (
                  okullar.map(okul => (
                    <option key={okul.id} value={okul.id}>
                      {okul.name}
                    </option>
                  ))
                )}
              </select>
              <div className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none text-gray-500">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="m6 9 6 6 6-6"/>
                </svg>
              </div>
            </div>
          </div>
          
          {/* Bölüm Seçimi */}
          <div className="form-group">
            <label className="block text-gray-700 text-sm font-medium mb-1.5">Bölüm</label>
            <div className="relative">
              <select 
                name="departmentId"
                value={formData.departmentId}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60 appearance-none"
                disabled={loading || dataLoading || bolumler.length === 0}
              >
                {dataLoading ? (
                  <option value="">Yükleniyor...</option>
                ) : bolumler.length === 0 ? (
                  <option value="">Bölüm bulunamadı</option>
                ) : (
                  bolumler.map(bolum => (
                    <option key={bolum.id} value={bolum.id}>
                      {bolum.name}
                    </option>
                  ))
                )}
              </select>
              <div className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none text-gray-500">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="m6 9 6 6 6-6"/>
                </svg>
              </div>
            </div>
          </div>
          {/* Yükleniyor Göstergesi */}
          {dataLoading && (
            <div className="flex items-center justify-center py-1">
              <div className="w-4 h-4 border-2 border-gray-300 border-t-gray-700 rounded-full animate-spin"></div>
              <span className="ml-2 text-xs text-gray-500">Üniversite ve bölüm bilgileri yükleniyor...</span>
            </div>
          )}
        </div>
        
        {/* Şifre Alanları */}
        <div className="space-y-4">
          <div className="form-group">
            <label className="block text-gray-700 text-sm font-medium mb-1.5">Şifre</label>
            <div className="relative">
              <input 
                type="password" 
                name="password"
                value={formData.password}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
                placeholder="Şifre oluşturun" 
                disabled={loading}
                autoComplete="new-password"
              />
            </div>
          </div>
          
          <div className="form-group">
            <label className="block text-gray-700 text-sm font-medium mb-1.5">Şifre Tekrar</label>
            <div className="relative">
              <input 
                type="password" 
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleChange}
                className="w-full px-4 py-2.5 border border-gray-300 rounded-md focus:ring-1 focus:ring-primary/50 focus:border-primary outline-none transition-all text-gray-800 bg-white/60" 
                placeholder="Şifrenizi tekrar girin" 
                disabled={loading}
                autoComplete="new-password"
              />
            </div>
          </div>
        </div>
        
        {/* Kayıt Butonu */}
        <div className="mt-7">
          <button 
            type="submit"
            className="w-full bg-primary text-white py-2.5 px-4 rounded-md hover:bg-gray-700 focus:ring-2 focus:ring-gray-800/50 active:bg-gray-900 transition-all font-medium shadow-sm disabled:opacity-70 disabled:cursor-not-allowed"
            disabled={loading || dataLoading}
          >
            {loading ? 'Kayıt Yapılıyor...' : 'Kayıt Ol'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default OgrenciKayit; 
import { Route, Navigate, BrowserRouter, Routes } from 'react-router-dom';
import './index.css';
import AuthLayout from './layouts/AuthLayout/AuthLayout';
import OgrenciLayout from './layouts/OgrenciLayout/OgrenciLayout';
import AkademisyenLayout from './layouts/AkademisyenLayout/ALayout';
import { AuthProvider } from './contexts/AuthContext';
import PrivateRoute from './auth/PrivateRoute';
import AuthGuard from './auth/AuthGuard';
import { SearchProvider } from './contexts/SearchContext';

// Giriş formları
import OgrenciGiris from './components/auth/Ogrenci/GirisForm';
import OgrenciKayit from './components/auth/Ogrenci/KayitForm';
import AkademisyenGiris from './components/auth/Akademisyen/Giris';
import SifreSifirlama from './components/auth/SifreSifirla/SifreSifirlamaForm';

import Anasayfa from './pages/Ogrenci/Anasayfa';
import Bolumler from './pages/Ogrenci/Bolumler';
import BolumDetay from './pages/Ogrenci/BolumDetay';
import Randevular from './pages/Ogrenci/Randevular';
import Akademisyen from './pages/Ogrenci/Akademisyen';
import Ayarlar from './pages/Ogrenci/Ayarlar';

import AkAnasayfa from './pages/Akademisyen/Anasayfa';
import AkRandevular from './pages/Akademisyen/Randevular';
import OgrenciListesi from './pages/Akademisyen/OgrenciListesi';
import AkAyarlar from './pages/Akademisyen/Ayarlar';

// App bileşeni
function App() {
  return (
    <BrowserRouter>
      <SearchProvider>
        <AuthProvider>
          <AuthGuard>
            <AppRoutes />
          </AuthGuard>
        </AuthProvider>
      </SearchProvider>
    </BrowserRouter>
  );
}

// Uygulama rotaları
function AppRoutes() {
  return (
    <Routes>
      {/* Ana sayfa için yönlendirme */}
      <Route path="/" element={<Navigate to="/portal/ogrenci" replace />} />
      
      {/* AuthLayout ile kimlik doğrulama sayfaları */}
      <Route path="/portal" element={<AuthLayout />}>
        {/* URL'e göre formların gösterilmesi */}
        <Route index element={<Navigate to="/portal/ogrenci" replace />} />
        <Route path="ogrenci" element={<OgrenciGiris />} />
        <Route path="ogrenci-kayit" element={<OgrenciKayit />} />
        <Route path="akademisyen" element={<AkademisyenGiris />} />
        <Route path="sifre-sifirlama" element={<SifreSifirlama />} />
      </Route>
      
      {/* Öğrenci sayfaları - PrivateRoute ile korunuyor */}
      <Route 
        path="/ogrenci" 
        element={
          <PrivateRoute allowedRoles={['Student']}>
            <OgrenciLayout />
          </PrivateRoute>
        }
      >
        <Route index element={<Anasayfa />}/>
        <Route path="anasayfa" element={<Anasayfa />}/>
        <Route path="bolumler" element={<Bolumler />} />
        <Route path="bolumler/:bolumId" element={<BolumDetay />} />
        <Route path="randevular" element={<Randevular />} />
        <Route path="akademisyen-bul" element={<div>Akademisyen Arama İçeriği Gelecek</div>} />
        <Route path="akademisyen/:urlPath" element={<Akademisyen />} />
        <Route path="ayarlar" element={<Ayarlar />} />
      </Route>
      
      {/* Akademisyen sayfaları - PrivateRoute ile korunuyor */}
      <Route 
        path="/akademisyen" 
        element={
          <PrivateRoute allowedRoles={['Instructor']}>
            <AkademisyenLayout />
          </PrivateRoute>
        }
      >
        <Route index element={<div>Akademisyen Anasayfa İçeriği Gelecek</div>}/>
        <Route path="anasayfa" element={<AkAnasayfa />}/>
        <Route path="bolumler" element={<div>Akademisyen Bölümler İçeriği Gelecek</div>} />
        <Route path="bolumler/:bolumId" element={<div>Akademisyen Bölüm Detay İçeriği Gelecek</div>} />
        <Route path="randevular" element={<AkRandevular />} />
        <Route path="ogrenciler" element={<OgrenciListesi />} />
        <Route path="ayarlar" element={<AkAyarlar />} />
      </Route>
      
      {/* Oturumu kapatma işlemi için özel rota */}
      <Route path="/logout" element={<Navigate to="/portal/ogrenci" replace />} />
      
      {/* Geçersiz URL'ler için yönlendirme */}
      <Route path="*" element={<Navigate to="/portal/ogrenci" replace />} />
    </Routes>
  );
}

export default App;

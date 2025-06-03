import React, { useState } from 'react';
import { themeColors } from '../../../layouts/OgrenciLayout/OgrenciSidebar';
import { Bell, Save } from 'lucide-react';

interface BildirimAyari {
  id: string;
  baslik: string;
  aciklama: string;
  aktif: boolean;
}

const BildirimAyarlari: React.FC = () => {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formMessage, setFormMessage] = useState<{type: 'success' | 'error', text: string} | null>(null);
  
  // Bildirim ayarları
  const [bildirimler, setBildirimler] = useState<BildirimAyari[]>([
    {
      id: 'randevu_onay',
      baslik: 'Randevu Onay Bildirimleri',
      aciklama: 'Randevu talebiniz onaylandığında veya reddedildiğinde e-posta bildirimi alın.',
      aktif: true
    },
    {
      id: 'randevu_hatirlatma',
      baslik: 'Randevu Hatırlatmaları',
      aciklama: 'Onaylanan randevularınız için hatırlatma e-postaları alın.',
      aktif: true
    },
    {
      id: 'sistem_duyuru',
      baslik: 'Sistem Duyuruları',
      aciklama: 'Platform üzerindeki güncellemeler ve duyurular hakkında bildirim alın.',
      aktif: false
    },
    {
      id: 'akademisyen_mesaj',
      baslik: 'Akademisyen Mesajları',
      aciklama: 'Akademisyenler tarafından gönderilen mesajlar hakkında bildirim alın.',
      aktif: true
    },
    {
      id: 'bolum_duyuru',
      baslik: 'Bölüm Duyuruları',
      aciklama: 'Kayıtlı olduğunuz bölümlerin duyuruları hakkında bildirim alın.',
      aktif: true
    }
  ]);

  // Bildirim durumunu değiştiren fonksiyon
  const handleToggle = (id: string) => {
    setBildirimler(prev => 
      prev.map(bildirim => 
        bildirim.id === id 
          ? { ...bildirim, aktif: !bildirim.aktif } 
          : bildirim
      )
    );
    
    // Form mesajını temizle
    if (formMessage) {
      setFormMessage(null);
    }
  };

  // Form gönderim fonksiyonu
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    // API çağrısı simülasyonu
    setIsSubmitting(true);
    
    // API çağrısı burada yapılacak
    setTimeout(() => {
      setIsSubmitting(false);
      setFormMessage({
        type: 'success',
        text: 'Bildirim ayarlarınız başarıyla güncellendi.'
      });
    }, 1500);
  };

  return (
    <div className="p-4 bg-white/5 rounded-xl backdrop-blur-sm border border-white/10 h-full">
      <div className="flex items-center mb-4">
        <div className={`p-1.5 rounded-md mr-3 bg-white/10 ${themeColors.primary.border}`}>
          <Bell size={16} className={themeColors.primary.textLight} />
        </div>
        <h3 className={`${themeColors.primary.textLight} text-base font-medium`}>Bildirim Ayarları</h3>
      </div>
      
      <p className={`${themeColors.primary.textLight}/80 text-xs font-light mb-6`}>
        Hangi durumlarda bildirim almak istediğinizi seçin. Bildirimler hem e-posta hem de sistem içi bildirim olarak gönderilecektir.
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
        <div className="space-y-3">
          {/* Bildirim Ayarları Listesi */}
          {bildirimler.map((bildirim) => (
            <div 
              key={bildirim.id} 
              className={`p-3 rounded-lg bg-white/5 border ${themeColors.primary.border}`}
            >
              <div className="flex items-center justify-between">
                <div>
                  <h4 className={`${themeColors.primary.textLight} text-xs font-medium`}>
                    {bildirim.baslik}
                  </h4>
                  <p className={`${themeColors.primary.textLight}/70 text-[10px] font-light mt-1`}>
                    {bildirim.aciklama}
                  </p>
                </div>
                
                {/* Toggle Switch */}
                <label className="relative inline-flex items-center cursor-pointer">
                  <input 
                    type="checkbox" 
                    className="sr-only peer" 
                    checked={bildirim.aktif}
                    onChange={() => handleToggle(bildirim.id)}
                  />
                  <div className={`w-9 h-5 bg-white/10 rounded-full peer peer-checked:after:translate-x-full after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:rounded-full after:h-4 after:w-4 after:transition-all peer-checked:bg-blue-600/50`}></div>
                </label>
              </div>
            </div>
          ))}
          
          {/* Kaydet Butonu */}
          <div className="pt-3">
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
                  Ayarları Kaydet
                </>
              )}
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default BildirimAyarlari; 
// UI için kullanılan Randevu arayüzü
export interface UiRandevu {
  id: number;
  akademisyen: {
    id: string;
    name: string;
    fotograf_url: string | null;
    unvan: string;
  };
  tarih: string; // ISO formatında tarih string
  konu: string;
  aciklama: string | null;
  durum: 'onaylandı' | 'beklemede' | 'iptal edildi' | 'tamamlandı';
} 
import { fetchApi, API_ENDPOINTS } from '../../lib/api';

// Randevu durumları için enum tanımı
export enum RandevuDurum {
  Beklemede = 0,
  Onaylandi = 1,
  IptalEdildi = 2,
  Tamamlandi = 3
}

// Randevu veri tipi
export interface Randevu {
  id: number;
  academicUserId: string;
  studentUserId: string;
  academicUser: string; // API'den gelen akademisyen adı
  studentUser: string; // API'den gelen öğrenci adı
  scheduledAt: string; // ISO formatında tarih
  subject: string;
  description: string;
  status: RandevuDurum;
}

// Randevu detayları için interface
export interface RandevuDetay {
  AcademicUserId: string;
  StudentUserId: string;
  ScheduledAt: string;
  Subject: string;
  Description: string;
}

// Randevu oluşturma isteği için interface
export interface RandevuOlusturRequest {
  AcademicUserId: string;
  StudentUserId: string;
  ScheduledAt: string;
  Subject: string;
  Description: string;
}

// Randevu oluşturma yanıtı için interface
export interface RandevuOlusturResponse {
  success: boolean;
  message?: string;
  appointmentId?: string;
}

// Randevu durumu güncelleme isteği için interface
export interface RandevuDurumGuncelleRequest {
  appointmentId: number;
  status: number;
}

// Randevu durumu güncelleme yanıtı için interface
export interface RandevuDurumGuncelleResponse {
  success: boolean;
  message?: string;
}

/**
 * Randevu oluşturan fonksiyon
 * @param academicUserId Akademisyen ID'si
 * @param studentUserId Öğrenci ID'si 
 * @param scheduledAt Randevu zamanı (Date objesi)
 * @param subject Randevu konusu
 * @param description Randevu açıklaması
 * @returns Randevu oluşturma sonucu
 */
export async function createAppointment(
  academicUserId: string,
  studentUserId: string,
  scheduledAt: string | Date,
  subject: string,
  description: string
): Promise<RandevuOlusturResponse> {
  try {
    // Parametreleri kontrol et
    if (!academicUserId) console.error('academicUserId boş!');
    if (!studentUserId) console.error('studentUserId boş!');
    if (!subject) console.error('subject boş!');
    if (!description) console.error('description boş!');
    if (!scheduledAt) console.error('scheduledAt boş!');

    // ScheduledAt string veya Date olabilir, ISO formatına dönüştür
    let formattedScheduledAt: string;
    
    if (scheduledAt instanceof Date) {
      // Sorun: Backend Türkiye saatini (UTC+3) doğru şekilde işlemiyor ve 3 saat geriye alıyor
      // Çözüm: Gönderdiğimiz zamanı 3 saat ileri alarak bu dönüşümü telafi edelim
      
      // Önce bir kopya oluştur, orijinal Date'i değiştirmeyelim
      const adjustedDate = new Date(scheduledAt);
      
      // Türkiye saati ile UTC arasındaki 3 saatlik farkı telafi etmek için
      // saate +3 ekleyelim (backend'in -3 uygulamasını dengelemek için)
      adjustedDate.setHours(adjustedDate.getHours() + 3);
      
      // Şimdi normal ISO string formatına dönüştürelim
      // Not: Artık zaman dilimi eki (+03:00) eklemiyoruz, çünkü backend UTC bekliyor
      formattedScheduledAt = adjustedDate.toISOString().split('.')[0];
      
      console.log(`Orijinal seçilen tarih: ${scheduledAt}`);
      console.log(`Ayarlanmış (3 saat ileri) ve gönderilen tarih: ${formattedScheduledAt}`);
    } else {
      // Eğer zaten string ise, tarihi doğru formata çevirelim
      try {
        const date = new Date(scheduledAt);
        if (isNaN(date.getTime())) {
          throw new Error('Geçersiz tarih formatı');
        }
        
        // String olsa bile aynı düzeltmeyi yapalım
        const adjustedDate = new Date(date);
        adjustedDate.setHours(adjustedDate.getHours() + 3);
        formattedScheduledAt = adjustedDate.toISOString().split('.')[0];
      } catch (error) {
        console.error('Tarih dönüştürme hatası:', error);
        throw new Error('Randevu tarihi geçersiz formatta');
      }
    }

    // API isteği verilerini hazırla
    const requestData: RandevuOlusturRequest = {
      AcademicUserId: academicUserId,
      StudentUserId: studentUserId,
      ScheduledAt: formattedScheduledAt,
      Subject: subject,
      Description: description || " " // Boşsa en azından bir boşluk karakteri gönder
    };

    // İstek verilerini konsola yaz
    console.log('Randevu isteği gönderiliyor:', requestData);

    // API isteğini gönder
    const response = await fetchApi<RandevuOlusturResponse>(
      API_ENDPOINTS.ADMIN.APPOINTMENT,
      'POST',
      requestData
    );

    console.log('Randevu API yanıtı:', response);
    return response;
  } catch (error) {
    console.error('Randevu oluşturma hatası:', error);
    
    // Hata durumunda başarısız sonuç döndür
    return {
      success: false,
      message: error instanceof Error ? error.message : 'Randevu oluşturulurken bir hata oluştu'
    };
  }
}

/**
 * Öğrencinin randevularını getiren fonksiyon
 * @param studentId Öğrenci ID'si
 * @returns Randevu listesi
 */
export async function getStudentAppointments(studentId: string): Promise<Randevu[]> {
  try {
    // API endpoint'ini hazırla
    const endpoint = API_ENDPOINTS.ADMIN.STUDENT_GET_APPOINTMENTS.replace('{studentId}', studentId);
    
    // API isteğini gönder
    const response = await fetchApi<Randevu[]>(endpoint);
    
    // Yanıtı döndür
    return response;
  } catch (error) {
    console.error('Randevu bilgileri alınırken hata:', error);
    throw error;
  }
}

/**
 * Randevu durumunu string'e dönüştüren yardımcı fonksiyon
 * @param status Randevu durum kodu
 * @returns Durum metni
 */
export function getRandevuDurumText(status: RandevuDurum): 'beklemede' | 'onaylandı' | 'iptal edildi' | 'tamamlandı' {
  switch (status) {
    case RandevuDurum.Beklemede:
      return 'beklemede';
    case RandevuDurum.Onaylandi:
      return 'onaylandı';
    case RandevuDurum.IptalEdildi:
      return 'iptal edildi';
    case RandevuDurum.Tamamlandi:
      return 'tamamlandı';
    default:
      return 'beklemede';
  }
}

/**
 * Randevu durumunu güncelleyen fonksiyon
 * @param appointmentId Randevu ID'si
 * @param status Yeni randevu durumu
 * @returns İşlem sonucu
 */
export async function updateAppointmentStatus(
  appointmentId: number, 
  status: RandevuDurum
): Promise<RandevuDurumGuncelleResponse> {
  try {
    // İstek verilerini hazırla
    const requestData: RandevuDurumGuncelleRequest = {
      appointmentId: appointmentId,
      status: status
    };
    
    console.log('Randevu durumu güncelleme isteği gönderiliyor:', requestData);
    
    // API isteğini gönder
    const response = await fetchApi<RandevuDurumGuncelleResponse>(
      API_ENDPOINTS.ADMIN.APPOINTMENT_UPDATE_STATUS,
      'PUT',
      requestData
    );
    
    console.log('Randevu durumu güncelleme yanıtı:', response);
    return response;
  } catch (error) {
    console.error('Randevu durumu güncellenirken hata:', error);
    return {
      success: false,
      message: error instanceof Error ? error.message : 'Randevu durumu güncellenirken bir hata oluştu'
    };
  }
}

/**
 * Randevu iptal etme fonksiyonu
 * @param appointmentId Randevu ID'si
 * @returns İşlem sonucu
 */
export async function cancelAppointment(appointmentId: number): Promise<{ success: boolean; message?: string }> {
  try {
    // Yeni endpoint'i kullanarak randevu durumunu iptal edildi (2) olarak güncelle
    return await updateAppointmentStatus(appointmentId, RandevuDurum.IptalEdildi);
  } catch (error) {
    console.error('Randevu iptal edilirken hata:', error);
    return { 
      success: false, 
      message: error instanceof Error ? error.message : 'Randevu iptal edilirken bir hata oluştu' 
    };
  }
}

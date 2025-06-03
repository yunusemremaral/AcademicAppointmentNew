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
 * Akademisyenin randevularını getiren fonksiyon
 * @param academicId Akademisyen ID'si
 * @returns Randevu listesi
 */
export async function getAcademicAppointments(academicId: string): Promise<Randevu[]> {
  try {
    // API endpoint'ini hazırla
    const endpoint = API_ENDPOINTS.ADMIN.ACADEMIC_GET_APPOINTMENTS.replace('{academicId}', academicId);
    
    // API isteğini gönder
    const response = await fetchApi<Randevu[]>(endpoint);
    
    // Yanıtı döndür
    return response;
  } catch (error) {
    console.error('Akademisyen randevu bilgileri alınırken hata:', error);
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
    // Randevu durumunu iptal edildi (2) olarak güncelle
    return await updateAppointmentStatus(appointmentId, RandevuDurum.IptalEdildi);
  } catch (error) {
    console.error('Randevu iptal edilirken hata:', error);
    return { 
      success: false, 
      message: error instanceof Error ? error.message : 'Randevu iptal edilirken bir hata oluştu' 
    };
  }
}

/**
 * Randevu onaylama fonksiyonu
 * @param appointmentId Randevu ID'si
 * @returns İşlem sonucu
 */
export async function approveAppointment(appointmentId: number): Promise<{ success: boolean; message?: string }> {
  try {
    // Randevu durumunu onaylandı (1) olarak güncelle
    return await updateAppointmentStatus(appointmentId, RandevuDurum.Onaylandi);
  } catch (error) {
    console.error('Randevu onaylanırken hata:', error);
    return { 
      success: false, 
      message: error instanceof Error ? error.message : 'Randevu onaylanırken bir hata oluştu' 
    };
  }
}

/**
 * Randevu tamamlandı olarak işaretleme fonksiyonu
 * @param appointmentId Randevu ID'si
 * @returns İşlem sonucu
 */
export async function completeAppointment(appointmentId: number): Promise<{ success: boolean; message?: string }> {
  try {
    // Randevu durumunu tamamlandı (3) olarak güncelle
    return await updateAppointmentStatus(appointmentId, RandevuDurum.Tamamlandi);
  } catch (error) {
    console.error('Randevu tamamlanırken hata:', error);
    return { 
      success: false, 
      message: error instanceof Error ? error.message : 'Randevu tamamlanırken bir hata oluştu' 
    };
  }
}

import { fetchApi, API_ENDPOINTS } from "../../lib/api";

/* !!!  Düzgün çalışmıyor, token sorunları düzeltilecek. !!! */

/**
 * Şifre değiştirme isteği için arayüz
 */
interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
}

/**
 * Şifre değiştirme işlemi için API servis fonksiyonu
 * 
 * @param email - Kullanıcının e-posta adresi (değiştirilemez)
 * @param token - JWT token (değiştirilemez)
 * @param newPassword - Kullanıcının yeni şifresi
 * @returns API'den dönen cevap
 */
export const resetPassword = async (
  email: string,
  token: string,
  newPassword: string
): Promise<{ success: boolean; message: string }> => {
  try {
    const data: ResetPasswordRequest = {
      email,
      token,
      newPassword
    };

    const response = await fetchApi<{ success: boolean; message: string }>(
      API_ENDPOINTS.AUTH.RESET_PASSWORD,
      'POST',
      data,
      {
        Authorization: `Bearer ${token}`
      }
    );

    return response;
  } catch (error) {
    if (error instanceof Error) {
      return { success: false, message: error.message };
    }
    return { success: false, message: 'Bilinmeyen bir hata oluştu.' };
  }
};

export default {
  resetPassword
};

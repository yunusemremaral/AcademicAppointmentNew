import { getStudentsByDepartment, Student } from '../../lib/api';

/* !!!  Düzgün çalışmıyor, token sorunları düzeltilecek. !!! */

/**
 * Akademisyenin bölümüne ait öğrencileri getiren servis fonksiyonu
 * @param departmentId Akademisyenin bağlı olduğu bölüm ID'si
 * @returns Öğrenci listesi
 */
export async function getAcademicDepartmentStudents(departmentId: string): Promise<Student[]> {
  try {
    // API'den bölüme ait öğrencileri getir
    const students = await getStudentsByDepartment(departmentId);
    return students;
  } catch (error) {
    console.error('Öğrenci listesi alınırken hata oluştu:', error);
    throw error;
  }
}

/**
 * Öğrenci verilerini filtreleyerek sadece gerekli alanları döndüren yardımcı fonksiyon
 * @param students Öğrenci listesi
 * @returns Filtrelenmiş öğrenci listesi (id, userFullName, email)
 */
export function filterStudentData(students: Student[]): Pick<Student, 'id' | 'userFullName' | 'email'>[] {
  return students.map(({ id, userFullName, email }) => ({
    id,
    userFullName,
    email
  }));
}

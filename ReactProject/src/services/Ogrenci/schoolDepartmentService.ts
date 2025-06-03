import { getDepartmentsBySchool, getSchools } from '../../lib/api';
import { UserData } from '../../contexts/AuthContext';

/**
 * Bölüm bilgisi için arayüz
 */
export interface DepartmentInfo {
  id: string;
  name: string;
  url: string;
}

/**
 * Okul bilgisi için arayüz
 */
export interface SchoolInfo {
  id: string;
  name: string;
}

/**
 * Kullanıcının okul ve bölüm bilgileri için arayüz
 */
export interface UserSchoolDepartmentInfo {
  bolum: DepartmentInfo;
  okul: SchoolInfo;
}

/**
 * Öğrencinin departmentId'sine göre bölüm bilgilerini getiren fonksiyon
 * @param departmentId Bölüm ID'si (string)
 * @param schoolId Okul ID'si (string)
 * @returns Bölüm adı ve id'si
 */
export const getBolumBilgisi = async (departmentId: string, schoolId: string): Promise<DepartmentInfo> => {
  try {
    // Okul ID'sini number'a çevir
    const schoolIdNumber = parseInt(schoolId, 10);
    
    // Okul ID'si geçerli değilse varsayılan değerler döndür
    if (isNaN(schoolIdNumber)) {
      return {
        id: departmentId || "1",
        name: "Bilgisayar Mühendisliği",
        url: `/ogrenci/bolumler/${departmentId || "1"}`
      };
    }

    // Okula ait tüm bölümleri getir
    const bolumler = await getDepartmentsBySchool(schoolIdNumber);
    
    // Bölüm ID'sini number'a çevir
    const departmentIdNumber = parseInt(departmentId, 10);
    
    // Aranan bölümü bul
    const bolum = bolumler.find(b => b.id === departmentIdNumber);
    
    if (bolum) {
      return {
        id: bolum.id.toString(),
        name: bolum.name,
        url: `/ogrenci/bolumler/${bolum.id}`
      };
    }
    
    // Bölüm bulunamazsa varsayılan değerler döndür
    return {
      id: departmentId,
      name: "Bölüm",
      url: `/ogrenci/bolumler/${departmentId}`
    };
  } catch (error) {
    console.error(`Bölüm bilgisi alınırken hata: ${error}`);
    
    // Hata durumunda varsayılan değerler döndür
    return {
      id: departmentId || "1",
      name: "Bilgisayar Mühendisliği",
      url: `/ogrenci/bolumler/${departmentId || "1"}`
    };
  }
};

/**
 * Öğrencinin schoolId'sine göre okul bilgilerini getiren fonksiyon
 * @param schoolId Okul ID'si (string)
 * @returns Okul adı ve id'si
 */
export const getOkulBilgisi = async (schoolId: string): Promise<SchoolInfo> => {
  try {
    // Okul ID'sini number'a çevir
    const schoolIdNumber = parseInt(schoolId, 10);
    
    // Okul ID'si geçerli değilse varsayılan değerler döndür
    if (isNaN(schoolIdNumber)) {
      return {
        id: "1",
        name: "Bülent Ecevit Üniversitesi"
      };
    }

    // Tüm okulları getir
    const okullar = await getSchools();
    
    // Aranan okulu bul
    const okul = okullar.find(o => o.id === schoolIdNumber);
    
    if (okul) {
      return {
        id: okul.id.toString(),
        name: okul.name
      };
    }
    
    // Okul bulunamazsa varsayılan değerler döndür
    return {
      id: schoolId,
      name: "Üniversite"
    };
  } catch (error) {
    console.error(`Okul bilgisi alınırken hata: ${error}`);
    
    // Hata durumunda varsayılan değerler döndür
    return {
      id: schoolId || "1",
      name: "Bülent Ecevit Üniversitesi"
    };
  }
};

/**
 * Kullanıcı verisinden okul ve bölüm bilgilerini çeken yardımcı fonksiyon
 * @param user Kullanıcı verisi
 * @returns Okul ve bölüm bilgileri
 */
export const getUserOkulBolumBilgisi = async (user: UserData | null): Promise<UserSchoolDepartmentInfo> => {
  // Kullanıcı yoksa varsayılan değerler döndür
  if (!user) {
    return {
      bolum: {
        id: "1",
        name: "Bilgisayar Mühendisliği",
        url: "/ogrenci/bolumler/1"
      },
      okul: {
        id: "1",
        name: "Bülent Ecevit Üniversitesi"
      }
    };
  }
  
  try {
    // Bölüm ve okul bilgilerini paralel olarak getir
    const [bolumBilgisi, okulBilgisi] = await Promise.all([
      getBolumBilgisi(user.departmentId, user.schoolId),
      getOkulBilgisi(user.schoolId)
    ]);
    
    return {
      bolum: bolumBilgisi,
      okul: okulBilgisi
    };
  } catch (error) {
    console.error(`Kullanıcı okul-bölüm bilgisi alınırken hata: ${error}`);
    
    // Hata durumunda varsayılan değerler döndür
    return {
      bolum: {
        id: user.departmentId || "1",
        name: "Bilgisayar Mühendisliği",
        url: `/ogrenci/bolumler/${user.departmentId || "1"}`
      },
      okul: {
        id: user.schoolId || "1",
        name: "Bülent Ecevit Üniversitesi"
      }
    };
  }
};

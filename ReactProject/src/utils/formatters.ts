// Akademisyen isimlerini formatlama yardımcı fonksiyonları

/**
 * Akademisyen isimlerini unvan ve isim olarak parçalara ayırır.
 * @param fullName Akademisyen tam adı
 * @returns Unvan ve isim içeren bir nesne
 */
export const formatAkademisyenIsmi = (fullName: string) => {
  if (!fullName) return { unvan: "", isim: "İsim Bilgisi Yok" };
  
  // İsimleri parçalara ayıralım ve unvan/isim şeklinde formatlayalım
  const parts = fullName.split(" ");
  
  // Unvan kısmını belirleyelim (Dr. Öğr. Üyesi, Prof. Dr. vb.)
  let unvan = "";
  let isim = "";
  
  if (fullName.includes("Prof. Dr.")) {
    unvan = "Prof. Dr.";
    isim = parts.slice(2).join(" ");
  } else if (fullName.includes("Doç. Dr.")) {
    unvan = "Doç. Dr.";
    isim = parts.slice(2).join(" ");
  } else if (fullName.includes("Dr. Öğr. Üyesi")) {
    unvan = "Dr. Öğr. Üyesi";
    isim = parts.slice(3).join(" ");
  } else if (fullName.includes("Arş. Görevlisi")) {
    unvan = "Arş. Gör.";
    isim = parts.slice(2).join(" ");
  } else {
    // Varsayılan format
    unvan = parts[0];
    isim = parts.slice(1).join(" ");
  }
  
  return { unvan, isim };
};

/**
 * İsimden baş harfleri çıkarır.
 * @param fullName Tam isim
 * @returns İsmin baş harfleri (en fazla 2 harf)
 */
export const getInitials = (fullName: string) => {
  if (!fullName) return "??";
  
  // Akademisyen ismini formatla
  const { isim } = formatAkademisyenIsmi(fullName);
  
  // Boşluklarla böl ve her kelimenin baş harfini al
  const initials = isim
    .split(' ')
    .map(name => name.charAt(0))
    .join('')
    .toUpperCase();
  
  // En fazla 2 harf alınacak şekilde sınırla
  return initials.substring(0, 2);
}; 
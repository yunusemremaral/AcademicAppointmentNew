import React from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Calendar, Clock, CheckCircle, User, ArrowRight, Loader2 } from 'lucide-react';
import { format } from 'date-fns';
import { tr } from 'date-fns/locale';
import { Link } from 'react-router-dom';
import { themeColors } from '../../layouts/OgrenciLayout/OgrenciSidebar';

// Modal özellikleri için arayüz
interface RandevuModalProps {
  isOpen: boolean;
  onClose: () => void;
  academisyenName: string;
  randevuBilgi: {
    tarih: Date;
    konu: string;
    aciklama?: string;
  } | null;
  loading: boolean;
  success: boolean;
}

// RandevuModal bileşeni
const RandevuModal: React.FC<RandevuModalProps> = ({
  isOpen,
  onClose,
  academisyenName,
  randevuBilgi,
  loading,
  success
}) => {
  // Modal görünür değilse hiçbir şey render etme
  if (!isOpen) return null;

  return (
    <AnimatePresence>
      {isOpen && (
        // Modal arkaplanı - tüm ekranı kaplar
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          exit={{ opacity: 0 }}
          className="fixed inset-0 bg-black/40 backdrop-blur-sm z-50 flex items-center justify-center p-4"
          onClick={onClose}
        >
          {/* Modal içeriği */}
          <motion.div
            initial={{ scale: 0.9, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            exit={{ scale: 0.9, opacity: 0 }}
            transition={{ type: "spring", duration: 0.5 }}
            className="bg-white rounded-xl shadow-lg max-w-md w-full overflow-hidden"
            onClick={(e: React.MouseEvent) => e.stopPropagation()} // Event tipini belirttim
          >
            {/* Başlık çubuğu */}
            <div className={`${themeColors.primary.main} p-4 relative`}>
              <div className="absolute inset-0 bg-gradient-to-tr from-white/5 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-500"></div>
              <div className="relative text-white">
                <h3 className="font-medium text-base">
                  {loading ? "Randevu Oluşturuluyor" : "Randevu Oluşturuldu"}
                </h3>
                <p className="text-white/80 text-xs mt-1">
                  {loading ? "Lütfen bekleyin..." : "Randevunuz başarıyla kaydedildi"}
                </p>
              </div>
            </div>

            {/* İçerik kısmı - Duruma göre gösterilecek */}
            <div className="p-5">
              {/* Yükleniyor durumu */}
              {loading && (
                <div className="flex flex-col items-center justify-center py-8">
                  <motion.div 
                    animate={{ rotate: 360 }}
                    transition={{ repeat: Infinity, duration: 1, ease: "linear" }}
                    className="mb-4"
                  >
                    <Loader2 size={40} className="text-primary" />
                  </motion.div>
                  <p className="text-gray-700 text-sm font-medium">Randevunuz kaydediliyor</p>
                  <p className="text-gray-500 text-xs mt-1.5">Lütfen sayfadan ayrılmayınız</p>
                </div>
              )}

              {/* Başarılı durumu */}
              {success && randevuBilgi && (
                <div className="py-2">
                  {/* Başarı mesajı ve ikonu */}
                  <div className="flex items-center justify-center mb-4">
                    <div className="bg-green-50 p-3 rounded-full">
                      <CheckCircle size={32} className="text-green-500" />
                    </div>
                  </div>
                  
                  {/* Akademisyen bilgisi */}
                  <div className="mb-4 bg-gray-50 rounded-lg p-3.5 flex items-center">
                    <div className="bg-primary/10 p-2 rounded-full mr-3">
                      <User size={20} className="text-primary" />
                    </div>
                    <div>
                      <p className="text-xs text-gray-500">Akademisyen</p>
                      <p className="text-sm font-medium text-gray-800">{academisyenName}</p>
                    </div>
                  </div>
                  
                  {/* Randevu bilgileri */}
                  <div className="space-y-3">
                    {/* Tarih bilgisi */}
                    <div className="flex items-center">
                      <div className="bg-primary/10 p-1.5 rounded-md mr-3">
                        <Calendar size={16} className="text-primary" />
                      </div>
                      <div>
                        <p className="text-xs text-gray-500">Tarih</p>
                        <p className="text-sm font-medium text-gray-800">
                          {format(randevuBilgi.tarih, 'd MMMM yyyy', { locale: tr })}
                        </p>
                      </div>
                    </div>
                    
                    {/* Saat bilgisi */}
                    <div className="flex items-center">
                      <div className="bg-primary/10 p-1.5 rounded-md mr-3">
                        <Clock size={16} className="text-primary" />
                      </div>
                      <div>
                        <p className="text-xs text-gray-500">Saat</p>
                        <p className="text-sm font-medium text-gray-800">
                          {format(randevuBilgi.tarih, 'HH:mm', { locale: tr })}
                        </p>
                      </div>
                    </div>
                    
                    {/* Konu */}
                    <div className="bg-gray-50 p-3 rounded-lg">
                      <p className="text-xs text-gray-500 mb-1">Randevu Konusu</p>
                      <p className="text-sm text-gray-800">{randevuBilgi.konu}</p>
                    </div>
                    
                    {/* Açıklama - varsa göster */}
                    {randevuBilgi.aciklama && randevuBilgi.aciklama.trim() !== '' && randevuBilgi.aciklama.trim() !== ' ' && (
                      <div className="bg-gray-50 p-3 rounded-lg">
                        <p className="text-xs text-gray-500 mb-1">Açıklama</p>
                        <p className="text-sm text-gray-800">{randevuBilgi.aciklama}</p>
                      </div>
                    )}
                  </div>
                </div>
              )}
            </div>

            {/* Alt butonlar */}
            <div className="border-t border-gray-100 p-4">
              {loading ? (
                <p className="text-center text-xs text-gray-500">İşlem sürüyor...</p>
              ) : (
                <div className="flex justify-between items-center gap-3">
                  <Link
                    to="/ogrenci"
                    onClick={onClose}
                    className="px-4 py-2 border border-gray-200 rounded-lg text-sm text-gray-700 hover:bg-gray-50 transition-colors duration-200 flex-1 text-center"
                  >
                    Kapat
                  </Link>
                  <Link
                    to="/ogrenci/randevular"
                    className={`${themeColors.primary.main} px-4 py-2 rounded-lg text-sm text-white hover:shadow-md transition-all duration-300 flex items-center justify-center flex-1`}
                  >
                    Randevularım <ArrowRight size={14} className="ml-1.5" />
                  </Link>
                </div>
              )}
            </div>
          </motion.div>
        </motion.div>
      )}
    </AnimatePresence>
  );
};

export default RandevuModal;

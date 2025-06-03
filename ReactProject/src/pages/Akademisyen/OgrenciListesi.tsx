import React, { useState, useEffect } from 'react';
import { getAcademicDepartmentStudents, filterStudentData } from '../../services/Akademisyen/studentService';
import OgrenciKart from '../../components/Akademisyen/OgrenciListesi/OgrenciKart';
import { Student } from '../../lib/api';
import { useAuth } from '../../contexts/AuthContext';
import { Users, Search } from 'lucide-react';
import Loading from '../../components/Loading';

/**
 * Akademisyenin bölümüne ait öğrencileri listeleyen sayfa
 * AuthContext'ten akademisyenin departmentId'si otomatik olarak alınır
 */
const OgrenciListesi: React.FC = () => {
  // Auth context'ten kullanıcı bilgilerini al
  const { user, loading: authLoading } = useAuth();
  
  // Öğrenci listesi state'i
  const [students, setStudents] = useState<Pick<Student, 'id' | 'userFullName' | 'email'>[]>([]);
  // Filtrelenmiş öğrenci listesi
  const [filteredStudents, setFilteredStudents] = useState<Pick<Student, 'id' | 'userFullName' | 'email'>[]>([]);
  // Arama metni
  const [searchText, setSearchText] = useState<string>('');
  // Yükleniyor durumu
  const [loading, setLoading] = useState<boolean>(true);
  // Hata durumu
  const [error, setError] = useState<string | null>(null);

  // Öğrenci listesini getir
  useEffect(() => {
    // Auth yükleniyorsa veya kullanıcı yoksa bekle
    if (authLoading || !user) {
      return;
    }

    // Departman ID kontrolü
    if (!user.departmentId) {
      setError('Akademisyen departman bilgisi bulunamadı. Lütfen sistem yöneticisiyle iletişime geçin.');
      setLoading(false);
      return;
    }

    const fetchStudents = async () => {
      try {
        setLoading(true);
        // Akademisyenin bölümüne ait öğrencileri getir
        const data = await getAcademicDepartmentStudents(user.departmentId);
        // Öğrenci verilerini filtrele
        const filteredData = filterStudentData(data);
        setStudents(filteredData);
        setFilteredStudents(filteredData);
        setError(null);
      } catch (err) {
        console.error('Öğrenci listesi yüklenirken hata oluştu:', err);
        setError('Öğrenci listesi yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.');
      } finally {
        setLoading(false);
      }
    };

    fetchStudents();
  }, [user, authLoading]);

  // Arama fonksiyonu - Sadece isimle arama
  useEffect(() => {
    if (searchText.trim() === '') {
      setFilteredStudents(students);
      return;
    }

    const searchLower = searchText.toLowerCase();
    const filtered = students.filter(student => 
      student.userFullName.toLowerCase().includes(searchLower)
    );
    setFilteredStudents(filtered);
  }, [searchText, students]);

  // Auth yükleniyorsa bekletme ekranı göster
  if (authLoading) {
    return <Loading />;
  }

  // Kullanıcı giriş yapmamışsa veya akademisyen değilse hata göster
  if (!user || user.roles !== 'Instructor') {
    return (
      <div className="container mx-auto py-6 px-4">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
          <span className="block sm:inline">Bu sayfayı görüntüleme yetkiniz bulunmamaktadır.</span>
        </div>
      </div>
    );
  }

  return (
    <div className="p-2 sm:p-3 md:p-5 max-w-full font-poppins">
      {/* Sayfa başlığı ve arama çubuğu */}
      <div className={`p-3 sm:p-4 bg-primary rounded-xl shadow-md mb-4 sm:mb-5 backdrop-blur-sm bg-opacity-95`}>
        <div className="flex flex-col md:flex-row items-start md:items-center justify-between">
          <div className="flex items-center mb-3 md:mb-0">
            <Users size={16} className="text-white mr-2.5" />
            <h1 className="text-base sm:text-lg font-semibold text-white">Öğrenci Listesi</h1>
          </div>
          
          {/* Arama çubuğu */}
          <div className="relative w-full md:w-64 lg:w-80">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <Search size={16} className="text-white/70" />
            </div>
            <input
              type="text"
              placeholder="Öğrenci ara..."
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              className="w-full pl-10 pr-4 py-2 bg-white/20 border border-white/30 rounded-lg text-white placeholder-white/70 focus:outline-none focus:ring-2 focus:ring-white/50 text-sm"
            />
          </div>
        </div>
        <p className="text-white/90 text-xs mt-1.5 max-w-2xl">
          Bölümünüzdeki öğrenci listesini görüntüleyin ve arayın.
        </p>
      </div>

      {/* Yükleniyor durumu */}
      {loading && <Loading />}

      {/* Hata durumu */}
      {!loading && error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-6" role="alert">
          <span className="block sm:inline">{error}</span>
        </div>
      )}

      {/* Öğrenci listesi */}
      {!loading && !error && (
        <>
          {filteredStudents.length === 0 ? (
            <div className="text-center py-8 bg-white rounded-lg shadow-sm border border-gray-200 p-8">
              <Users size={40} className="mx-auto text-gray-400 mb-3" />
              <p className="text-gray-600 font-medium">Öğrenci bulunamadı</p>
              {searchText && (
                <p className="text-gray-500 text-sm mt-1">
                  "{searchText}" aramasına uygun öğrenci kaydı bulunmamaktadır.
                </p>
              )}
              {!searchText && (
                <p className="text-gray-500 text-sm mt-1">
                  Bu bölüme ait kayıtlı öğrenci bulunmamaktadır.
                </p>
              )}
            </div>
          ) : (
            <>
              <div className="mb-4 text-sm text-gray-600">
                {searchText ? (
                  <p className="px-1">
                    "{searchText}" için {filteredStudents.length} öğrenci bulundu
                  </p>
                ) : (
                  <p className="px-1">Toplam {filteredStudents.length} öğrenci</p>
                )}
              </div>
              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                {filteredStudents.map((student) => (
                  <OgrenciKart key={student.id} student={student} />
                ))}
              </div>
            </>
          )}
        </>
      )}
    </div>
  );
};

export default OgrenciListesi;

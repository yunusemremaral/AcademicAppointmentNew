import { Outlet } from 'react-router-dom';

interface OgrenciMainProps {
  isSearching: boolean;
}

const OgrenciMain: React.FC<OgrenciMainProps> = ({ isSearching }) => {
  return (
    <div className="flex-1 overflow-auto pb-6">
      {isSearching ? (
        // Arama sonuçları gösterilecek
        <div className="bg-white rounded-lg shadow-sm p-4">
          <div className="flex items-center justify-between mb-4">
            <h2 className="font-medium text-gray-700">Arama sonuçları</h2>
          </div>
          
          {/* Arama sonuçları buraya eklenecek (API entegrasyonu ile) */}
          <div className="text-center text-gray-500 py-8">
            <p>Henüz bir sonuç bulunamadı.</p>
            <p className="text-xs mt-2">API entegrasyonu tamamlandıktan sonra bu bölümde sonuçlar listelenecek.</p>
          </div>
        </div>
      ) : (
        // Normal içerik gösterilecek - Outlet ile alt rotaları render et
        <div className="rounded-lg shadow-sm">
          {/* Ana içerik container */}
          <div className="w-full sm:mt-0">
            {/* Burada Outlet ile alt sayfalar render edilecek */}
            <Outlet />
          </div>
        </div>
      )}
    </div>
  );
};

export default OgrenciMain;

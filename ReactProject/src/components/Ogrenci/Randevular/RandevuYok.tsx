import { memo } from 'react';
import { Calendar } from 'lucide-react';

const EmptyState = memo(() => (
  <div className="text-center py-12 sm:py-16 bg-white rounded-xl border border-gray-100 shadow-sm">
    <div className="relative mx-auto mb-4 w-16 h-16 rounded-full bg-gray-50 flex items-center justify-center">
      <Calendar size={32} className="text-gray-300" />
    </div>
    
    <h3 className="text-base font-medium text-gray-700 mb-2">
      Henüz randevunuz bulunmuyor
    </h3>
    
    <p className="text-gray-500 text-xs max-w-md mx-auto px-6">
      Akademisyen profillerinden randevu oluşturabilirsiniz.
    </p>
  </div>
));

EmptyState.displayName = 'EmptyState';

export default EmptyState; 
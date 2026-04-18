import { useCompareStore } from '../../store/useCompareStore';
import { X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';
import { useNavigate } from 'react-router-dom';

interface CompareModalProps {
  onClose: () => void;
}

const CompareModal = ({ onClose }: CompareModalProps) => {
  const { items, removeItem, clearItems } = useCompareStore();
  const navigate = useNavigate();

  useModalScrollLock(true);

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-md shadow-xl flex flex-col max-h-[80vh]">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Порівняння</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        {items.length === 0 ? (
          <p className="text-sm text-gray-400 text-center p-6">Список порівняння порожній</p>
        ) : (
          <>
            <div className="overflow-y-auto flex-1">
              {items.map(item => (
                <div key={item.id} className="flex items-center gap-3 p-4 border-b border-gray-100">
                  <img src={item.imageUrl} alt={item.name} className="w-14 h-14 object-contain rounded" />
                  <div className="flex-1 min-w-0">
                    <p className="text-sm font-medium line-clamp-2">{item.name}</p>
                    <p className="text-sm text-green-700 font-bold">{item.price} ₴</p>
                  </div>
                  <button onClick={() => removeItem(item.id)} className="text-gray-300 hover:text-red-400 transition-colors">
                    <X size={14} />
                  </button>
                </div>
              ))}
            </div>
            <div className="p-4 border-t border-gray-100 flex flex-col gap-3">
              <button
                onClick={clearItems}
                className="w-full border border-gray-300 text-gray-500 py-2 rounded text-sm hover:border-red-400 hover:text-red-400 transition-colors"
              >
                Очистити список
              </button>
              <button
                onClick={() => { navigate('/compare'); onClose(); }}
                className="w-full bg-orange-500 text-white py-2 rounded text-sm hover:bg-orange-400 transition-colors font-medium"
              >
                Перейти до порівняння
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
}

export default CompareModal;
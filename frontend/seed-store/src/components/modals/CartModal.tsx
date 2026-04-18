import { useCartStore } from '../../store/useCartStore';
import { Plus, Minus, X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';
import { useNavigate } from 'react-router-dom';

interface CartModalProps {
  onClose: () => void;
}

const CartModal = ({ onClose }: CartModalProps) => {
  const { items, incrementQuantity, decrementQuantity, removeItem, clearItems } = useCartStore();
  const navigate = useNavigate();
  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);
  const totalCount = items.reduce((sum, item) => sum + item.quantity, 0);

  useModalScrollLock(true);

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-md shadow-xl flex flex-col max-h-[80vh]">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Кошик</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        {items.length === 0 ? (
          <p className="text-sm text-gray-400 text-center p-6">Кошик порожній</p>
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
                  <div className="flex flex-col items-end gap-2">
                    <button onClick={() => removeItem(item.id)} className="text-gray-300 hover:text-red-400 transition-colors">
                      <X size={14} />
                    </button>
                    <div className="flex items-center gap-1">
                      <button onClick={() => decrementQuantity(item.id)} className="border border-gray-300 rounded p-0.5 hover:border-green-700 hover:text-green-700 transition-colors">
                        <Minus size={12} />
                      </button>
                      <span className="text-sm w-6 text-center">{item.quantity}</span>
                      <button onClick={() => incrementQuantity(item.id)} className="border border-gray-300 rounded p-0.5 hover:border-green-700 hover:text-green-700 transition-colors">
                        <Plus size={12} />
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
            <div className="p-4 flex flex-col gap-3 border-t border-gray-100">
              <p className="text-sm text-gray-600">
                Всього <span className="font-semibold">{totalCount} товарів</span> на суму: <span className="font-bold text-green-700">{total} ₴</span>
              </p>
              <button
                onClick={clearItems}
                className="w-full border border-gray-300 text-gray-500 py-2 rounded text-sm hover:border-red-400 hover:text-red-400 transition-colors"
              >
                Очистити кошик
              </button>
              <button
                onClick={() => { onClose(); navigate('/checkout'); }}
                className="w-full bg-green-600 text-white py-2 rounded text-sm hover:bg-green-700 transition-colors font-medium"
              >
                Оформити замовлення
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
}

export default CartModal;
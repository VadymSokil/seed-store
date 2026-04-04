import { X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';

interface CabinetModalProps {
  onClose: () => void;
}

const CabinetModal = ({ onClose }: CabinetModalProps) => {
  useModalScrollLock(true);

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-sm shadow-xl">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Вхід до кабінету</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        <div className="p-4 flex flex-col gap-3">
          <button className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium">
            Увійти
          </button>
          <button className="w-full border border-green-600 text-green-600 py-2 rounded hover:bg-green-50 transition-colors font-medium">
            Зареєструватись
          </button>
        </div>
      </div>
    </div>
  );
}

export default CabinetModal;
import { useState } from 'react';
import { X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';

interface DeleteAccountModalProps {
  onConfirm: () => Promise<void>;
  onClose: () => void;
}

export default function DeleteAccountModal({ onConfirm, onClose }: DeleteAccountModalProps) {
  const [isLoading, setIsLoading] = useState(false);

  useModalScrollLock(true);

  const handleConfirm = async () => {
    setIsLoading(true);
    try {
      await onConfirm();
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-sm shadow-xl">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Видалення акаунту</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        <div className="p-4 flex flex-col gap-4">
          <p className="text-sm text-gray-600">Ви впевнені що хочете видалити акаунт?</p>
          <div className="flex gap-2">
            <button
              onClick={handleConfirm}
              disabled={isLoading}
              className="flex-1 py-2 rounded bg-red-500 text-white text-sm font-medium hover:bg-red-600 transition-colors disabled:opacity-60 disabled:cursor-not-allowed"
            >
              {isLoading ? 'Завантаження...' : 'Так'}
            </button>
            <button
              onClick={onClose}
              disabled={isLoading}
              className="flex-1 py-2 rounded border border-gray-300 text-gray-600 text-sm font-medium hover:border-gray-400 transition-colors disabled:opacity-60 disabled:cursor-not-allowed"
            >
              Ні
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
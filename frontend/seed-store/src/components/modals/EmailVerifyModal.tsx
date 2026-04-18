import { useState } from 'react';
import { X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';
import { confirmEmailChange, resendEmailChangeCode } from '../../api/accountApi';
import { useAuthStore } from '../../store/useAuthStore';
import CodeInput from '../ui/CodeInput';
import { useCountdown } from '../../hooks/useCountdown';

interface EmailVerifyModalProps {
  newEmail: string;
  onClose: () => void;
}

export default function EmailVerifyModal({ newEmail, onClose }: EmailVerifyModalProps) {
  const setAccount = useAuthStore(s => s.setAccount);
  const account = useAuthStore(s => s.account);

  const [code, setCode] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const { formatted: timer, expired } = useCountdown(900, true);

  useModalScrollLock(true);

  const handleConfirm = async () => {
    setError('');
    if (!code) { setError('Введіть код підтвердження'); return; }
    setIsLoading(true);
    try {
      await confirmEmailChange({ newEmail, code });
      setAccount({ ...account!, email: newEmail });
      onClose();
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_code') setError('Невірний код');
      else if (status === 'code_expired') setError('Код застарів. Запросіть новий');
      else setError('Помилка. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const handleResend = async () => {
    setError('');
    setCode('');
    setIsLoading(true);
    try {
      await resendEmailChangeCode();
    } catch {
      setError('Не вдалося надіслати код. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-sm shadow-xl">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Підтвердження пошти</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        <div className="p-4 flex flex-col gap-3">
          <p className="text-sm text-gray-500">
            На адресу <span className="font-medium text-gray-700">{newEmail}</span> надіслано код підтвердження.
          </p>
          <p className={`text-xs ${expired ? 'text-red-500' : 'text-gray-400'}`}>
            Код дійсний: {timer}
          </p>
          <CodeInput value={code} onChange={setCode} />
          {error && <p className="text-xs text-red-500">{error}</p>}
          <button
            onClick={handleConfirm}
            disabled={isLoading}
            className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed"
          >
            {isLoading ? 'Завантаження...' : 'Підтвердити'}
          </button>
          <button
            onClick={handleResend}
            disabled={isLoading}
            className="text-sm text-gray-400 hover:text-green-600 transition-colors text-center disabled:opacity-60 disabled:cursor-not-allowed"
          >
            Надіслати код повторно
          </button>
        </div>
      </div>
    </div>
  );
}
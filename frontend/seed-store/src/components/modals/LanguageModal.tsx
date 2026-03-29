import { X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';

interface LanguageModalProps {
  onClose: () => void;
  language: 'UA' | 'EN';
  onLanguageChange: (lang: 'UA' | 'EN') => void;
}

const languages = [
  { code: 'UA' as const, name: 'Українська', flag: 'ua' },
  { code: 'EN' as const, name: 'English', flag: 'gb' },
];

const LanguageModal = ({ onClose, language, onLanguageChange }: LanguageModalProps) => {
  useModalScrollLock();

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-xs shadow-xl">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Мова</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        <div className="p-2">
          {languages.map((lang) => (
            <button
              key={lang.code}
              onClick={() => { onLanguageChange(lang.code); onClose(); }}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded text-sm transition-colors ${
                language === lang.code
                  ? 'bg-green-50 text-green-700 font-medium'
                  : 'text-gray-700 hover:bg-gray-50'
              }`}
            >
              <span className={`fi fi-${lang.flag} text-xl rounded-sm`}></span>
              <span>{lang.name}</span>
              {language === lang.code && (
                <span className="ml-auto text-green-600">✓</span>
              )}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}

export default LanguageModal;
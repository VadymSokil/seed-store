import { useState, useRef, useEffect } from 'react';

interface AutocompleteInputProps<T> {
  value: string;
  onChange: (value: string) => void;
  onSelect: (item: T) => void;
  options: T[];
  getLabel: (item: T) => string;
  placeholder?: string;
  disabled?: boolean;
  isLoading?: boolean;
}

export default function AutocompleteInput<T>({
  value,
  onChange,
  onSelect,
  options,
  getLabel,
  placeholder,
  disabled,
  isLoading,
}: AutocompleteInputProps<T>) {
  const [isOpen, setIsOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(e.target as Node)) {
        setIsOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onChange(e.target.value);
    setIsOpen(true);
  };

  const handleSelect = (item: T) => {
    onSelect(item);
    setIsOpen(false);
  };

  const inputClass = 'w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 disabled:bg-gray-50 disabled:text-gray-400';

  return (
    <div ref={containerRef} className="relative">
      <input
        value={value}
        onChange={handleChange}
        onFocus={() => options.length > 0 && setIsOpen(true)}
        placeholder={placeholder}
        disabled={disabled}
        className={inputClass}
      />
      {isLoading && (
        <div className="absolute right-3 top-1/2 -translate-y-1/2">
          <div className="w-4 h-4 border-2 border-gray-300 border-t-green-600 rounded-full animate-spin" />
        </div>
      )}
      {isOpen && options.length > 0 && (
        <ul className="absolute z-50 w-full bg-white border border-gray-200 rounded shadow-md mt-1 max-h-60 overflow-y-auto">
          {options.map((item, index) => (
            <li
              key={index}
              onMouseDown={() => handleSelect(item)}
              className="px-3 py-2 text-sm text-gray-700 hover:bg-green-50 cursor-pointer"
            >
              {getLabel(item)}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
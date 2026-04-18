import { useRef, useState, useEffect } from 'react';
import { createPortal } from 'react-dom';
import { Search, X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { searchProducts, searchCategories } from '../../api/searchApi';
import type { SearchProduct, SearchCategory } from '../../api/searchApi';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';

interface SearchModalProps {
  onClose: () => void;
  anchorRef?: React.RefObject<HTMLDivElement | null>;
}

const SearchModal = ({ onClose, anchorRef }: SearchModalProps) => {
  const navigate = useNavigate();
  const [query, setQuery] = useState('');
  const [products, setProducts] = useState<SearchProduct[]>([]);
  const [categories, setCategories] = useState<SearchCategory[]>([]);
  const [loading, setLoading] = useState(false);
  const [position, setPosition] = useState<{ top: number; left: number; width: number; height: number } | null>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  const scrollRef = useRef<HTMLDivElement>(null);
  useModalScrollLock(true);

  const isDesktop = !!position;

  useEffect(() => {
    if (anchorRef?.current) {
      const rect = anchorRef.current.getBoundingClientRect();
      setPosition({ top: rect.top, left: rect.left, width: rect.width, height: rect.height });
    }
  }, []);

  useEffect(() => {
    if (position) inputRef.current?.focus();
  }, [position]);

  useEffect(() => {
    if (!isDesktop) inputRef.current?.focus();
  }, []);

  useEffect(() => {
    if (!query) { setProducts([]); setCategories([]); return; }
    const timer = setTimeout(() => {
      setLoading(true);
      Promise.all([searchProducts(query), searchCategories(query)])
        .then(([prods, cats]) => { setProducts(prods); setCategories(cats); })
        .finally(() => setLoading(false));
    }, 300);
    return () => clearTimeout(timer);
  }, [query]);

  const total = products.length + categories.length;

  const handleCategoryClick = (slug: string) => {
    navigate(`/category/${slug}`);
    onClose();
  };

  const handleProductClick = (slug: string) => {
    navigate(`/product/${slug}`);
    onClose();
  };

  const handleShowAll = () => {
    navigate(`/search?q=${encodeURIComponent(query)}`);
    onClose();
  };

  const resultsList = (
    <div ref={scrollRef} className={`overflow-y-auto ${isDesktop ? 'max-h-72' : 'flex-1'}`}>
      {categories.map((cat, i) => (
        <div key={i} onClick={() => handleCategoryClick(cat.path)} className="px-4 py-3 hover:bg-gray-50 cursor-pointer text-sm text-gray-700 border-b border-gray-100">
          {cat.name}
        </div>
      ))}
      {products.map((product, i) => (
        <div key={i} onClick={() => handleProductClick(product.slug)} className="flex items-center gap-3 px-4 py-3 hover:bg-gray-50 cursor-pointer border-b border-gray-100">
          <img src={product.imageUrl} alt={product.name} className="w-12 h-12 object-cover rounded" />
          <div className="flex-1 min-w-0">
            <p className="text-sm font-medium line-clamp-1">{product.name}</p>
            <p className="text-sm text-green-700 font-bold">{product.price} ₴</p>
          </div>
        </div>
      ))}
    </div>
  );

  const results = (
    <>
      {query && (
        <div className="px-4 py-2 border-b border-gray-100">
          <p className="text-xs text-gray-400">
            {loading ? 'Пошук...' : `Знайдено результатів: ${total}`}
          </p>
        </div>
      )}
      {!loading && query && resultsList}
      {query && !loading && (
        <div className="p-3 border-t border-gray-100">
          <button
            onClick={handleShowAll}
            className="w-full bg-green-600 text-white py-2 rounded text-sm hover:bg-green-700 transition-colors"
          >
            Показати всі результати
          </button>
        </div>
      )}
    </>
  );

  if (anchorRef && !position) return null;

  return createPortal(
    isDesktop ? (
      <>
        <div className="fixed inset-0 z-40 bg-black/40" onClick={onClose} />
        <div
          className="fixed z-50 bg-white border border-green-600 rounded overflow-hidden flex items-stretch"
          style={{ top: position!.top, left: position!.left, width: position!.width, height: position!.height }}
        >
          <input
            ref={inputRef}
            type="text"
            placeholder="Пошук товарів..."
            className="flex-1 px-3 py-2 outline-none text-base"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            onKeyDown={(e) => { if (e.key === 'Enter' && query) handleShowAll(); }}
          />
          <div className="border-l px-3 flex items-center">
            <Search size={20} strokeWidth={2.5} className="text-gray-400" />
          </div>
        </div>
        {query && (
          <div
            data-search-modal
            className="fixed bg-white border border-gray-200 rounded-b-lg shadow-lg z-50 flex flex-col"
            style={{ top: position!.top + position!.height - 2, left: position!.left, width: position!.width, maxHeight: '400px' }}
          >
            {results}
          </div>
        )}
      </>
    ) : (
      <div className="fixed inset-0 z-50 flex flex-col bg-white">
        <div className="flex items-center gap-3 p-4 border-b border-gray-200">
          <div className="flex-1 flex items-stretch border rounded overflow-hidden focus-within:border-green-600">
            <input
              ref={inputRef}
              type="text"
              placeholder="Пошук товарів..."
              className="flex-1 px-3 py-2 outline-none"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              onKeyDown={(e) => { if (e.key === 'Enter' && query) handleShowAll(); }}
            />
          </div>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={24} />
          </button>
        </div>
        {results}
      </div>
    ),
    document.body
  );
}

export default SearchModal;
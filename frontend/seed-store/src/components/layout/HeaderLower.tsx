import { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Menu, Tag, Scale, Heart, ShoppingCart, User, Search } from 'lucide-react';
import CartModal from '../modals/CartModal';
import CabinetModal from '../modals/CabinetModal';
import CompareModal from '../modals/CompareModal';
import SearchModal from '../modals/SearchModal';
import { useCartStore } from '../../store/useCartStore';
import { useFavoritesStore } from '../../store/useFavoritesStore';
import { useCompareStore } from '../../store/useCompareStore';
import CounterBadge from '../ui/CounterBadge';
import { useAuthStore } from '../../store/useAuthStore';

const HeaderLower = () => {
  const [isCabinetOpen, setIsCabinetOpen] = useState(false);
  const [isCartOpen, setIsCartOpen] = useState(false);
  const [isCompareOpen, setIsCompareOpen] = useState(false);
  const [isSearchModalOpen, setIsSearchModalOpen] = useState(false);
  const [isDesktopSearchOpen, setIsDesktopSearchOpen] = useState(false);

  const navigate = useNavigate();

  const cartCount = useCartStore((state) => state.count);
  const favoritesCount = useFavoritesStore((state) => state.count);
  const compareCount = useCompareStore((state) => state.count);

  const desktopSearchRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      const target = e.target as HTMLElement;
      if (
        desktopSearchRef.current && !desktopSearchRef.current.contains(target) &&
        !target.closest('[data-search-modal]')
      ) {
        setIsDesktopSearchOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const account = useAuthStore(s => s.account);

const handleCabinetClick = () => {
  if (account) {
    navigate('/cabinet');
  } else {
    setIsCabinetOpen(true);
  }
};

  return (
    <div className="bg-white py-3">
      {isCartOpen && <CartModal onClose={() => setIsCartOpen(false)} />}
      {isCabinetOpen && <CabinetModal onClose={() => setIsCabinetOpen(false)} />}
      {isCompareOpen && <CompareModal onClose={() => setIsCompareOpen(false)} />}
      {isSearchModalOpen && <SearchModal onClose={() => setIsSearchModalOpen(false)} />}
      {isDesktopSearchOpen && <SearchModal onClose={() => setIsDesktopSearchOpen(false)} anchorRef={desktopSearchRef} />}

      {/* Мобільна версія */}
      <div className="md:hidden px-4">
        <div className="grid grid-cols-7 gap-1">
          <button
            onClick={() => navigate('/catalog')}
            className="relative flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors"
          >
            <Menu size={20} strokeWidth={2.5} />
          </button>
          <button onClick={() => navigate('/discounts')} className="relative flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors">
            <Tag size={20} strokeWidth={2.5} />
          </button>
          <button
            onClick={() => setIsCompareOpen(!isCompareOpen)}
            className="relative flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors"
          >
            <Scale size={20} strokeWidth={2.5} />
            <CounterBadge count={compareCount} />
          </button>
          <button
            onClick={() => navigate('/favorites')}
            className="relative flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors"
          >
            <Heart size={20} strokeWidth={2.5} />
            <CounterBadge count={favoritesCount} />
          </button>
          <button
            onClick={() => setIsCartOpen(!isCartOpen)}
            className="relative flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors"
          >
            <ShoppingCart size={20} strokeWidth={2.5} />
            <CounterBadge count={cartCount} />
          </button>
          <button
            onClick={handleCabinetClick}
            className="flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors"
          >
            <User size={20} strokeWidth={2.5} />
          </button>
          <button
            onClick={() => setIsSearchModalOpen(true)}
            className="flex justify-center items-center aspect-square border border-gray-300 text-gray-400 rounded hover:border-green-700 hover:text-green-700 transition-colors"
          >
            <Search size={20} strokeWidth={2.5} />
          </button>
        </div>
      </div>

      {/* Десктопна версія */}
      <div className="hidden md:flex px-4 md:px-8 xl:px-8 2xl:px-32 items-center gap-3">
        <button
          onClick={() => navigate('/catalog')}
          className="bg-green-600 text-white px-4 py-2 rounded flex items-center gap-2 hover:bg-green-700 active:bg-green-700 transition-colors"
        >
          <Menu size={20} strokeWidth={2.5} />
          <span className="font-medium">Каталог</span>
        </button>
        <div className="relative flex-1" ref={desktopSearchRef}>
          <div
            className="flex items-stretch border rounded overflow-hidden focus-within:border-green-600 cursor-text"
            onClick={() => setIsDesktopSearchOpen(true)}
          >
            <input
              type="text"
              placeholder="Пошук товарів..."
              className="flex-1 px-3 py-2 outline-none cursor-text"
              readOnly
            />
            <div className="border-l px-3 flex items-center">
              <Search size={20} strokeWidth={2.5} className="text-gray-400" />
            </div>
          </div>
        </div>
        <button onClick={() => navigate('/discounts')} className="border border-gray-300 text-gray-400 px-[9px] py-[9px] rounded hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors">
          <Tag size={20} strokeWidth={2.5} />
        </button>
        <button
          onClick={() => setIsCompareOpen(!isCompareOpen)}
          className="relative border border-gray-300 text-gray-400 px-[9px] py-[9px] rounded hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors"
        >
          <Scale size={20} strokeWidth={2.5} />
          <CounterBadge count={compareCount} />
        </button>
        <button
          onClick={() => navigate('/favorites')}
          className="relative border border-gray-300 text-gray-400 px-[9px] py-[9px] rounded hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors"
        >
          <Heart size={20} strokeWidth={2.5} />
          <CounterBadge count={favoritesCount} />
        </button>
        <button
          onClick={() => setIsCartOpen(!isCartOpen)}
          className="relative border border-gray-300 text-gray-400 px-[9px] py-[9px] rounded hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors"
        >
          <ShoppingCart size={20} strokeWidth={2.5} />
          <CounterBadge count={cartCount} />
        </button>
        <button
          onClick={handleCabinetClick}
          className="border border-gray-300 text-gray-400 px-[9px] py-[9px] rounded hover:border-green-700 hover:text-green-700 active:border-green-700 active:text-green-700 transition-colors"
        >
          <User size={20} strokeWidth={2.5} />
        </button>
      </div>
    </div>
  );
}

export default HeaderLower;
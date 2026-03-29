import { Heart, Star } from 'lucide-react';
import { useCartStore } from '../../store/useCartStore';
import { useFavoritesStore } from '../../store/useFavoritesStore';
import { useCompareStore } from '../../store/useCompareStore';

interface ProductCardProps {
  id: number;
  name: string;
  imageUrl: string;
  price: number;
  oldPrice?: number;
  rating: number;
  reviewsCount: number;
  inStock: boolean;
}

const ProductCard = ({ id, name, imageUrl, price, oldPrice, rating, reviewsCount, inStock }: ProductCardProps) => {
  const isInCart = useCartStore((state) => state.items.some(i => i.id === id));
  const isFavorite = useFavoritesStore((state) => state.items.some(i => i.id === id));
  const isCompared = useCompareStore((state) => state.items.some(i => i.id === id));

  const addToCart = useCartStore((state) => state.addItem);
  const removeFromCart = useCartStore((state) => state.removeItem);
  const addToFavorites = useFavoritesStore((state) => state.addItem);
  const removeFromFavorites = useFavoritesStore((state) => state.removeItem);
  const addToCompare = useCompareStore((state) => state.addItem);
  const removeFromCompare = useCompareStore((state) => state.removeItem);

  return (
    <div className="bg-white rounded-lg border border-gray-200 overflow-hidden hover:shadow-md hover:-translate-y-1 transition-all duration-200">
      <div className="relative">
        <img src={imageUrl} alt={name} className="w-full h-48 object-cover scale-110 transition-transform duration-300" />
        <div className="absolute top-2 right-2 flex flex-col gap-2">
          <button
            onClick={() => isFavorite ? removeFromFavorites(id) : addToFavorites({ id, name, imageUrl, price, inStock })}
            className="bg-white p-1.5 rounded-full shadow transition-colors"
          >
            <Heart
              size={18}
              className={`transition-colors ${
                isFavorite
                  ? 'fill-red-700 text-red-700 hover:fill-red-500 hover:text-red-500'
                  : 'text-gray-600 hover:fill-red-300 hover:text-red-300'
              }`}
            />
          </button>
        </div>
      </div>

      <div className="p-3 flex flex-col gap-2">
        <p className="text-sm font-medium line-clamp-2 min-h-10">{name}</p>

        <div className="flex items-center gap-1">
          <div className="flex">
            {[1, 2, 3, 4, 5].map((star) => (
              <div key={star} className="relative">
                <Star size={14} className="text-gray-300 fill-gray-300" />
                <div
                  className="absolute inset-0 overflow-hidden"
                  style={{ width: `${Math.min(Math.max(rating - star + 1, 0), 1) * 100}%` }}
                >
                  <Star size={14} className="text-yellow-400 fill-yellow-400" />
                </div>
              </div>
            ))}
          </div>
          <span className="text-xs text-gray-600">{rating.toFixed(1)}</span>
          <span className="text-xs text-gray-400">({reviewsCount} відгуків)</span>
        </div>

        <span className={`text-xs ${inStock ? 'text-green-600' : 'text-red-500'}`}>
          {inStock ? 'В наявності' : 'Немає в наявності'}
        </span>

        <div className="flex items-center gap-2">
          <span className="text-base font-bold text-green-700">{price} ₴</span>
          {oldPrice && <span className="text-sm text-gray-400 line-through">{oldPrice} ₴</span>}
        </div>

        <button
          onClick={() => isCompared ? removeFromCompare(id) : addToCompare({ id, name, imageUrl, price })}
          className={`w-full py-1.5 rounded text-sm transition-colors border ${
            isCompared
              ? 'bg-orange-500 text-white border-orange-500 hover:bg-orange-400'
              : 'bg-white text-gray-600 border-gray-300 hover:bg-orange-50 hover:text-orange-500 hover:border-orange-300'
          }`}
        >
          {isCompared ? 'В порівнянні' : 'Порівняти'}
        </button>
        <button
          onClick={() => isInCart ? removeFromCart(id) : addToCart({ id, name, imageUrl, price, quantity: 1 })}
          className={`w-full text-white py-1.5 rounded text-sm transition-colors ${isInCart ? 'bg-green-800 hover:bg-green-700' : 'bg-green-600 hover:bg-green-700'}`}
        >
          {isInCart ? 'В кошику' : 'В кошик'}
        </button>
      </div>
    </div>
  );
}

export default ProductCard;
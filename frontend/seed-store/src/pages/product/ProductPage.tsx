import { useEffect, useState, useMemo } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Star, Heart, Scale, ShoppingCart } from 'lucide-react';
import Layout from '../../components/layout/Layout';
import { getProduct } from '../../api/productsApi';
import type { ProductDetails } from '../../api/productsApi';
import { useCartStore } from '../../store/useCartStore';
import { useFavoritesStore } from '../../store/useFavoritesStore';
import { useCompareStore } from '../../store/useCompareStore';
import { getCategories } from '../../api/catalogApi';
import type { Category } from '../../types/catalog';
import { getDeliveryVariants, getPaymentVariants } from '../../api/storeInfoApi';
import type { StoreInfoVariant } from '../../api/storeInfoApi';

export default function ProductPage() {
  const { slug } = useParams<{ slug: string }>();
  const navigate = useNavigate();

  const [product, setProduct] = useState<ProductDetails | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState(0);
  const [lightboxOpen, setLightboxOpen] = useState(false);
  const [deliveryVariants, setDeliveryVariants] = useState<StoreInfoVariant[]>([]);
  const [paymentVariants, setPaymentVariants] = useState<StoreInfoVariant[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);

  const isInCart = useCartStore(s => s.items.some(i => i.id === product?.id));
  const isFavorite = useFavoritesStore(s => s.items.some(i => i.id === product?.id));
  const isCompared = useCompareStore(s => s.items.some(i => i.id === product?.id));
  const addToCart = useCartStore(s => s.addItem);
  const removeFromCart = useCartStore(s => s.removeItem);
  const addToFavorites = useFavoritesStore(s => s.addItem);
  const removeFromFavorites = useFavoritesStore(s => s.removeItem);
  const addToCompare = useCompareStore(s => s.addItem);
  const removeFromCompare = useCompareStore(s => s.removeItem);

  useEffect(() => {
    getDeliveryVariants().then(setDeliveryVariants).catch(console.error);
    getPaymentVariants().then(setPaymentVariants).catch(console.error);
  }, []);

  useEffect(() => {
    getCategories().then(setCategories).catch(console.error);
  }, []);

  useEffect(() => {
    if (!slug) return;
    setIsLoading(true);
    getProduct(slug)
      .then(data => { setProduct(data); setSelectedImage(0); })
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, [slug]);

  useEffect(() => {
    if (!lightboxOpen) return;
    const preventDefault = (e: Event) => e.preventDefault();
    document.addEventListener('wheel', preventDefault, { passive: false });
    document.addEventListener('touchmove', preventDefault, { passive: false });
    return () => {
      document.removeEventListener('wheel', preventDefault);
      document.removeEventListener('touchmove', preventDefault);
    };
  }, [lightboxOpen]);

  const categoryChain = useMemo(() => {
    if (!product || categories.length === 0) return [];
    const chain: Category[] = [];
    let current = categories.find(c => c.id === product.categoryId);
    while (current) {
      chain.unshift(current);
      current = current.parentId ? categories.find(c => c.id === current!.parentId) : undefined;
    }
    return chain;
  }, [product, categories]);

  const featureGroups = useMemo(() => {
    if (!product) return [];
    const map = new Map<string, { headerId: number; headerName: string; features: typeof product.features }>();
    for (const f of product.features) {
      if (!map.has(f.headerName)) {
        map.set(f.headerName, { headerId: f.headerId, headerName: f.headerName, features: [] });
      }
      map.get(f.headerName)!.features.push(f);
    }
    return [...map.values()].sort((a, b) => a.headerId - b.headerId);
  }, [product]);

  const actualPrice = product?.hasDiscount && product.discountPrice ? product.discountPrice : product?.price;

  if (isLoading) return <Layout><div className="max-w-6xl mx-auto px-4 py-8 text-gray-400">Завантаження...</div></Layout>;
  if (!product) return <Layout><div className="max-w-6xl mx-auto px-4 py-8 text-gray-500">Товар не знайдено</div></Layout>;

  return (
    <Layout>
      <div className="py-8">

        <nav className="flex flex-wrap items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/catalog')}>Каталог</span>
          {categoryChain.map((cat, i) => {
            const isLast = i === categoryChain.length - 1;
            const path = categoryChain.slice(0, i + 1).map(c => c.slug).join('/');
            return (
              <span key={cat.id} className="flex items-center gap-2">
                <span>/</span>
                <span
                  className="cursor-pointer hover:text-green-600"
                  onClick={() => navigate(isLast ? `/category/${path}` : `/catalog/${path}`)}
                >
                  {cat.name}
                </span>
              </span>
            );
          })}
          <span>/</span>
          <span className="text-gray-800 font-medium line-clamp-1">{product.name}</span>
        </nav>

        <div className="flex flex-col md:flex-row gap-6 mb-10">

          <div className="md:w-1/3 shrink-0">
            <div className="relative">
              <div
                className="rounded-xl overflow-hidden border border-gray-200 bg-white mb-3 cursor-zoom-in"
                onClick={() => setLightboxOpen(true)}
              >
                <img
                  src={product.imageUrls[selectedImage]}
                  alt={product.name}
                  className="w-full aspect-[4/5] object-contain"
                />
              </div>
              <button
                onClick={() => isFavorite
                  ? removeFromFavorites(product.id)
                  : addToFavorites({ id: product.id, name: product.name, slug: product.slug, imageUrl: product.imageUrls[0], price: actualPrice!, inStock: product.quantity > 0 })}
                className="absolute top-4 right-4 bg-white p-1.5 rounded-full shadow transition-colors"
              >
                <Heart
                  size={20}
                  className={`transition-colors ${
                    isFavorite
                      ? 'fill-red-700 text-red-700 hover:fill-red-500 hover:text-red-500'
                      : 'text-gray-600 hover:fill-red-300 hover:text-red-300'
                  }`}
                />
              </button>
            </div>
            {product.imageUrls.length > 1 && (
              <div className="flex gap-2 flex-wrap">
                {product.imageUrls.map((url, i) => (
                  <button
                    key={i}
                    onClick={() => setSelectedImage(i)}
                    className={`w-16 h-16 rounded-lg overflow-hidden border-2 transition-colors ${
                      selectedImage === i ? 'border-green-600' : 'border-gray-200'
                    }`}
                  >
                    <img src={url} alt="" className="w-full h-full object-cover" />
                  </button>
                ))}
              </div>
            )}
          </div>

          <div className="flex-1 flex flex-col gap-4">
            <p className="text-xs text-gray-400">Артикул: {product.article}</p>
            <h1 className="text-xl font-bold text-gray-800 leading-snug">{product.name}</h1>

            <div className="flex items-center gap-2">
              <div className="flex">
                {[1, 2, 3, 4, 5].map(star => (
                  <div key={star} className="relative">
                    <Star size={16} className="text-gray-300 fill-gray-300" />
                    <div
                      className="absolute inset-0 overflow-hidden"
                      style={{ width: `${Math.min(Math.max(product.rating - star + 1, 0), 1) * 100}%` }}
                    >
                      <Star size={16} className="text-yellow-400 fill-yellow-400" />
                    </div>
                  </div>
                ))}
              </div>
              <span className="text-sm text-gray-600">{product.rating.toFixed(1)}</span>
              <span className="text-sm text-gray-400">({product.reviewCount} відгуків)</span>
            </div>

            <span className={`text-sm font-medium ${product.quantity > 0 ? 'text-green-600' : 'text-red-500'}`}>
              {product.quantity > 0 ? 'В наявності' : 'Немає в наявності'}
            </span>

            <div className="flex items-end gap-3">
              <span className="text-3xl font-bold text-green-700">{actualPrice} ₴</span>
              {product.hasDiscount && product.price && (
                <span className="text-lg text-gray-400 line-through mb-0.5">{product.price} ₴</span>
              )}
            </div>

            <div className="flex gap-3 mt-2">
              <button
                onClick={() => isInCart
                  ? removeFromCart(product.id)
                  : addToCart({ id: product.id, name: product.name, imageUrl: product.imageUrls[0], price: actualPrice!, quantity: 1 })}
                className={`flex-1 py-3 rounded-lg font-medium transition-colors flex items-center justify-center gap-2 ${
                  isInCart ? 'bg-green-800 hover:bg-green-700 text-white' : 'bg-green-600 hover:bg-green-700 text-white'
                }`}
              >
                <ShoppingCart size={18} />
                {isInCart ? 'В кошику' : 'В кошик'}
              </button>
              <button
                onClick={() => isCompared
                  ? removeFromCompare(product.id)
                  : addToCompare({ id: product.id, name: product.name, imageUrl: product.imageUrls[0], price: actualPrice! })}
                className={`flex-1 py-3 rounded-lg border font-medium transition-colors flex items-center justify-center gap-2 ${
                  isCompared
                    ? 'border-orange-400 text-orange-600 bg-orange-50'
                    : 'border-gray-300 text-gray-600 hover:border-orange-400 hover:text-orange-500'
                }`}
              >
                <Scale size={18} />
                {isCompared ? 'В порівнянні' : 'Порівняти'}
              </button>
            </div>
          </div>

          <div className="flex flex-col gap-6 md:w-64 shrink-0">
            {deliveryVariants.length > 0 && (
              <div>
                <h3 className="text-sm font-semibold text-gray-700 mb-3">Доставка</h3>
                <div className="flex flex-col gap-2">
                  {deliveryVariants.map(v => (
                    <div key={v.id} className="border border-gray-200 rounded-lg px-3 py-2.5">
                      <p className="text-sm font-medium text-gray-700">{v.name}</p>
                      <p className="text-xs text-gray-400 mt-0.5">{v.description}</p>
                    </div>
                  ))}
                </div>
              </div>
            )}
            {paymentVariants.length > 0 && (
              <div>
                <h3 className="text-sm font-semibold text-gray-700 mb-3">Оплата</h3>
                <div className="flex flex-col gap-2">
                  {paymentVariants.map(v => (
                    <div key={v.id} className="border border-gray-200 rounded-lg px-3 py-2.5">
                      <p className="text-sm font-medium text-gray-700">{v.name}</p>
                      <p className="text-xs text-gray-400 mt-0.5">{v.description}</p>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>

        </div>

        <div className="mb-10">
          <h2 className="text-lg font-bold text-gray-800 mb-4">Характеристики</h2>
          <div className="flex flex-col gap-6">
            {featureGroups.map(group => (
              <div key={group.headerId}>
                <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-3">{group.headerName}</h3>
                <div className="border rounded-lg overflow-hidden">
                  {group.features.map((f, i) => (
                    <div
                      key={f.featureId}
                      className={`flex ${i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}`}
                    >
                      <span className="w-1/2 px-4 py-2.5 text-sm text-gray-500 border-r border-gray-200">{f.featureName}</span>
                      <span className="w-1/2 px-4 py-2.5 text-sm text-gray-800 font-medium">{f.value}</span>
                    </div>
                  ))}
                </div>
              </div>
            ))}
          </div>
        </div>

        {product.description && (
          <div>
            <h2 className="text-lg font-bold text-gray-800 mb-4">Про цей товар</h2>
            <p className="text-sm text-gray-600 leading-relaxed whitespace-pre-line">{product.description}</p>
          </div>
        )}

      </div>

      {lightboxOpen && (
        <div
          className="fixed inset-0 z-50 bg-black/80 flex items-center justify-center p-4"
          onClick={() => setLightboxOpen(false)}
        >
          <img
            src={product.imageUrls[selectedImage]}
            alt={product.name}
            className="max-w-full max-h-full object-contain rounded-lg"
            onClick={e => e.stopPropagation()}
          />
        </div>
      )}
    </Layout>
  );
}
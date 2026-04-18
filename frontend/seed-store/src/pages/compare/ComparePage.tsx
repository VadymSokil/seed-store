import { useEffect, useState, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { X, Star } from 'lucide-react';
import Layout from '../../components/layout/Layout';
import { getProduct } from '../../api/productsApi';
import type { ProductDetails } from '../../api/productsApi';
import { useCompareStore } from '../../store/useCompareStore';

export default function ComparePage() {
  const navigate = useNavigate();
  const compareItems = useCompareStore(s => s.items);
  const removeItem = useCompareStore(s => s.removeItem);
  const clearItems = useCompareStore(s => s.clearItems);

  const [products, setProducts] = useState<ProductDetails[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const showDiff = true;

  useEffect(() => {
    if (compareItems.length === 0) { setProducts([]); return; }
    setIsLoading(true);
    Promise.all(compareItems.map(i => getProduct(String(i.id))))
      .then(setProducts)
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, [compareItems.map(i => i.id).join(',')]);

  const featureValueMap = useMemo(() => {
    const map = new Map<string, Set<string>>();
    for (const p of products) {
      for (const f of p.features) {
        if (!map.has(f.featureSlug)) map.set(f.featureSlug, new Set());
        map.get(f.featureSlug)!.add(f.value);
      }
    }
    return map;
  }, [products]);

  const getFeatureBg = (featureSlug: string, value: string | undefined) => {
    if (!showDiff) return '';
    const values = featureValueMap.get(featureSlug);
    if (!value || !values || values.size > 1) return 'bg-red-50';
    return 'bg-green-50';
  };

  return (
    <Layout>
      <div className="py-8">
        <nav className="flex flex-wrap items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="text-gray-800 font-medium">Порівняння</span>
        </nav>

        {products.length > 0 && (
          <div className="flex items-center justify-between mb-6">
            <button
              onClick={() => { clearItems(); setProducts([]); }}
              className="text-sm text-gray-400 hover:text-red-500 transition-colors"
            >
              Очистити список
            </button>
          </div>
        )}

        {isLoading ? (
          <div className="text-gray-400">Завантаження...</div>
        ) : products.length === 0 ? (
          <p className="text-gray-500">Список порівняння порожній</p>
        ) : (
          <div className="flex gap-4 overflow-x-auto pb-4 scrollbar-hide">
            {products.map(p => {
              const price = p.hasDiscount && p.discountPrice ? p.discountPrice : p.price;
              return (
                <div key={p.id} className="flex-none w-64 border border-gray-200 rounded-xl overflow-hidden bg-white">
                  <div className="flex justify-end p-2">
                    <button
                      onClick={() => removeItem(p.id)}
                      className="text-gray-300 hover:text-red-400 transition-colors"
                    >
                      <X size={16} />
                    </button>
                  </div>

                  <div className="cursor-pointer" onClick={() => navigate(`/product/${p.slug}`)}>
                    <img src={p.imageUrls[0]} alt={p.name} className="w-full h-48 object-contain" />
                  </div>

                  <div className="p-4 flex flex-col gap-3">
                    <p
                      className="text-sm font-medium line-clamp-3 cursor-pointer hover:text-green-700"
                      onClick={() => navigate(`/product/${p.slug}`)}
                    >
                      {p.name}
                    </p>

                    <div className="flex items-center gap-1">
                      <div className="flex">
                        {[1, 2, 3, 4, 5].map(star => (
                          <div key={star} className="relative">
                            <Star size={13} className="text-gray-300 fill-gray-300" />
                            <div
                              className="absolute inset-0 overflow-hidden"
                              style={{ width: `${Math.min(Math.max(p.rating - star + 1, 0), 1) * 100}%` }}
                            >
                              <Star size={13} className="text-yellow-400 fill-yellow-400" />
                            </div>
                          </div>
                        ))}
                      </div>
                      <span className="text-xs text-gray-500">{p.rating.toFixed(1)}</span>
                      <span className="text-xs text-gray-400">({p.reviewCount})</span>
                    </div>

                    <span className={`text-xs font-medium ${p.quantity > 0 ? 'text-green-600' : 'text-red-500'}`}>
                      {p.quantity > 0 ? 'В наявності' : 'Немає в наявності'}
                    </span>

                    <div className="flex items-end gap-2">
                      <span className="text-lg font-bold text-green-700">{price} ₴</span>
                      {p.hasDiscount && <span className="text-sm text-gray-400 line-through">{p.price} ₴</span>}
                    </div>

                    <div className="border-t border-gray-100 pt-3">
                      <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-2">Характеристики</p>
                      <div className="flex flex-col gap-1">
                        {p.features.map(f => (
                          <div
                            key={f.featureId}
                            className={`flex flex-col px-2 py-1 rounded transition-colors ${getFeatureBg(f.featureSlug, f.value)}`}
                          >
                            <span className="text-xs text-gray-400">{f.featureName}</span>
                            <span className="text-xs font-medium text-gray-700">{f.value}</span>
                          </div>
                        ))}
                      </div>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </Layout>
  );
}
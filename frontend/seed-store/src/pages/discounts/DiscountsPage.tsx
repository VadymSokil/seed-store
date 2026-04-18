import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { ChevronDown, ChevronUp } from 'lucide-react';
import Layout from '../../components/layout/Layout';
import { getActiveDiscountGroups } from '../../api/productsApi';
import type { DiscountGroup } from '../../api/productsApi';

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('uk-UA', { day: '2-digit', month: '2-digit', year: 'numeric' });

export default function DiscountsPage() {
  const navigate = useNavigate();
  const [groups, setGroups] = useState<DiscountGroup[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [openIds, setOpenIds] = useState<Set<number>>(new Set());

  useEffect(() => {
    getActiveDiscountGroups()
      .then(setGroups)
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, []);

  const toggleGroup = (id: number) => {
    setOpenIds(prev => {
      const next = new Set(prev);
      next.has(id) ? next.delete(id) : next.add(id);
      return next;
    });
  };

  return (
    <Layout>
      <div className="py-8">
        <nav className="flex items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="text-gray-800 font-medium">Акції</span>
        </nav>

        <h1 className="text-xl font-bold text-gray-800 mb-6">Акції</h1>

        {isLoading ? (
          <p className="text-sm text-gray-400">Завантаження...</p>
        ) : groups.length === 0 ? (
          <p className="text-sm text-gray-400">Активних акцій немає</p>
        ) : (
          <div className="flex flex-col gap-3">
            {groups.map(group => (
              <div key={group.id} className="border border-gray-200 rounded-lg">
                <button
                  onClick={() => toggleGroup(group.id)}
                  className="w-full flex items-center justify-between px-4 py-3 hover:bg-gray-50 transition-colors"
                >
                  <div className="flex items-center gap-4">
                    <p className="text-sm font-semibold text-gray-800">{group.name}</p>
                    <p className="text-xs text-gray-400">{formatDate(group.startDate)} — {formatDate(group.endDate)}</p>
                  </div>
                  {openIds.has(group.id) ? <ChevronUp size={16} className="text-gray-400" /> : <ChevronDown size={16} className="text-gray-400" />}
                </button>

                {openIds.has(group.id) && (
                  <div className="border-t border-gray-100 p-4 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
                    {group.products.map(product => (
                      <div
                        key={product.productId}
                        onClick={() => navigate(`/product/${product.slug}`)}
                        className="bg-white rounded-lg border border-gray-200 overflow-hidden cursor-pointer hover:shadow-md hover:-translate-y-1 transition-all duration-200"
                      >
                        <div className="relative">
                          <img
                            src={product.imageUrl}
                            alt={product.name}
                            className="w-full h-64 object-contain"
                          />
                          <span className="absolute top-2 left-2 bg-red-500 text-white text-xs font-bold px-1.5 py-0.5 rounded">
                            -{product.discountPercent}%
                          </span>
                        </div>
                        <div className="p-3 flex flex-col gap-2">
                          <p className="text-sm font-medium line-clamp-2 min-h-10 hover:text-green-700">
                            {product.name}
                          </p>
                          <div className="flex items-center gap-2">
                            <span className="text-base font-bold text-green-700">{product.discountPrice} ₴</span>
                            <span className="text-sm text-gray-400 line-through">{product.originalPrice} ₴</span>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
    </Layout>
  );
}
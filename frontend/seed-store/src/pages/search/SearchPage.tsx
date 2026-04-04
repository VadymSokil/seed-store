import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import Layout from '../../components/layout/Layout';
import { searchProducts, searchCategories } from '../../api/searchApi';
import type { SearchProduct, SearchCategory } from '../../api/searchApi';

export default function SearchPage() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const query = searchParams.get('q') ?? '';

  const [products, setProducts] = useState<SearchProduct[]>([]);
  const [categories, setCategories] = useState<SearchCategory[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (!query) return;
    setIsLoading(true);
    Promise.all([searchProducts(query), searchCategories(query)])
      .then(([prods, cats]) => { setProducts(prods); setCategories(cats); })
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, [query]);

  const total = products.length + categories.length;

  return (
    <Layout>
      <div className="py-8">
        <nav className="flex flex-wrap items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="text-gray-800 font-medium">Пошук</span>
        </nav>

        {query && (
          <p className="text-sm text-gray-400 mb-6">
            {isLoading ? 'Пошук...' : `За запитом "${query}" знайдено: ${total} результатів`}
          </p>
        )}

        {!query ? (
          <p className="text-gray-500">Введіть запит для пошуку</p>
        ) : isLoading ? (
          <div className="text-gray-400">Завантаження...</div>
        ) : total === 0 ? (
          <p className="text-gray-500">Нічого не знайдено</p>
        ) : (
          <div className="flex flex-col gap-8">
            {categories.length > 0 && (
              <div>
                <h2 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-3">Категорії</h2>
                <div className="flex flex-col divide-y divide-gray-100 border border-gray-200 rounded-lg overflow-hidden">
                  {categories.map((cat, i) => (
                    <div
                      key={i}
                      onClick={() => navigate(`/category/${cat.path}`)}
                      className="px-4 py-3 hover:bg-gray-50 cursor-pointer text-sm text-gray-700"
                    >
                      {cat.name}
                    </div>
                  ))}
                </div>
              </div>
            )}

            {products.length > 0 && (
              <div>
                <h2 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-3">Товари</h2>
                <div className="flex flex-col divide-y divide-gray-100 border border-gray-200 rounded-lg overflow-hidden">
                  {products.map((product, i) => (
                    <div
                      key={i}
                      onClick={() => navigate(`/product/${product.slug}`)}
                      className="flex items-center gap-4 px-4 py-3 hover:bg-gray-50 cursor-pointer"
                    >
                      <img src={product.imageUrl} alt={product.name} className="w-14 h-14 object-cover rounded" />
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium line-clamp-1">{product.name}</p>
                        <p className="text-sm text-green-700 font-bold">{product.price} ₴</p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        )}
      </div>
    </Layout>
  );
}
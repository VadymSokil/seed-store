import { useEffect, useMemo, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { SlidersHorizontal } from 'lucide-react';
import Layout from '../../components/layout/Layout';
import { getCategories } from '../../api/catalogApi';
import { getProductList, getProductFilters } from '../../api/productsApi';
import type { Category } from '../../types/catalog';
import type { TopProduct, ProductFiltersResponse } from '../../api/productsApi';
import ProductCard from '../../components/product/ProductCard';
import FiltersModal from '../../components/modals/FiltersModal';
import type { ActiveFilter } from '../../components/category/CategoryFeatureFilters';

const PAGE_SIZE = 12;

interface Filters {
  priceFrom: number | null;
  priceTo: number | null;
  hasDiscount: boolean | null;
  inStock: boolean | null;
  sortByPriceAsc: boolean | null;
  sortByPriceDesc: boolean | null;
}

const defaultFilters: Filters = {
  priceFrom: null,
  priceTo: null,
  hasDiscount: null,
  inStock: null,
  sortByPriceAsc: null,
  sortByPriceDesc: null,
};

export default function CategoryPage() {
  const { '*': splat } = useParams();
  const navigate = useNavigate();

  const [categories, setCategories] = useState<Category[]>([]);
  const [products, setProducts] = useState<TopProduct[]>([]);
  const [filtersData, setFiltersData] = useState<ProductFiltersResponse | null>(null);
  const [isLoadingProducts, setIsLoadingProducts] = useState(false);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [filters, setFilters] = useState<Filters>(defaultFilters);
  const [activeFilters, setActiveFilters] = useState<ActiveFilter[]>([]);
  const [selectedFeatures, setSelectedFeatures] = useState<Record<number, Set<string>>>({});
  const [isFiltersModalOpen, setIsFiltersModalOpen] = useState(false);

  const stableFilters = useMemo(() => filters, [
    filters.priceFrom,
    filters.priceTo,
    filters.hasDiscount,
    filters.inStock,
    filters.sortByPriceAsc,
    filters.sortByPriceDesc,
  ]);

  useEffect(() => {
    getCategories().then(setCategories).catch(console.error);
  }, []);

  const slugs = useMemo(
    () => (splat ? splat.split('/').filter(Boolean) : []),
    [splat]
  );

  const breadcrumbChain = useMemo(() => {
    const chain: Category[] = [];
    let currentParentId: number | null = null;

    for (const slug of slugs) {
      const found = categories.find(
        c => c.slug === slug && c.parentId === currentParentId
      );
      if (!found) break;
      chain.push(found);
      currentParentId = found.id;
    }

    return chain;
  }, [slugs, categories]);

  const currentCategory = breadcrumbChain[breadcrumbChain.length - 1];
  const totalPages = Math.ceil(totalCount / PAGE_SIZE);

  useEffect(() => {
    setPage(1);
    setFilters(defaultFilters);
    setActiveFilters([]);
    setSelectedFeatures({});
    setFiltersData(null);
  }, [currentCategory?.id]);

  useEffect(() => {
    if (!currentCategory) return;
    getProductFilters(currentCategory.id).then(setFiltersData).catch(console.error);
  }, [currentCategory?.id]);

  useEffect(() => {
    setPage(prev => prev === 1 ? prev : 1);
  }, [stableFilters, activeFilters]);

  useEffect(() => {
    if (!currentCategory) return;
    setIsLoadingProducts(true);
    getProductList({
      categoryId: currentCategory.id,
      page,
      pageSize: PAGE_SIZE,
      activeFilters: activeFilters.length > 0 ? activeFilters : null,
      ...stableFilters,
    })
      .then(data => {
        setProducts(data.items);
        setTotalCount(data.totalCount);
      })
      .catch(console.error)
      .finally(() => setIsLoadingProducts(false));
  }, [currentCategory?.id, page, stableFilters, activeFilters]);

  const getPageNumbers = () => {
    if (totalPages <= 5) return Array.from({ length: totalPages }, (_, i) => i + 1);
    if (page <= 3) return [1, 2, 3, 4, '...', totalPages];
    if (page >= totalPages - 2) return [1, '...', totalPages - 3, totalPages - 2, totalPages - 1, totalPages];
    return [1, '...', page - 1, page, page + 1, '...', totalPages];
  };

  return (
    <Layout>
      <FiltersModal
        isOpen={isFiltersModalOpen}
        onClose={() => setIsFiltersModalOpen(false)}
        filters={filters}
        onFiltersChange={setFilters}
        filtersData={filtersData}
        activeFilters={activeFilters}
        onActiveFiltersChange={setActiveFilters}
        selectedFeatures={selectedFeatures}
        onSelectedFeaturesChange={setSelectedFeatures}
      />

      <div className="py-8">
        <div className="flex flex-wrap items-center justify-between mb-6">
          <nav className="flex items-center gap-2 text-sm text-gray-500">
            <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>
              Головна
            </span>
            <span>/</span>
            <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/catalog')}>
              Каталог
            </span>
            {breadcrumbChain.map((cat, i) => (
              <span key={cat.id} className="flex items-center gap-2">
                <span>/</span>
                <span
                  className={`cursor-pointer hover:text-green-600 ${
                    i === breadcrumbChain.length - 1 ? 'text-gray-800 font-medium' : ''
                  }`}
                  onClick={() => navigate('/catalog/' + slugs.slice(0, i + 1).join('/'))}
                >
                  {cat.name}
                </span>
              </span>
            ))}
          </nav>

          <button
            onClick={() => setIsFiltersModalOpen(true)}
            className="flex items-center gap-2 border border-gray-300 text-gray-600 px-3 py-1.5 rounded hover:border-green-600 hover:text-green-600 transition-colors text-sm"
          >
            <SlidersHorizontal size={16} />
            Фільтри
          </button>
        </div>

        <div className="flex-1">
          {isLoadingProducts ? (
            <div className="text-gray-400">Завантаження...</div>
          ) : products.length === 0 ? (
            <p className="text-gray-500">Товари відсутні</p>
          ) : (
            <>
              <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
                {products.map(p => (
                  <ProductCard
                    key={p.id}
                    id={p.id}
                    slug={p.slug}
                    name={p.name}
                    imageUrl={p.imageUrl}
                    price={p.hasDiscount && p.discountPrice ? p.discountPrice : p.price}
                    oldPrice={p.hasDiscount ? p.price : undefined}
                    rating={p.rating}
                    reviewsCount={p.reviewCount}
                    inStock={p.quantity > 0}
                  />
                ))}
              </div>

              {totalPages > 1 && (
                <div className="flex justify-center items-center gap-1 mt-8">
                  <button
                    onClick={() => setPage(p => p - 1)}
                    disabled={page === 1}
                    className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
                  >
                    ←
                  </button>
                  {getPageNumbers().map((p, i) =>
                    p === '...' ? (
                      <span key={`dots-${i}`} className="px-3 py-2 text-gray-400">...</span>
                    ) : (
                      <button
                        key={p}
                        onClick={() => setPage(p as number)}
                        className={`px-3 py-2 rounded border transition-colors ${
                          page === p
                            ? 'bg-green-600 text-white border-green-600'
                            : 'border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600'
                        }`}
                      >
                        {p}
                      </button>
                    )
                  )}
                  <button
                    onClick={() => setPage(p => p + 1)}
                    disabled={page === totalPages}
                    className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
                  >
                    →
                  </button>
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </Layout>
  );
}
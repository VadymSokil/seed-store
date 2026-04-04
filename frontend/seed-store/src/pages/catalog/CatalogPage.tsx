import { useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getCategories } from '../../api/catalogApi';
import type { Category } from '../../types/catalog';
import CategoryCard from '@/components/сatalog/CategoryCard';
import Layout from '../../components/layout/Layout';

export default function CatalogPage() {
  const { '*': splat } = useParams();
  const navigate = useNavigate();

  const [categories, setCategories] = useState<Category[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    getCategories()
      .then(setCategories)
      .finally(() => setIsLoading(false));
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

  const currentParentId =
    breadcrumbChain.length > 0
      ? breadcrumbChain[breadcrumbChain.length - 1].id
      : null;

  const visibleCategories = useMemo(
    () =>
      categories
        .filter(c => c.parentId === currentParentId)
        .sort((a, b) => a.viewOrder - b.viewOrder),
    [categories, currentParentId]
  );

  const handleCardClick = (category: Category) => {
    const hasChildren = categories.some(c => c.parentId === category.id);
    if (hasChildren) {
      navigate('/catalog/' + [...slugs, category.slug].join('/'));
    } else {
      navigate('/category/' + [...slugs, category.slug].join('/'));
    }
  };

  const handleBreadcrumbClick = (index: number) => {
    if (index < 0) {
      navigate('/catalog');
    } else {
      navigate('/catalog/' + slugs.slice(0, index + 1).join('/'));
    }
  };

  if (isLoading) return <Layout><div className="p-8">Завантаження...</div></Layout>;

  return (
    <Layout>
      <div className="py-8">
        <nav className="flex flex-wrap items-center gap-2 text-sm mb-6 text-gray-500">
          <span
            className="cursor-pointer hover:text-green-600"
            onClick={() => navigate('/')}
          >
            Головна
          </span>
          <span>/</span>
          <span
            className="cursor-pointer hover:text-green-600"
            onClick={() => handleBreadcrumbClick(-1)}
          >
            Каталог
          </span>
          {breadcrumbChain.map((cat, i) => (
            <span key={cat.id} className="flex items-center gap-2">
              <span>/</span>
              <span
                className={`cursor-pointer hover:text-green-600 ${
                  i === breadcrumbChain.length - 1 ? 'text-gray-800 font-medium' : ''
                }`}
                onClick={() => handleBreadcrumbClick(i)}
              >
                {cat.name}
              </span>
            </span>
          ))}
        </nav>

        {visibleCategories.length === 0 ? (
          <p className="text-gray-500">Категорії відсутні</p>
        ) : (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
            {visibleCategories.map(cat => (
              <CategoryCard key={cat.id} category={cat} onClick={handleCardClick} />
            ))}
          </div>
        )}
      </div>
    </Layout>
  );
}
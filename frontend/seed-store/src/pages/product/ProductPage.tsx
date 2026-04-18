import { useEffect, useState, useMemo } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Star, Heart, Scale, ShoppingCart, Pencil, Trash2 } from 'lucide-react';
import Layout from '../../components/layout/Layout';
import { getProduct, getProductList } from '../../api/productsApi';
import type { ProductDetails, TopProduct } from '../../api/productsApi';
import { useCartStore } from '../../store/useCartStore';
import { useFavoritesStore } from '../../store/useFavoritesStore';
import { useCompareStore } from '../../store/useCompareStore';
import { getCategories } from '../../api/catalogApi';
import type { Category } from '../../types/catalog';
import { getDeliveryVariants, getPaymentVariants } from '../../api/storeInfoApi';
import type { StoreInfoVariant } from '../../api/storeInfoApi';
import { getProductReviews, addReview, updateReview, deleteReview } from '../../api/reviewsApi';
import type { ProductReview } from '../../api/reviewsApi';
import { useAuthStore } from '../../store/useAuthStore';
import { useRecentlyViewedStore } from '../../store/useRecentlyViewedStore';
import ProductCarousel from '../../components/product/ProductCarousel';

type ProductTab = 'description' | 'features' | 'reviews';
const PAGE_SIZE = 5;

const StarRating = ({ value, onChange }: { value: number; onChange?: (v: number) => void }) => (
  <div className="flex gap-1">
    {[1, 2, 3, 4, 5].map(star => (
      <button key={star} type="button" onClick={() => onChange?.(star)} className={onChange ? 'cursor-pointer' : 'cursor-default'}>
        <Star size={20} className={star <= value ? 'text-yellow-400 fill-yellow-400' : 'text-gray-300 fill-gray-300'} />
      </button>
    ))}
  </div>
);

const formatDateTime = (iso: string) =>
  new Date(iso).toLocaleString('uk-UA', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });

export default function ProductPage() {
  const { slug } = useParams<{ slug: string }>();
  const navigate = useNavigate();
  const account = useAuthStore(s => s.account);

  const [product, setProduct] = useState<ProductDetails | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedImage, setSelectedImage] = useState(0);
  const [lightboxOpen, setLightboxOpen] = useState(false);
  const [deliveryVariants, setDeliveryVariants] = useState<StoreInfoVariant[]>([]);
  const [paymentVariants, setPaymentVariants] = useState<StoreInfoVariant[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [relatedProducts, setRelatedProducts] = useState<TopProduct[]>([]);
  const [activeTab, setActiveTab] = useState<ProductTab>('description');

  const [reviews, setReviews] = useState<ProductReview[]>([]);
  const [reviewsTotalCount, setReviewsTotalCount] = useState(0);
  const [reviewsPage, setReviewsPage] = useState(1);
  const [reviewsLoading, setReviewsLoading] = useState(false);
  const [editingReview, setEditingReview] = useState<ProductReview | null>(null);
  const [formRating, setFormRating] = useState(5);
  const [formText, setFormText] = useState('');
  const [formError, setFormError] = useState('');
  const [formLoading, setFormLoading] = useState(false);
  const [showForm, setShowForm] = useState(false);

  const isInCart = useCartStore(s => s.items.some(i => i.id === product?.id));
  const isFavorite = useFavoritesStore(s => s.items.some(i => i.id === product?.id));
  const isCompared = useCompareStore(s => s.items.some(i => i.id === product?.id));
  const addToCart = useCartStore(s => s.addItem);
  const removeFromCart = useCartStore(s => s.removeItem);
  const addToFavorites = useFavoritesStore(s => s.addItem);
  const removeFromFavorites = useFavoritesStore(s => s.removeItem);
  const addToCompare = useCompareStore(s => s.addItem);
  const removeFromCompare = useCompareStore(s => s.removeItem);
  const addToRecentlyViewed = useRecentlyViewedStore(s => s.addItem);
  const recentlyViewed = useRecentlyViewedStore(s => s.items);

  const getPageNumbers = () => {
  if (totalPages <= 5) return Array.from({ length: totalPages }, (_, i) => i + 1);
  if (reviewsPage <= 3) return [1, 2, 3, 4, '...', totalPages];
  if (reviewsPage >= totalPages - 2) return [1, '...', totalPages - 3, totalPages - 2, totalPages - 1, totalPages];
  return [1, '...', reviewsPage - 1, reviewsPage, reviewsPage + 1, '...', totalPages];
};

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
    setActiveTab('description');
    getProduct(slug)
      .then(data => {
        setProduct(data);
        setSelectedImage(0);
        addToRecentlyViewed({
          id: data.id,
          slug: data.slug,
          name: data.name,
          imageUrl: data.imageUrls[0],
          price: data.hasDiscount && data.discountPrice ? data.discountPrice : data.price,
          oldPrice: data.hasDiscount && data.discountPrice ? data.price : undefined,
          rating: data.rating,
          reviewsCount: data.reviewCount,
          inStock: data.quantity > 0,
        });
        getProductList({
          categoryId: data.categoryId,
          priceFrom: null, priceTo: null,
          hasDiscount: null, sortByPriceAsc: null, sortByPriceDesc: null,
          inStock: null, activeFilters: null,
          page: 1, pageSize: 10,
        })
          .then(res => setRelatedProducts(res.items.filter(p => p.id !== data.id)))
          .catch(console.error);
      })
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, [slug]);

  useEffect(() => {
    if (!product) return;
    setReviewsLoading(true);
    getProductReviews(product.id, reviewsPage, PAGE_SIZE)
      .then(data => { setReviews(data.reviews); setReviewsTotalCount(data.totalCount); })
      .catch(console.error)
      .finally(() => setReviewsLoading(false));
  }, [product, reviewsPage]);

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
      if (!map.has(f.headerName)) map.set(f.headerName, { headerId: f.headerId, headerName: f.headerName, features: [] });
      map.get(f.headerName)!.features.push(f);
    }
    return [...map.values()].sort((a, b) => a.headerId - b.headerId);
  }, [product]);

  const actualPrice = product?.hasDiscount && product.discountPrice ? product.discountPrice : product?.price;
  const myReview = account ? reviews.find(r => r.accountId === account.id) : null;
  const totalPages = Math.ceil(reviewsTotalCount / PAGE_SIZE);

  const openAddForm = () => { setEditingReview(null); setFormRating(5); setFormText(''); setFormError(''); setShowForm(true); };
  const openEditForm = (review: ProductReview) => { setEditingReview(review); setFormRating(review.rating); setFormText(review.text ?? ''); setFormError(''); setShowForm(true); };
  const cancelForm = () => { setShowForm(false); setEditingReview(null); setFormError(''); };

  const handleSubmitReview = async () => {
    setFormError('');
    if (formRating < 1) { setFormError('Оберіть оцінку'); return; }
    setFormLoading(true);
    try {
      if (editingReview) {
        await updateReview(editingReview.id, { rating: formRating, text: formText || undefined });
      } else {
        await addReview({ productId: product!.id, rating: formRating, text: formText || undefined });
      }
      setShowForm(false);
      setEditingReview(null);
      setReviewsPage(1);
      const data = await getProductReviews(product!.id, 1, PAGE_SIZE);
      setReviews(data.reviews);
      setReviewsTotalCount(data.totalCount);
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'review_exists') setFormError('Ви вже залишили відгук на цей товар');
      else if (status === 'not_purchased') setFormError('Ви можете залишити відгук лише після покупки цього товару');
      else setFormError('Помилка. Спробуйте ще раз');
    } finally { setFormLoading(false); }
  };

  const handleDeleteReview = async (reviewId: number) => {
    try {
      await deleteReview(reviewId);
      const data = await getProductReviews(product!.id, reviewsPage, PAGE_SIZE);
      setReviews(data.reviews);
      setReviewsTotalCount(data.totalCount);
    } catch { }
  };

  if (isLoading) return <Layout><div className="py-8 text-gray-400">Завантаження...</div></Layout>;
  if (!product) return <Layout><div className="py-8 text-gray-500">Товар не знайдено</div></Layout>;

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
                <span className="cursor-pointer hover:text-green-600" onClick={() => navigate(isLast ? `/category/${path}` : `/catalog/${path}`)}>
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
              <div className="rounded-xl overflow-hidden border border-gray-200 bg-white mb-3 cursor-zoom-in" onClick={() => setLightboxOpen(true)}>
                <img src={product.imageUrls[selectedImage]} alt={product.name} className="w-full aspect-[4/5] object-contain" />
              </div>
              <button
                onClick={() => isFavorite
                  ? removeFromFavorites(product.id)
                  : addToFavorites({ id: product.id, name: product.name, slug: product.slug, imageUrl: product.imageUrls[0], price: actualPrice!, inStock: product.quantity > 0 })}
                className="absolute top-4 right-4 bg-white p-1.5 rounded-full shadow transition-colors"
              >
                <Heart size={20} className={`transition-colors ${isFavorite ? 'fill-red-700 text-red-700 hover:fill-red-500 hover:text-red-500' : 'text-gray-600 hover:fill-red-300 hover:text-red-300'}`} />
              </button>
            </div>
            {product.imageUrls.length > 1 && (
              <div className="flex gap-2 flex-wrap">
                {product.imageUrls.map((url, i) => (
                  <button key={i} onClick={() => setSelectedImage(i)} className={`w-16 h-16 rounded-lg overflow-hidden border-2 transition-colors ${selectedImage === i ? 'border-green-600' : 'border-gray-200'}`}>
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
                    <div className="absolute inset-0 overflow-hidden" style={{ width: `${Math.min(Math.max(product.rating - star + 1, 0), 1) * 100}%` }}>
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
              {product.hasDiscount && product.price && <span className="text-lg text-gray-400 line-through mb-0.5">{product.price} ₴</span>}
            </div>
            <div className="flex gap-3 mt-2">
              <button
                onClick={() => isInCart ? removeFromCart(product.id) : addToCart({ id: product.id, name: product.name, imageUrl: product.imageUrls[0], price: actualPrice!, quantity: 1 })}
                className={`flex-1 py-3 rounded-lg font-medium transition-colors flex items-center justify-center gap-2 ${isInCart ? 'bg-green-800 hover:bg-green-700 text-white' : 'bg-green-600 hover:bg-green-700 text-white'}`}
              >
                <ShoppingCart size={18} />
                {isInCart ? 'В кошику' : 'В кошик'}
              </button>
              <button
                onClick={() => isCompared ? removeFromCompare(product.id) : addToCompare({ id: product.id, name: product.name, imageUrl: product.imageUrls[0], price: actualPrice! })}
                className={`flex-1 py-3 rounded-lg border font-medium transition-colors flex items-center justify-center gap-2 ${isCompared ? 'border-orange-400 text-orange-600 bg-orange-50' : 'border-gray-300 text-gray-600 hover:border-orange-400 hover:text-orange-500'}`}
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

        {/* Табы */}
        <div className="flex border-b border-gray-200 mb-6">
          {([
            { key: 'description', label: 'Опис' },
            { key: 'features', label: 'Характеристики' },
            { key: 'reviews', label: `Відгуки (${product.reviewCount})` },
          ] as { key: ProductTab; label: string }[]).map(tab => (
            <button
              key={tab.key}
              onClick={() => setActiveTab(tab.key)}
              className={`px-4 py-2.5 text-sm font-medium transition-colors border-b-2 -mb-px
                ${activeTab === tab.key ? 'border-green-600 text-green-700' : 'border-transparent text-gray-500 hover:text-gray-800'}`}
            >
              {tab.label}
            </button>
          ))}
        </div>

        <div className="mb-10">
          {activeTab === 'description' && (
            product.description
              ? <p className="text-sm text-gray-600 leading-relaxed whitespace-pre-line">{product.description}</p>
              : <p className="text-sm text-gray-400">Опис відсутній</p>
          )}

          {activeTab === 'features' && (
            <div className="flex flex-col gap-6">
              {featureGroups.map(group => (
                <div key={group.headerId}>
                  <h3 className="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-3">{group.headerName}</h3>
                  <div className="border rounded-lg overflow-hidden">
                    {group.features.map((f, i) => (
                      <div key={f.featureId} className={`flex ${i % 2 === 0 ? 'bg-gray-50' : 'bg-white'}`}>
                        <span className="w-1/2 px-4 py-2.5 text-sm text-gray-500 border-r border-gray-200">{f.featureName}</span>
                        <span className="w-1/2 px-4 py-2.5 text-sm text-gray-800 font-medium">{f.value}</span>
                      </div>
                    ))}
                  </div>
                </div>
              ))}
            </div>
          )}

          {activeTab === 'reviews' && (
            <div>
              {account && !myReview && !showForm && (
                <div className="flex justify-end mb-6">
                  <button onClick={openAddForm} className="text-sm text-green-600 hover:text-green-700 font-medium transition-colors">
                    Залишити відгук
                  </button>
                </div>
              )}

              {showForm && (
                <div className="border border-gray-200 rounded-lg p-4 mb-6 flex flex-col gap-3">
                  <p className="text-sm font-semibold text-gray-700">{editingReview ? 'Редагувати відгук' : 'Новий відгук'}</p>
                  <div>
                    <p className="text-xs text-gray-400 mb-1">Оцінка *</p>
                    <StarRating value={formRating} onChange={setFormRating} />
                  </div>
                  <div>
                    <p className="text-xs text-gray-400 mb-1">Коментар</p>
                    <textarea
                      value={formText}
                      onChange={e => setFormText(e.target.value)}
                      rows={3}
                      className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 resize-none"
                      placeholder="Поділіться враженнями про товар..."
                    />
                  </div>
                  {formError && <p className="text-xs text-red-500">{formError}</p>}
                  <div className="flex gap-3">
                    <button onClick={handleSubmitReview} disabled={formLoading} className="bg-green-600 text-white px-4 py-2 rounded text-sm font-medium hover:bg-green-700 transition-colors disabled:opacity-60">
                      {formLoading ? 'Збереження...' : 'Зберегти'}
                    </button>
                    <button onClick={cancelForm} className="text-sm text-gray-400 hover:text-red-500 transition-colors">Скасувати</button>
                  </div>
                </div>
              )}

              {reviewsLoading ? (
                <p className="text-sm text-gray-400">Завантаження відгуків...</p>
              ) : reviews.length === 0 ? (
                <p className="text-sm text-gray-400">Відгуків поки немає</p>
              ) : (
                <div className="flex flex-col gap-4">
                  {reviews.map(review => (
                    <div key={review.id} className="border border-gray-200 rounded-lg p-4 flex flex-col gap-2">
                      <div className="flex items-start justify-between gap-4">
                        <div className="flex flex-col gap-1">
                          <p className="text-sm font-medium text-gray-800">{review.firstName} {review.lastName}</p>
                          {review.isPending && (
                            <span className="text-xs text-orange-500 bg-orange-50 border border-orange-200 rounded px-2 py-0.5 w-fit">На модерації</span>
                          )}
                          <StarRating value={review.rating} />
                        </div>
                        <div className="flex items-center gap-3 shrink-0">
                          <p className="text-xs text-gray-400">{formatDateTime(review.createdAt)}</p>
                          {account && review.accountId === account.id && (
                            <div className="flex gap-2">
                              <button onClick={() => openEditForm(review)} className="text-gray-400 hover:text-green-600 transition-colors"><Pencil size={14} /></button>
                              <button onClick={() => handleDeleteReview(review.id)} className="text-gray-400 hover:text-red-500 transition-colors"><Trash2 size={14} /></button>
                            </div>
                          )}
                        </div>
                      </div>
                      {review.text && <p className="text-sm text-gray-600">{review.text}</p>}
                      {review.updatedAt && <p className="text-xs text-gray-400">Редаговано: {formatDateTime(review.updatedAt)}</p>}
                      {review.reply && (
                        <div className="bg-green-50 border border-green-100 rounded p-3 mt-1">
                          <p className="text-xs text-green-700 font-medium mb-1">Відповідь магазину:</p>
                          <p className="text-sm text-gray-700">{review.reply}</p>
                          {review.replyUpdatedAt
                            ? <p className="text-xs text-gray-400 mt-1">Редаговано: {formatDateTime(review.replyUpdatedAt)}</p>
                            : review.replyCreatedAt
                            ? <p className="text-xs text-gray-400 mt-1">{formatDateTime(review.replyCreatedAt)}</p>
                            : null}
                        </div>
                      )}
                    </div>
                  ))}
                  {totalPages > 1 && (
                    <div className="flex justify-center items-center gap-1 mt-4">
                      <button
                        onClick={() => setReviewsPage(p => Math.max(1, p - 1))}
                        disabled={reviewsPage === 1}
                        className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
                      >←</button>
                      {getPageNumbers().map((p, i) =>
                        p === '...' ? (
                          <span key={`dots-${i}`} className="px-3 py-2 text-gray-400">...</span>
                        ) : (
                          <button
                            key={p}
                            onClick={() => setReviewsPage(p as number)}
                            className={`px-3 py-2 rounded border transition-colors ${
                              reviewsPage === p
                                ? 'bg-green-600 text-white border-green-600'
                                : 'border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600'
                            }`}
                          >{p}</button>
                        )
                      )}
                      <button
                        onClick={() => setReviewsPage(p => Math.min(totalPages, p + 1))}
                        disabled={reviewsPage === totalPages}
                        className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
                      >→</button>
                    </div>
                  )}
                </div>
              )}
            </div>
          )}
        </div>

        {relatedProducts.length > 0 && (
          <ProductCarousel
            title="Вас також може зацікавити"
            products={relatedProducts.map(p => ({
              id: p.id, slug: p.slug, name: p.name, imageUrl: p.imageUrl,
              price: p.hasDiscount && p.discountPrice ? p.discountPrice : p.price,
              oldPrice: p.hasDiscount && p.discountPrice ? p.price : undefined,
              rating: p.rating, reviewsCount: p.reviewCount, inStock: p.quantity > 0,
            }))}
          />
        )}

        {recentlyViewed.filter(i => i.id !== product.id).length > 0 && (
          <ProductCarousel
            title="Ви переглядали"
            products={recentlyViewed.filter(i => i.id !== product.id)}
          />
        )}

      </div>

      {lightboxOpen && (
        <div className="fixed inset-0 z-50 bg-black/80 flex items-center justify-center p-4" onClick={() => setLightboxOpen(false)}>
          <img src={product.imageUrls[selectedImage]} alt={product.name} className="max-w-full max-h-full object-contain rounded-lg" onClick={e => e.stopPropagation()} />
        </div>
      )}
    </Layout>
  );
}
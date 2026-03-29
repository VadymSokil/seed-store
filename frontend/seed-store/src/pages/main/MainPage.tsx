import { useEffect, useState } from 'react';
import Layout from '../../components/layout/Layout';
import ProductCarousel from '../../components/product/ProductCarousel';
import type { TopProductsResponse, TopProduct } from '../../api/productsApi';
import { getTopProducts } from '../../api/productsApi';

const mapProduct = (p: TopProduct) => ({
  id: p.id,
  name: p.name,
  imageUrl: p.imageUrl,
  price: p.hasDiscount && p.discountPrice ? p.discountPrice : p.price,
  oldPrice: p.hasDiscount ? p.price : undefined,
  rating: p.rating,
  reviewsCount: p.reviewCount,
  inStock: p.quantity > 0,
});

const MainPage = () => {
  const [tops, setTops] = useState<TopProductsResponse | null>(null);

  useEffect(() => {
    getTopProducts().then(setTops).catch(console.error);
  }, []);

  if (!tops) return <Layout><div className="py-10 text-center text-gray-400">Завантаження...</div></Layout>;

  return (
    <Layout>
      <ProductCarousel title="Новинки" products={tops.newest.map(mapProduct)} />
      <ProductCarousel title="Популярні" products={tops.popular.map(mapProduct)} />
      <ProductCarousel title="Обговорювані" products={tops.mostDiscussed.map(mapProduct)} />
    </Layout>
  );
}

export default MainPage;
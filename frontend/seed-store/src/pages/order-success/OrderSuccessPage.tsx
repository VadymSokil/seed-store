import { useEffect } from 'react';
import { useLocation, useNavigate, useSearchParams } from 'react-router-dom';
import Layout from '../../components/layout/Layout';
import { useCartStore } from '../../store/useCartStore';

export default function OrderSuccessPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const [searchParams] = useSearchParams();
  const clearItems = useCartStore(s => s.clearItems);
  const orderNumber = (location.state?.orderNumber as string | undefined)
    ?? searchParams.get('orderNumber')
    ?? undefined;

  useEffect(() => {
    clearItems();
  }, []);

  return (
    <Layout>
      <div className="py-16 flex flex-col items-center text-center gap-4">
        <div className="w-16 h-16 rounded-full bg-green-100 flex items-center justify-center">
          <span className="text-green-600 text-3xl">✓</span>
        </div>
        <h1 className="text-xl font-bold text-gray-800">Дякуємо за замовлення!</h1>
        {orderNumber && (
          <p className="text-sm text-gray-500">
            Номер вашого замовлення: <span className="font-semibold text-gray-700">{orderNumber}</span>
          </p>
        )}
        <p className="text-sm text-gray-500 max-w-sm">
          Наш менеджер зв'яжеться з вами найближчим часом для уточнення деталей замовлення.
        </p>
        <button
          onClick={() => navigate('/')}
          className="mt-4 bg-green-600 text-white px-6 py-2 rounded hover:bg-green-700 transition-colors text-sm font-medium"
        >
          На головну
        </button>
      </div>
    </Layout>
  );
}
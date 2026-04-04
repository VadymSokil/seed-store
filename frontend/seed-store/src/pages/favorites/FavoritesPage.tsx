import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Heart, X } from 'lucide-react';
import Layout from '../../components/layout/Layout';
import { useFavoritesStore } from '../../store/useFavoritesStore';

export default function FavoritesPage() {
  const navigate = useNavigate();
  const { items, removeItem, clearItems  } = useFavoritesStore();
  const [search, setSearch] = useState('');

  const filtered = items.filter(item =>
    item.name.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <Layout>
      <div className="py-8">
  <nav className="flex flex-wrap items-center gap-2 text-sm mb-6 text-gray-500">
    <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
    <span>/</span>
    <span className="text-gray-800 font-medium">Обране</span>
  </nav>

  <div className="mb-6">
    <div className="flex items-center justify-between mb-3">
      <h1 className="text-xl font-bold text-gray-800">Обране</h1>
      <button
        onClick={clearItems}
        className="text-sm text-gray-400 hover:text-red-500 transition-colors"
      >
        Очистити
      </button>
    </div>
    {items.length > 0 && (
      <input
        type="text"
        placeholder="Пошук в обраному..."
        value={search}
        onChange={e => setSearch(e.target.value)}
        className="border border-gray-300 rounded px-3 py-1.5 text-sm outline-none focus:border-green-600 w-full"
      />
    )}
  </div>

        {items.length === 0 ? (
          <div className="flex flex-col items-center gap-3 py-16 text-gray-400">
            <Heart size={48} strokeWidth={1.5} />
            <p>Список обраного порожній</p>
          </div>
        ) : filtered.length === 0 ? (
          <p className="text-gray-500">Нічого не знайдено</p>
        ) : (
          <div className="flex flex-col divide-y divide-gray-100 border border-gray-200 rounded-lg overflow-hidden">
            {filtered.map(item => (
              <div key={item.id} className="flex items-center gap-4 px-4 py-3 hover:bg-gray-50">
                <img
                  src={item.imageUrl}
                  alt={item.name}
                  className="w-14 h-14 object-contain rounded cursor-pointer"
                  onClick={() => navigate(`/product/${item.slug}`)}
                />
                <div
                  className="flex-1 min-w-0 cursor-pointer"
                  onClick={() => navigate(`/product/${item.slug}`)}
                >
                  <p className="text-sm font-medium line-clamp-1 hover:text-green-700">{item.name}</p>
                  <p className="text-sm text-green-700 font-bold">{item.price} ₴</p>
                  <span className={`text-xs ${item.inStock ? 'text-green-600' : 'text-red-500'}`}>
                    {item.inStock ? 'В наявності' : 'Немає в наявності'}
                  </span>
                </div>
                <button
                  onClick={() => removeItem(item.id)}
                  className="text-gray-300 hover:text-red-400 transition-colors shrink-0"
                >
                  <X size={16} />
                </button>
              </div>
            ))}
          </div>
        )}
      </div>
    </Layout>
  );
}
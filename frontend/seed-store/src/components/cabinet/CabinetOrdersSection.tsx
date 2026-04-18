import { useState, useEffect } from 'react';
import { getOrders } from '../../api/ordersApi';
import type { Order } from '../../api/ordersApi';
import OrderDetailsModal from '../modals/OrderDetailsModal';

const PAGE_SIZE = 5;

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('uk-UA', { day: '2-digit', month: '2-digit', year: 'numeric' });

const getPageNumbers = (page: number, totalPages: number) => {
  if (totalPages <= 5) return Array.from({ length: totalPages }, (_, i) => i + 1);
  if (page <= 3) return [1, 2, 3, 4, '...', totalPages];
  if (page >= totalPages - 2) return [1, '...', totalPages - 3, totalPages - 2, totalPages - 1, totalPages];
  return [1, '...', page - 1, page, page + 1, '...', totalPages];
};

export default function CabinetOrdersSection() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  const totalPages = Math.ceil(totalCount / PAGE_SIZE);

  useEffect(() => {
    setIsLoading(true);
    getOrders(page)
      .then(data => {
        setOrders(data.items);
        setTotalCount(data.totalCount);
      })
      .catch(console.error)
      .finally(() => setIsLoading(false));
  }, [page]);

  return (
    <>
      {selectedOrder && <OrderDetailsModal order={selectedOrder} onClose={() => setSelectedOrder(null)} />}

      {isLoading ? (
        <p className="text-sm text-gray-400">Завантаження...</p>
      ) : orders.length === 0 ? (
        <p className="text-sm text-gray-400">Замовлень поки немає</p>
      ) : (
        <>
          <div className="flex flex-col gap-3">
            {orders.map(order => (
              <div
                key={order.id}
                onClick={() => setSelectedOrder(order)}
                className="border border-gray-200 rounded-lg p-4 cursor-pointer hover:border-green-400 transition-colors"
              >
                <div className="flex items-center justify-between mb-1">
                  <p className="text-sm font-medium text-gray-800">{order.orderNumber}</p>
                  <p className="text-xs text-gray-400">{formatDate(order.orderDate)}</p>
                </div>
                <div className="flex items-center justify-between">
                  <p className="text-xs text-gray-500">{order.statusName}</p>
                  <p className="text-sm font-semibold text-green-700">{order.totalAmount} ₴</p>
                </div>
              </div>
            ))}
          </div>

          {totalPages > 1 && (
            <div className="flex justify-center items-center gap-1 mt-6">
              <button
                onClick={() => setPage(p => p - 1)}
                disabled={page === 1}
                className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
              >
                ←
              </button>
              {getPageNumbers(page, totalPages).map((p, i) =>
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
    </>
  );
}
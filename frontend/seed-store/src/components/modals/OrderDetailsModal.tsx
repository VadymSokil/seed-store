import { useRef } from 'react';
import { X } from 'lucide-react';
import type { Order } from '../../api/ordersApi';
import { getPaymentData } from '../../api/ordersApi';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';

interface Props {
  order: Order;
  onClose: () => void;
}

const formatDate = (iso: string) =>
  new Date(iso).toLocaleDateString('uk-UA', { day: '2-digit', month: '2-digit', year: 'numeric' });

const deliveryLabel: Record<string, string> = {
  nova_poshta: 'Нова Пошта - відділення',
  courier: 'Нова пошта - кур\'єр',
};

const paymentLabel: Record<string, string> = {
  cash_on_delivery: 'Накладений платіж',
  online: 'Онлайн оплата',
};

export default function OrderDetailsModal({ order, onClose }: Props) {
  const liqpayFormRef = useRef<HTMLFormElement>(null);
  const liqpayDataRef = useRef<HTMLInputElement>(null);
  const liqpaySignatureRef = useRef<HTMLInputElement>(null);

  useModalScrollLock(true);

  const isPaid = order.transactions.some(t => t.isSuccess);

  const handlePay = async () => {
    try {
      const response = await getPaymentData(order.orderNumber);
      if (liqpayFormRef.current && liqpayDataRef.current && liqpaySignatureRef.current) {
        liqpayDataRef.current.value = response.data;
        liqpaySignatureRef.current.value = response.signature;
        liqpayFormRef.current.submit();
      }
    } catch {
      // помилка
    }
  };

  const settlementName = order.settlement ? `с. ${order.settlement}` : order.city ? `м. ${order.city}` : null;

  const address = order.deliveryCode === 'nova_poshta'
    ? [settlementName, order.district ? `${order.district} р-н` : null, order.region ? `${order.region} обл.` : null, order.postalOfficeNumber ? `відд. ${order.postalOfficeNumber}` : null].filter(Boolean).join(', ')
    : [settlementName, order.district ? `${order.district} р-н` : null, order.region ? `${order.region} обл.` : null, order.street, order.houseNumber ? `буд. ${order.houseNumber}` : null, order.apartmentNumber ? `кв. ${order.apartmentNumber}` : null].filter(Boolean).join(', ');

  const rowClass = 'flex justify-between gap-4 py-1.5 border-b border-gray-50 last:border-0';
  const labelClass = 'text-sm text-gray-400 shrink-0';
  const valueClass = 'text-sm text-gray-700 text-right';

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 px-4">
      <form ref={liqpayFormRef} method="POST" action="https://www.liqpay.ua/api/3/checkout" style={{ display: 'none' }}>
        <input ref={liqpayDataRef} type="hidden" name="data" />
        <input ref={liqpaySignatureRef} type="hidden" name="signature" />
      </form>

      <div className="bg-white rounded-xl w-full max-w-lg max-h-[90vh] flex flex-col">

        <div className="flex items-center justify-between p-6 pb-4">
          <p className="text-sm font-semibold text-gray-800">Замовлення {order.orderNumber}</p>
          <button onClick={onClose} className="text-gray-400 hover:text-gray-600 transition-colors">
            <X size={18} />
          </button>
        </div>

        <div className="overflow-y-auto flex-1 px-6 flex flex-col gap-5">

          <div className="flex flex-col">
            <div className={rowClass}>
              <span className={labelClass}>Дата замовлення</span>
              <span className={valueClass}>{formatDate(order.orderDate)}</span>
            </div>
            <div className={rowClass}>
              <span className={labelClass}>Статус</span>
              <span className={valueClass}>{order.statusName}</span>
            </div>
            <div className={rowClass}>
              <span className={labelClass}>Доставка</span>
              <span className={valueClass}>{deliveryLabel[order.deliveryCode] ?? order.deliveryCode}</span>
            </div>
            <div className={rowClass}>
              <span className={labelClass}>Оплата</span>
              <span className={valueClass}>{paymentLabel[order.paymentCode] ?? order.paymentCode}</span>
            </div>
            <div className={rowClass}>
              <span className={labelClass}>Статус оплати</span>
              <span className={isPaid ? 'text-sm text-green-600 text-right' : 'text-sm text-gray-400 text-right'}>
                {isPaid ? 'Оплачено' : 'Не оплачено'}
              </span>
            </div>
            {order.paidAt && (
              <div className={rowClass}>
                <span className={labelClass}>Дата оплати</span>
                <span className={valueClass}>{formatDate(order.paidAt)}</span>
              </div>
            )}
            {order.trackingNumber && (
              <div className={rowClass}>
                <span className={labelClass}>ТТН</span>
                <span className={valueClass}>{order.trackingNumber}</span>
              </div>
            )}
            {address && (
              <div className={rowClass}>
                <span className={labelClass}>Адреса</span>
                <span className={valueClass}>{address}</span>
              </div>
            )}
            <div className={rowClass}>
              <span className={labelClass}>Коментар</span>
              <span className={valueClass}>{order.comment || '—'}</span>
            </div>
            <div className={rowClass}>
              <span className={labelClass}>Коментар менеджера</span>
              <span className={valueClass}>{order.customerComment || '—'}</span>
            </div>
          </div>

          <div>
            <p className="text-xs text-gray-400 uppercase tracking-wide mb-2">Товари</p>
            <div className="flex flex-col gap-2">
              {order.items.map(item => (
                <div key={item.productId} className="flex items-center gap-2">
                  {item.productImageUrlSnapshot && (
                    <img src={item.productImageUrlSnapshot} alt={item.productNameSnapshot} className="w-10 h-10 object-contain rounded" />
                  )}
                  <div className="flex-1 min-w-0">
                    <p className="text-xs text-gray-700 line-clamp-2">{item.productNameSnapshot}</p>
                    <p className="text-xs text-gray-400">{item.quantity} шт × {item.price} ₴</p>
                  </div>
                  <p className="text-xs font-medium text-gray-700 shrink-0">{item.price * item.quantity} ₴</p>
                </div>
              ))}
            </div>
          </div>

          {order.paymentCode === 'online' && (
            <div>
              <p className="text-xs text-gray-400 uppercase tracking-wide mb-2">Транзакції</p>
              {order.transactions.length === 0 ? (
                <p className="text-sm text-gray-400">Транзакцій немає</p>
              ) : (
                <div className="flex flex-col">
                  <div className="flex gap-2 text-xs text-gray-400 pb-1 border-b border-gray-100">
                    <span className="flex-1">Дата</span>
                    <span className="w-24 text-right">Сума</span>
                    <span className="w-20 text-right">Статус</span>
                  </div>
                  {order.transactions.map((t, i) => (
                    <div key={i} className="flex gap-2 text-xs py-1.5 border-b border-gray-50 last:border-0">
                      <span className="flex-1 text-gray-700">{formatDate(t.createdAt)}</span>
                      <span className="w-24 text-right text-gray-700">{t.amount} {t.currency}</span>
                      <span className={`w-20 text-right ${t.isSuccess ? 'text-green-600' : 'text-red-500'}`}>
                        {t.isSuccess ? 'Успішно' : 'Помилка'}
                      </span>
                    </div>
                  ))}
                </div>
              )}
            </div>
          )}

          <div className="pb-1" />
        </div>

        <div className="p-6 pt-4 border-t border-gray-100 flex flex-col gap-3">
          <div className="flex justify-between items-center">
            <p className="text-sm text-gray-600">Разом:</p>
            <p className="text-lg font-bold text-green-700">{order.totalAmount} ₴</p>
          </div>
          {order.paymentCode === 'online' && !isPaid && (
            <button
              onClick={handlePay}
              className="w-full bg-green-600 text-white py-2.5 rounded font-medium text-sm hover:bg-green-700 transition-colors"
            >
              Оплатити замовлення
            </button>
          )}
        </div>

      </div>
    </div>
  );
}
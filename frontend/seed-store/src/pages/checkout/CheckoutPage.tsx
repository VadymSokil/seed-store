import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../../components/layout/Layout';
import { useAuthStore } from '../../store/useAuthStore';
import { useCartStore } from '../../store/useCartStore';
import { getDeliveryVariants, getPaymentVariants } from '../../api/storeInfoApi';
import type { StoreInfoVariant } from '../../api/storeInfoApi';
import { addOrder } from '../../api/ordersApi';
import AutocompleteInput from '../../components/ui/AutocompleteInput';
import { searchSettlements, searchWarehouses } from '../../api/novaPoshtaApi';
import type { NovaPoshtaSettlement, NovaPoshtaWarehouse } from '../../api/novaPoshtaApi';

export default function CheckoutPage() {
  const navigate = useNavigate();
  const account = useAuthStore(s => s.account);
  const { items, count } = useCartStore();
  const clearItems = useCartStore(s => s.clearItems);

  const [deliveryVariants, setDeliveryVariants] = useState<StoreInfoVariant[]>([]);
  const [paymentVariants, setPaymentVariants] = useState<StoreInfoVariant[]>([]);

  const [firstName, setFirstName] = useState(account?.firstName ?? '');
  const [lastName, setLastName] = useState(account?.lastName ?? '');
  const [middleName, setMiddleName] = useState(account?.middleName ?? '');
  const [phoneNumber, setPhoneNumber] = useState(account?.phoneNumber ?? '');
  const [deliveryCode, setDeliveryCode] = useState('');
  const [paymentCode, setPaymentCode] = useState('');
  const [street, setStreet] = useState('');
  const [houseNumber, setHouseNumber] = useState('');
  const [apartmentNumber, setApartmentNumber] = useState('');
  const [comment, setComment] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const [settlementQuery, setSettlementQuery] = useState('');
  const [settlementOptions, setSettlementOptions] = useState<NovaPoshtaSettlement[]>([]);
  const [selectedSettlement, setSelectedSettlement] = useState<NovaPoshtaSettlement | null>(null);
  const [settlementsLoading, setSettlementsLoading] = useState(false);

  const [warehouseQuery, setWarehouseQuery] = useState('');
  const [warehouseOptions, setWarehouseOptions] = useState<NovaPoshtaWarehouse[]>([]);
  const [selectedWarehouse, setSelectedWarehouse] = useState<NovaPoshtaWarehouse | null>(null);
  const [warehousesLoading, setWarehousesLoading] = useState(false);

  const liqpayFormRef = useRef<HTMLFormElement>(null);
  const liqpayDataRef = useRef<HTMLInputElement>(null);
  const liqpaySignatureRef = useRef<HTMLInputElement>(null);

  const total = items.reduce((sum, item) => sum + item.price * item.quantity, 0);

  useEffect(() => {
    getDeliveryVariants().then(setDeliveryVariants).catch(console.error);
    getPaymentVariants().then(setPaymentVariants).catch(console.error);
  }, []);

  useEffect(() => {
    if (deliveryVariants.length > 0 && !deliveryCode) {
      setDeliveryCode(deliveryVariants[0].code);
    }
  }, [deliveryVariants]);

  useEffect(() => {
    if (paymentVariants.length > 0 && !paymentCode) {
      setPaymentCode(paymentVariants[0].code);
    }
  }, [paymentVariants]);

  useEffect(() => {
    if (settlementQuery.length < 2) { setSettlementOptions([]); return; }
    const timer = setTimeout(async () => {
      setSettlementsLoading(true);
      try {
        const results = await searchSettlements(settlementQuery);
        setSettlementOptions(results);
      } finally {
        setSettlementsLoading(false);
      }
    }, 300);
    return () => clearTimeout(timer);
  }, [settlementQuery]);

  useEffect(() => {
    if (!selectedSettlement) { setWarehouseOptions([]); return; }
    const timer = setTimeout(async () => {
      setWarehousesLoading(true);
      try {
        const results = await searchWarehouses(selectedSettlement.id, warehouseQuery || undefined);
        setWarehouseOptions(results);
      } finally {
        setWarehousesLoading(false);
      }
    }, 300);
    return () => clearTimeout(timer);
  }, [selectedSettlement, warehouseQuery]);

  const handleSelectSettlement = (item: NovaPoshtaSettlement) => {
    setSelectedSettlement(item);
    setSettlementQuery(item.name);
    setSelectedWarehouse(null);
    setWarehouseQuery('');
  };

  const handleSelectWarehouse = (item: NovaPoshtaWarehouse) => {
    setSelectedWarehouse(item);
    setWarehouseQuery(item.name);
  };

  const handleDeliveryChange = (code: string) => {
    setDeliveryCode(code);
    setSelectedSettlement(null);
    setSettlementQuery('');
    setSelectedWarehouse(null);
    setWarehouseQuery('');
  };

  const handleSubmit = async () => {
    setError('');
    if (!phoneNumber) { setError('Введіть номер телефону'); return; }
    if (!deliveryCode) { setError('Оберіть спосіб доставки'); return; }
    if (!paymentCode) { setError('Оберіть спосіб оплати'); return; }
    if (deliveryCode === 'nova_poshta') {
      if (!selectedSettlement) { setError('Оберіть населений пункт'); return; }
      if (!selectedWarehouse) { setError('Оберіть відділення або поштомат'); return; }
    }
    if (deliveryCode === 'courier') {
      if (!selectedSettlement) { setError('Оберіть населений пункт'); return; }
      if (!street || !houseNumber) { setError('Введіть адресу доставки'); return; }
    }
    if (items.length === 0) { setError('Кошик порожній'); return; }

    setIsLoading(true);
    try {
      const response = await addOrder({
        firstName: firstName || undefined,
        lastName: lastName || undefined,
        middleName: middleName || undefined,
        phoneNumber,
        deliveryCode,
        paymentCode,
        postalOfficeNumber: selectedWarehouse?.digitalAddress || undefined,
        region: selectedSettlement?.region || undefined,
        district: selectedSettlement?.district || undefined,
        city: selectedSettlement?.type === 'м.' ? selectedSettlement.name : undefined,
        settlement: selectedSettlement?.type !== 'м.' ? selectedSettlement?.name : undefined,
        street: street || undefined,
        houseNumber: houseNumber || undefined,
        apartmentNumber: apartmentNumber || undefined,
        comment: comment || undefined,
        items: items.map(i => ({
          productId: i.id,
          quantity: i.quantity,
          price: i.price,
        })),
      });
      if (response.data && response.signature && liqpayFormRef.current && liqpayDataRef.current && liqpaySignatureRef.current) {
        liqpayDataRef.current.value = response.data;
        liqpaySignatureRef.current.value = response.signature;
        liqpayFormRef.current.submit();
      } else {
        clearItems();
        navigate('/order-success', { state: { orderNumber: response.orderNumber } });
      }
    } catch {
      setError('Помилка оформлення замовлення. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const inputClass = 'w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600';
  const labelClass = 'text-xs text-gray-400 uppercase tracking-wide mb-1 block';

  return (
    <Layout>
      <div className="py-8">

        <form
          ref={liqpayFormRef}
          method="POST"
          action="https://www.liqpay.ua/api/3/checkout"
          style={{ display: 'none' }}
        >
          <input ref={liqpayDataRef} type="hidden" name="data" />
          <input ref={liqpaySignatureRef} type="hidden" name="signature" />
        </form>

        <nav className="flex items-center gap-2 text-sm mb-6 text-gray-500">
          <span className="cursor-pointer hover:text-green-600" onClick={() => navigate('/')}>Головна</span>
          <span>/</span>
          <span className="text-gray-800 font-medium">Оформлення замовлення</span>
        </nav>

        <h1 className="text-xl font-bold text-gray-800 mb-6">Оформлення замовлення</h1>

        {count === 0 ? (
          <p className="text-gray-500">Кошик порожній</p>
        ) : (
          <div className="flex flex-col lg:flex-row gap-8">

            <div className="flex-1 flex flex-col gap-6">

              <div className="border border-gray-200 rounded-lg p-4 flex flex-col gap-3">
                <p className="text-sm font-semibold text-gray-700">Контактні дані</p>
                <div>
                  <label className={labelClass}>Прізвище</label>
                  <input value={lastName} onChange={e => setLastName(e.target.value)} className={inputClass} />
                </div>
                <div>
                  <label className={labelClass}>Ім'я</label>
                  <input value={firstName} onChange={e => setFirstName(e.target.value)} className={inputClass} />
                </div>
                <div>
                  <label className={labelClass}>По батькові</label>
                  <input value={middleName} onChange={e => setMiddleName(e.target.value)} className={inputClass} />
                </div>
                <div>
                  <label className={labelClass}>Телефон *</label>
                  <input type="tel" placeholder="+380XXXXXXXXX" value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)} className={inputClass} />
                </div>
              </div>

              <div className="border border-gray-200 rounded-lg p-4 flex flex-col gap-3">
                <p className="text-sm font-semibold text-gray-700">Спосіб доставки</p>
                <div className="flex flex-col gap-2">
                  {deliveryVariants.map(v => (
                    <label key={v.code} className="flex items-start gap-3 cursor-pointer">
                      <input
                        type="radio"
                        name="delivery"
                        value={v.code}
                        checked={deliveryCode === v.code}
                        onChange={() => handleDeliveryChange(v.code)}
                        className="accent-green-600 mt-0.5"
                      />
                      <div>
                        <p className="text-sm font-medium text-gray-700">{v.name}</p>
                        <p className="text-xs text-gray-400">{v.description}</p>
                      </div>
                    </label>
                  ))}
                </div>

                {deliveryCode === 'nova_poshta' && (
                  <div className="flex flex-col gap-3">
                    <div>
                      <label className={labelClass}>Населений пункт *</label>
                      <AutocompleteInput<NovaPoshtaSettlement>
                        value={settlementQuery}
                        onChange={v => { setSettlementQuery(v); setSelectedSettlement(null); }}
                        onSelect={handleSelectSettlement}
                        options={settlementOptions}
                        getLabel={i => `${i.type} ${i.name}, ${i.district} р-н, ${i.region} обл.`}
                        placeholder="Введіть назву міста або села"
                        isLoading={settlementsLoading}
                      />
                    </div>
                    <div>
                      <label className={labelClass}>Відділення або поштомат *</label>
                      <AutocompleteInput<NovaPoshtaWarehouse>
                        value={warehouseQuery}
                        onChange={v => { setWarehouseQuery(v); setSelectedWarehouse(null); }}
                        onSelect={handleSelectWarehouse}
                        options={warehouseOptions}
                        getLabel={i => i.name}
                        placeholder="Введіть номер або адресу відділення"
                        disabled={!selectedSettlement}
                        isLoading={warehousesLoading}
                      />
                    </div>
                  </div>
                )}

                {deliveryCode === 'courier' && (
                  <div className="flex flex-col gap-3">
                    <div>
                      <label className={labelClass}>Населений пункт *</label>
                      <AutocompleteInput<NovaPoshtaSettlement>
                        value={settlementQuery}
                        onChange={v => { setSettlementQuery(v); setSelectedSettlement(null); }}
                        onSelect={handleSelectSettlement}
                        options={settlementOptions}
                        getLabel={i => `${i.type} ${i.name}, ${i.district} р-н, ${i.region} обл.`}
                        placeholder="Введіть назву міста або села"
                        isLoading={settlementsLoading}
                      />
                    </div>
                    <div>
                      <label className={labelClass}>Вулиця *</label>
                      <input value={street} onChange={e => setStreet(e.target.value)} className={inputClass} />
                    </div>
                    <div className="flex gap-3">
                      <div className="flex-1">
                        <label className={labelClass}>Номер будинку *</label>
                        <input value={houseNumber} onChange={e => setHouseNumber(e.target.value)} className={inputClass} />
                      </div>
                      <div className="flex-1">
                        <label className={labelClass}>Квартира</label>
                        <input value={apartmentNumber} onChange={e => setApartmentNumber(e.target.value)} className={inputClass} />
                      </div>
                    </div>
                  </div>
                )}
              </div>

              <div className="border border-gray-200 rounded-lg p-4 flex flex-col gap-3">
                <p className="text-sm font-semibold text-gray-700">Спосіб оплати</p>
                <div className="flex flex-col gap-2">
                  {paymentVariants.map(v => (
                    <label key={v.code} className="flex items-start gap-3 cursor-pointer">
                      <input
                        type="radio"
                        name="payment"
                        value={v.code}
                        checked={paymentCode === v.code}
                        onChange={() => setPaymentCode(v.code)}
                        className="accent-green-600 mt-0.5"
                      />
                      <div>
                        <p className="text-sm font-medium text-gray-700">{v.name}</p>
                        <p className="text-xs text-gray-400">{v.description}</p>
                      </div>
                    </label>
                  ))}
                </div>
              </div>

              <div className="border border-gray-200 rounded-lg p-4">
                <label className={labelClass}>Коментар до замовлення</label>
                <textarea
                  value={comment}
                  onChange={e => setComment(e.target.value)}
                  rows={3}
                  className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 resize-none"
                />
              </div>

            </div>

            <div className="lg:w-80 shrink-0">
              <div className="border border-gray-200 rounded-lg p-4 flex flex-col gap-3 sticky top-24">
                <p className="text-sm font-semibold text-gray-700">Ваше замовлення</p>
                <div className="flex flex-col gap-2">
                  {items.map(item => (
                    <div key={item.id} className="flex items-center gap-2">
                      <img src={item.imageUrl} alt={item.name} className="w-10 h-10 object-contain rounded" />
                      <div className="flex-1 min-w-0">
                        <p className="text-xs text-gray-700 line-clamp-2">{item.name}</p>
                        <p className="text-xs text-gray-400">{item.quantity} шт × {item.price} ₴</p>
                      </div>
                      <p className="text-xs font-medium text-gray-700 shrink-0">{item.price * item.quantity} ₴</p>
                    </div>
                  ))}
                </div>
                <div className="border-t border-gray-100 pt-3 flex justify-between items-center">
                  <p className="text-sm text-gray-600">Разом:</p>
                  <p className="text-lg font-bold text-green-700">{total} ₴</p>
                </div>
                {error && <p className="text-xs text-red-500">{error}</p>}
                <button
                  onClick={handleSubmit}
                  disabled={isLoading}
                  className="w-full bg-green-600 text-white py-2.5 rounded font-medium text-sm hover:bg-green-700 transition-colors disabled:opacity-60 disabled:cursor-not-allowed"
                >
                  {isLoading ? 'Завантаження...' : 'Підтвердити замовлення'}
                </button>
              </div>
            </div>

          </div>
        )}
      </div>
    </Layout>
  );
}
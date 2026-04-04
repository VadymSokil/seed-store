import api from './axiosInstance';

export interface StoreInfoVariant {
  id: number;
  code: string;
  name: string;
  description: string;
  viewOrder: number;
}

export const getDeliveryVariants = (): Promise<StoreInfoVariant[]> =>
  api.get<StoreInfoVariant[]>('/api/store-info/delivery-variants').then(r => r.data);

export const getPaymentVariants = (): Promise<StoreInfoVariant[]> =>
  api.get<StoreInfoVariant[]>('/api/store-info/payment-variants').then(r => r.data);
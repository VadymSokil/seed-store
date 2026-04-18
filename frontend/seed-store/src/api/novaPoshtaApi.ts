import api from './axiosInstance';

export interface NovaPoshtaSettlement {
  id: string;
  name: string;
  region: string;
  district: string;
  type: string;
}

export interface NovaPoshtaWarehouse {
  id: string;
  name: string;
  digitalAddress: string;
}

export const searchSettlements = (value: string): Promise<NovaPoshtaSettlement[]> =>
  api.get<NovaPoshtaSettlement[]>(`/api/orders/nova-poshta/settlements?value=${encodeURIComponent(value)}`).then(res => res.data);

export const searchWarehouses = (settlementId: string, value?: string): Promise<NovaPoshtaWarehouse[]> => {
  const params = new URLSearchParams({ settlementId });
  if (value) params.append('value', value);
  return api.get<NovaPoshtaWarehouse[]>(`/api/orders/nova-poshta/warehouses?${params}`).then(res => res.data);
};
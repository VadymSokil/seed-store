import api from './axiosInstance';

export interface OrderItemRequest {
  productId: number;
  quantity: number;
  price: number;
}

export interface AddOrderRequest {
  firstName?: string;
  lastName?: string;
  middleName?: string;
  phoneNumber: string;
  deliveryCode: string;
  paymentCode: string;
  postalOfficeNumber?: string;
  region?: string;
  district?: string;
  city?: string;
  settlement?: string;
  street?: string;
  houseNumber?: string;
  apartmentNumber?: string;
  comment?: string;
  customerComment?: string;
  items: OrderItemRequest[];
}

export interface AddOrderResponse {
  orderNumber: string;
  data?: string;
  signature?: string;
}

export const addOrder = (data: AddOrderRequest): Promise<AddOrderResponse> =>
  api.post<AddOrderResponse>('/api/orders', data).then(r => r.data);

export interface OrderTransaction {
  amount: number;
  currency: string;
  createdAt: string;
  isSuccess: boolean;
}

export interface OrderItem {
  productId: number;
  productNameSnapshot: string;
  productImageUrlSnapshot: string | null;
  quantity: number;
  price: number;
}

export interface Order {
  id: number;
  orderNumber: string;
  orderDate: string;
  statusName: string;
  comment: string | null;
  customerComment: string | null;
  deliveryCode: string;
  paymentCode: string;
  postalOfficeNumber: string | null;
  trackingNumber: string | null;
  totalAmount: number;
  firstName: string;
  lastName: string;
  middleName: string | null;
  phoneNumber: string;
  region: string | null;
  district: string | null;
  city: string | null;
  settlement: string | null;
  street: string | null;
  houseNumber: string | null;
  apartmentNumber: string | null;
  paidAt: string | null;
  items: OrderItem[];
  transactions: OrderTransaction[];
}

export interface OrdersResponse {
  totalCount: number;
  items: Order[];
}

export const getOrders = (page: number): Promise<OrdersResponse> =>
  api.get<OrdersResponse>('/api/orders', { params: { page, pageSize: 5 } }).then(r => r.data);

export interface GetPaymentDataResponse {
  data: string;
  signature: string;
}

export const getPaymentData = (orderNumber: string): Promise<GetPaymentDataResponse> =>
  api.post<GetPaymentDataResponse>(`/api/orders/${orderNumber}/pay`).then(r => r.data);





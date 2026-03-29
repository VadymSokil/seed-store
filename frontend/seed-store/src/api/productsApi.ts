import api from './axiosInstance';

export interface TopProduct {
  id: number;
  categoryId: number;
  article: string;
  name: string;
  slug: string;
  description: string;
  price: number;
  hasDiscount: boolean;
  discountPrice: number | null;
  discountEndDate: string | null;
  quantity: number;
  rating: number;
  reviewCount: number;
  imageUrl: string;
}

export interface TopProductsResponse {
  newest: TopProduct[];
  popular: TopProduct[];
  mostDiscussed: TopProduct[];
}

export const getTopProducts = async (): Promise<TopProductsResponse> => {
  const response = await api.get<TopProductsResponse>('/api/products/top');
  return response.data;
}
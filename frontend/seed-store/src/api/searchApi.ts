import api from './axiosInstance';

export interface SearchProduct {
  name: string;
  price: number;
  imageUrl: string;
}

export interface SearchCategory {
  name: string;
}

export const searchProducts = async (value: string): Promise<SearchProduct[]> => {
  const response = await api.get<SearchProduct[]>('/api/products/search', { params: { value } });
  return response.data;
}

export const searchCategories = async (value: string): Promise<SearchCategory[]> => {
  const response = await api.get<SearchCategory[]>('/api/catalog/search', { params: { value } });
  return response.data;
}
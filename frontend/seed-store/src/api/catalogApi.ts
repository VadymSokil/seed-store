import axiosInstance from './axiosInstance';
import type { Category } from '../types/catalog';

export const getCategories = (): Promise<Category[]> =>
  axiosInstance.get<Category[]>('/api/catalog').then(res => res.data);
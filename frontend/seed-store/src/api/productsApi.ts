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

export interface ActiveFilter {
  featureSlug: string;
  valueSlug: string;
}

export interface ProductListRequest {
  categoryId: number | null;
  priceFrom: number | null;
  priceTo: number | null;
  hasDiscount: boolean | null;
  sortByPriceAsc: boolean | null;
  sortByPriceDesc: boolean | null;
  inStock: boolean | null;
  activeFilters: ActiveFilter[] | null;
  page: number;
  pageSize: number;
}

export interface ProductListResponse {
  items: TopProduct[];
  totalCount: number;
}

export const getProductList = (request: ProductListRequest): Promise<ProductListResponse> =>
  api.post<ProductListResponse>('/api/products/list', request).then(res => res.data);

export interface FilterValue {
  id: number;
  featureId: number;
  headerId: number;
  value: string;
  valueSlug: string;
  viewOrder: number;
}

export interface Feature {
  id: number;
  name: string;
  slug: string;
  viewOrder: number;
}

export interface ProductFiltersResponse {
  features: Feature[];
  headers: { id: number; name: string; viewOrder: number }[];
  filterValues: FilterValue[];
}

export const getProductFilters = (categoryId: number): Promise<ProductFiltersResponse> =>
  api.get<ProductFiltersResponse>(`/api/products/filters?categoryId=${categoryId}`).then(res => res.data);



export interface ProductFeature {
  featureId: number;
  featureName: string;
  featureSlug: string;
  headerId: number;
  headerName: string;
  value: string;
  valueSlug: string;
  viewOrder: number;
}

export interface ProductDetails {
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
  imageUrls: string[];
  features: ProductFeature[];
}

export const getProduct = (slugOrId: string): Promise<ProductDetails> =>
  api.get<ProductDetails>(`/api/products/${slugOrId}`).then(res => res.data);

export interface DiscountProduct {
  productId: number;
  name: string;
  slug: string;
  imageUrl: string;
  originalPrice: number;
  discountPrice: number;
  discountPercent: number;
}

export interface DiscountGroup {
  id: number;
  name: string;
  startDate: string;
  endDate: string;
  products: DiscountProduct[];
}

export const getActiveDiscountGroups = (): Promise<DiscountGroup[]> =>
  api.get<DiscountGroup[]>('/api/products/discounts').then(r => r.data);
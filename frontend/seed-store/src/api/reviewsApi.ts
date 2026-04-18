import api from './axiosInstance';

export interface ProductReview {
  id: number;
  accountId: number;
  firstName: string;
  lastName: string;
  rating: number;
  text: string | null;
  createdAt: string;
  updatedAt: string | null;
  reply: string | null;
  replyCreatedAt: string | null;
  replyUpdatedAt: string | null;
  isPending: boolean
}

export interface ProductReviewsResponse {
  reviews: ProductReview[];
  totalCount: number;
}

export const getProductReviews = (productId: number, page: number, pageSize: number): Promise<ProductReviewsResponse> =>
  api.get<ProductReviewsResponse>(`/api/reviews/product/${productId}?page=${page}&pageSize=${pageSize}`).then(r => r.data);

export interface AddReviewRequest {
  productId: number;
  rating: number;
  text?: string;
}

export const addReview = (data: AddReviewRequest): Promise<void> =>
  api.post('/api/reviews', data).then(r => r.data);

export interface UpdateReviewRequest {
  rating: number;
  text?: string;
}

export const updateReview = (reviewId: number, data: UpdateReviewRequest): Promise<void> =>
  api.put(`/api/reviews/${reviewId}`, data).then(r => r.data);

export const deleteReview = (reviewId: number): Promise<void> =>
  api.delete(`/api/reviews/${reviewId}`).then(r => r.data);

export interface AccountReview {
  id: number;
  productId: number;
  productNameSnapshot: string;
  productImageUrlSnapshot: string;
  rating: number;
  text: string | null;
  createdAt: string;
  updatedAt: string | null;
  statusCode: string;
  moderatorComment: string | null;
  reply: string | null;
  replyCreatedAt: string | null;
  replyUpdatedAt: string | null;
}

export interface AccountReviewsResponse {
  totalCount: number;
  items: AccountReview[];
}

export const getAccountReviews = (page: number): Promise<AccountReviewsResponse> =>
  api.get<AccountReviewsResponse>('/api/reviews/account', { params: { page, pageSize: 5 } }).then(r => r.data);

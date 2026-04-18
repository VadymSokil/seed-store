import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export interface RecentlyViewedItem {
  id: number;
  slug: string;
  name: string;
  imageUrl: string;
  price: number;
  oldPrice?: number;
  rating: number;
  reviewsCount: number;
  inStock: boolean;
}

interface RecentlyViewedStore {
  items: RecentlyViewedItem[];
  addItem: (item: RecentlyViewedItem) => void;
  removeItem: (id: number) => void;
}

export const useRecentlyViewedStore = create<RecentlyViewedStore>()(
  persist(
    (set) => ({
      items: [],
      addItem: (item) => set(state => {
        const filtered = state.items.filter(i => i.id !== item.id);
        return { items: [item, ...filtered].slice(0, 10) };
      }),
      removeItem: (id) => set(state => ({ items: state.items.filter(i => i.id !== id) })),
    }),
    { name: 'recently-viewed' }
  )
);
import { create } from 'zustand';

interface FavoriteItem {
  id: number;
  name: string;
  imageUrl: string;
  price: number;
  inStock: boolean;
}

interface FavoritesStore {
  items: FavoriteItem[];
  count: number;
  addItem: (product: FavoriteItem) => void;
  removeItem: (id: number) => void;
}

export const useFavoritesStore = create<FavoritesStore>((set) => ({
  items: [],
  count: 0,
  addItem: (product) => set((state) => {
    const exists = state.items.find(i => i.id === product.id);
    if (exists) return state;
    return { items: [...state.items, product], count: state.count + 1 };
  }),
  removeItem: (id) => set((state) => ({
    items: state.items.filter(i => i.id !== id),
    count: state.count - 1
  })),
}));
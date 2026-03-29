import { create } from 'zustand';

interface CompareItem {
  id: number;
  name: string;
  imageUrl: string;
  price: number;
}

interface CompareStore {
  items: CompareItem[];
  count: number;
  addItem: (product: CompareItem) => void;
  removeItem: (id: number) => void;
}

export const useCompareStore = create<CompareStore>((set) => ({
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
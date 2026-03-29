import { create } from 'zustand';

interface CartItem {
  id: number;
  name: string;
  imageUrl: string;
  price: number;
  quantity: number;
}

interface CartStore {
  items: CartItem[];
  count: number;
  addItem: (product: CartItem) => void;
  removeItem: (id: number) => void;
  incrementQuantity: (id: number) => void;
  decrementQuantity: (id: number) => void;
}

export const useCartStore = create<CartStore>((set) => ({
  items: [],
  count: 0,
  addItem: (product) => set((state) => {
    const exists = state.items.find(i => i.id === product.id);
    if (exists) return state;
    return { items: [...state.items, { ...product, quantity: 1 }], count: state.count + 1 };
  }),
  removeItem: (id) => set((state) => ({
    items: state.items.filter(i => i.id !== id),
    count: state.count - 1
  })),
  incrementQuantity: (id) => set((state) => ({
    items: state.items.map(i => i.id === id ? { ...i, quantity: i.quantity + 1 } : i)
  })),
  decrementQuantity: (id) => set((state) => ({
    items: state.items.map(i => i.id === id && i.quantity > 1 ? { ...i, quantity: i.quantity - 1 } : i)
  })),
}));
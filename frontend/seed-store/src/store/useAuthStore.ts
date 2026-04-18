import { create } from 'zustand';

interface AccountInfo {
  id: number;
  firstName: string;
  lastName: string;
  middleName: string;
  email: string;
  phoneNumber: string | null;
}

interface AuthStore {
  account: AccountInfo | null;
  setAccount: (account: AccountInfo | null) => void;
}

export const useAuthStore = create<AuthStore>((set) => ({
  account: null,
  setAccount: (account) => set({ account }),
}));
export interface Category {
  id: number;
  parentId: number | null;
  name: string;
  slug: string;
  imageUrl: string;
  viewOrder: number;
}
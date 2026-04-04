import type { Category } from '../../types/catalog';

interface Props {
  category: Category;
  onClick: (category: Category) => void;
}

export default function CategoryCard({ category, onClick }: Props) {
  return (
    <div
      className="cursor-pointer rounded-xl overflow-hidden shadow hover:shadow-md transition"
      onClick={() => onClick(category)}
    >
      <img
        src={category.imageUrl}
        alt={category.name}
        className="w-full aspect-square object-contain"
      />
      <p className="text-center font-medium py-2 px-1">{category.name}</p>
    </div>
  );
}
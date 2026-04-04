import { X } from 'lucide-react';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';
import CategoryFilters from '../category/CategoryFilters';
import CategoryFeatureFilters from '../category/CategoryFeatureFilters';
import type { ProductFiltersResponse } from '../../api/productsApi';
import type { ActiveFilter } from '../category/CategoryFeatureFilters';

interface Filters {
  priceFrom: number | null;
  priceTo: number | null;
  hasDiscount: boolean | null;
  inStock: boolean | null;
  sortByPriceAsc: boolean | null;
  sortByPriceDesc: boolean | null;
}

interface Props {
  isOpen: boolean;
  onClose: () => void;
  filters: Filters;
  onFiltersChange: (filters: Filters) => void;
  filtersData: ProductFiltersResponse | null;
  activeFilters: ActiveFilter[];
  onActiveFiltersChange: (filters: ActiveFilter[]) => void;
  selectedFeatures: Record<number, Set<string>>;
  onSelectedFeaturesChange: (selected: Record<number, Set<string>>) => void;
}

export default function FiltersModal({
  isOpen,
  onClose,
  filters,
  onFiltersChange,
  filtersData,
  onActiveFiltersChange,
  selectedFeatures,
  onSelectedFeaturesChange,
}: Props) {
  useModalScrollLock(isOpen);
  

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-sm shadow-xl flex flex-col h-[80vh]">
        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Фільтри</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>
        <div className="overflow-y-auto flex-1 p-4 flex flex-col gap-6">
          <CategoryFilters filters={filters} onChange={onFiltersChange} />
          {filtersData && filtersData.features.length > 0 && (
            <div className="pt-6 border-t border-gray-200">
              <CategoryFeatureFilters
                filtersData={filtersData}
                selected={selectedFeatures}
                onSelectedChange={onSelectedFeaturesChange}
                onChange={onActiveFiltersChange}
              />
            </div>
          )}
        </div>
        <div className="p-4 border-t border-gray-100 shrink-0">
          <button
            onClick={() => {
              onFiltersChange({
                priceFrom: null,
                priceTo: null,
                hasDiscount: null,
                inStock: null,
                sortByPriceAsc: null,
                sortByPriceDesc: null,
              });
              onActiveFiltersChange([]);
              onSelectedFeaturesChange({});
            }}
            className="w-full py-2 rounded border border-gray-300 text-sm text-gray-500 hover:border-red-400 hover:text-red-500 transition-colors"
          >
            Скинути всі фільтри
          </button>
        </div>
      </div>
    </div>
  );
}
import { useEffect, useState, useRef } from 'react';

interface Filters {
  priceFrom: number | null;
  priceTo: number | null;
  hasDiscount: boolean | null;
  inStock: boolean | null;
  sortByPriceAsc: boolean | null;
  sortByPriceDesc: boolean | null;
}

interface Props {
  filters: Filters;
  onChange: (filters: Filters) => void;
}

export default function CategoryFilters({ filters, onChange }: Props) {
  const [priceFrom, setPriceFrom] = useState(filters.priceFrom?.toString() ?? '');
  const [priceTo, setPriceTo] = useState(filters.priceTo?.toString() ?? '');

  

    const isFirstRender = useRef(true);

useEffect(() => {
  if (isFirstRender.current) {
    isFirstRender.current = false;
    return;
  }
  const timer = setTimeout(() => {
    onChange({
      ...filters,
      priceFrom: priceFrom ? Number(priceFrom) : null,
      priceTo: priceTo ? Number(priceTo) : null,
    });
  }, 500);

  return () => clearTimeout(timer);
}, [priceFrom, priceTo]);

  return (
    <div className="flex flex-col gap-6">
      <div>
        <p className="font-medium text-sm mb-3">Ціна, ₴</p>
        <div className="flex items-center gap-2">
          <input
            type="number"
            placeholder="Від"
            value={priceFrom}
            onChange={e => setPriceFrom(e.target.value)}
            className="w-full border border-gray-300 rounded px-2 py-1.5 text-sm outline-none focus:border-green-600"
          />
          <span className="text-gray-400">—</span>
          <input
            type="number"
            placeholder="До"
            value={priceTo}
            onChange={e => setPriceTo(e.target.value)}
            className="w-full border border-gray-300 rounded px-2 py-1.5 text-sm outline-none focus:border-green-600"
          />
        </div>
      </div>

      <div>
        <div className="flex flex-col gap-2">
          <label className="flex items-center gap-2 cursor-pointer text-sm">
            <input
              type="checkbox"
              checked={filters.sortByPriceAsc === true}
              onChange={e => onChange({
                ...filters,
                sortByPriceAsc: e.target.checked ? true : null,
                sortByPriceDesc: e.target.checked ? null : filters.sortByPriceDesc,
              })}
              className="accent-green-600"
            />
            Ціна: від дешевих
          </label>
          <label className="flex items-center gap-2 cursor-pointer text-sm">
            <input
              type="checkbox"
              checked={filters.sortByPriceDesc === true}
              onChange={e => onChange({
                ...filters,
                sortByPriceDesc: e.target.checked ? true : null,
                sortByPriceAsc: e.target.checked ? null : filters.sortByPriceAsc,
              })}
              className="accent-green-600"
            />
            Ціна: від дорогих
          </label>
        </div>
      </div>

      <div className="flex flex-col gap-2">
        <label className="flex items-center gap-2 cursor-pointer text-sm">
          <input
            type="checkbox"
            checked={filters.hasDiscount === true}
            onChange={e => onChange({ ...filters, hasDiscount: e.target.checked ? true : null })}
            className="accent-green-600"
          />
          Тільки зі знижкою
        </label>
        <label className="flex items-center gap-2 cursor-pointer text-sm">
          <input
            type="checkbox"
            checked={filters.inStock === true}
            onChange={e => onChange({ ...filters, inStock: e.target.checked ? true : null })}
            className="accent-green-600"
          />
          Тільки в наявності
        </label>
      </div>
    </div>
  );
}
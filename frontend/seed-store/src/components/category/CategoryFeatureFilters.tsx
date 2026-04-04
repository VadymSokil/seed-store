import type { ProductFiltersResponse } from '../../api/productsApi';

export interface ActiveFilter {
  featureSlug: string;
  valueSlug: string;
}

interface Props {
  filtersData: ProductFiltersResponse;
  selected: Record<number, Set<string>>;
  onSelectedChange: (selected: Record<number, Set<string>>) => void;
  onChange: (filters: ActiveFilter[]) => void;
}

export default function CategoryFeatureFilters({ filtersData, selected, onSelectedChange, onChange }: Props) {
  const { features, headers, filterValues } = filtersData;

  const handleChange = (featureId: number, valueSlug: string, totalValues: number) => {
    const newSelected = { ...selected };
    const current = new Set(newSelected[featureId] ?? []);

    if (current.has(valueSlug)) {
      current.delete(valueSlug);
    } else {
      current.add(valueSlug);
    }

    if (current.size === totalValues || current.size === 0) {
      delete newSelected[featureId];
    } else {
      newSelected[featureId] = current;
    }

    onSelectedChange(newSelected);

    const activeFilters: ActiveFilter[] = [];
    for (const [fId, slugs] of Object.entries(newSelected)) {
      const feature = features.find(f => f.id === Number(fId));
      if (!feature) continue;
      for (const vSlug of slugs as unknown as Set<string>) {
        activeFilters.push({ featureSlug: feature.slug, valueSlug: vSlug });
      }
    }
    onChange(activeFilters);
  };

  const sortedHeaders = [...headers].sort((a, b) => (a.viewOrder ?? 0) - (b.viewOrder ?? 0));

  return (
    <div className="flex flex-col gap-4">
      {sortedHeaders.map(header => {
        const headerFeatureIds = [...new Set(
          filterValues.filter(v => v.headerId === header.id).map(v => v.featureId)
        )];
        if (headerFeatureIds.length === 0) return null;

        const headerFeatures = features
          .filter(f => headerFeatureIds.includes(f.id))
          .sort((a, b) => a.viewOrder - b.viewOrder);

        return (
          <div key={header.id}>
            <p className="text-sm font-semibold mb-2 text-gray-700">{header.name}</p>
            <div className="flex flex-col gap-3 pl-1">
              {headerFeatures.map(feature => {
                const values = filterValues
                  .filter(v => v.featureId === feature.id && v.headerId === header.id)
                  .sort((a, b) => (a.viewOrder ?? 0) - (b.viewOrder ?? 0));

                return (
                  <div key={feature.id}>
                    <p className="text-sm font-medium mb-1.5 text-gray-600">{feature.name}</p>
                    <div className="flex flex-col gap-1.5 pl-1">
                      {values.map(v => (
                        <label key={v.id} className="flex items-center gap-2 cursor-pointer text-sm text-gray-600">
                          <input
                            type="checkbox"
                            checked={selected[feature.id]?.has(v.valueSlug) ?? false}
                            onChange={() => handleChange(feature.id, v.valueSlug, values.length)}
                            className="accent-green-600"
                          />
                          {v.value}
                        </label>
                      ))}
                    </div>
                  </div>
                );
              })}
            </div>
          </div>
        );
      })}
    </div>
  );
}
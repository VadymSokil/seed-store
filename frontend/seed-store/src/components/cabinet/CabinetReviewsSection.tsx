import { useState, useEffect } from 'react';
import { Star } from 'lucide-react';
import { getAccountReviews } from '../../api/reviewsApi';
import type { AccountReview } from '../../api/reviewsApi';
import ReviewDetailsModal from '../modals/ReviewDetailsModal';

const PAGE_SIZE = 5;

const statusLabel: Record<string, { label: string; className: string }> = {
  pending: { label: 'На модерації', className: 'text-orange-500' },
  approved: { label: 'Опубліковано', className: 'text-green-600' },
  rejected: { label: 'Відхилено', className: 'text-red-500' },
};

const getPageNumbers = (page: number, totalPages: number) => {
  if (totalPages <= 5) return Array.from({ length: totalPages }, (_, i) => i + 1);
  if (page <= 3) return [1, 2, 3, 4, '...', totalPages];
  if (page >= totalPages - 2) return [1, '...', totalPages - 3, totalPages - 2, totalPages - 1, totalPages];
  return [1, '...', page - 1, page, page + 1, '...', totalPages];
};

export default function CabinetReviewsSection() {
  const [reviews, setReviews] = useState<AccountReview[]>([]);
  const [selectedReview, setSelectedReview] = useState<AccountReview | null>(null);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(true);

  const totalPages = Math.ceil(totalCount / PAGE_SIZE);

  const load = (p: number) => {
    setIsLoading(true);
    getAccountReviews(p)
      .then(data => {
        setReviews(data.items);
        setTotalCount(data.totalCount);
      })
      .catch(console.error)
      .finally(() => setIsLoading(false));
  };

  useEffect(() => { load(page); }, [page]);

  const reload = () => load(page);

  return (
    <>
      {selectedReview && (
        <ReviewDetailsModal
          review={selectedReview}
          onClose={() => setSelectedReview(null)}
          onUpdated={reload}
          onDeleted={reload}
        />
      )}

      {isLoading ? (
        <p className="text-sm text-gray-400">Завантаження...</p>
      ) : reviews.length === 0 ? (
        <p className="text-sm text-gray-400">Відгуків поки немає</p>
      ) : (
        <>
          <div className="flex flex-col gap-3">
            {reviews.map(review => {
              const status = statusLabel[review.statusCode] ?? { label: review.statusCode, className: 'text-gray-500' };
              return (
                <div
                  key={review.id}
                  onClick={() => setSelectedReview(review)}
                  className="border border-gray-200 rounded-lg p-4 cursor-pointer hover:border-green-400 transition-colors"
                >
                  <div className="flex items-center gap-3 mb-2">
                    {review.productImageUrlSnapshot && (
                      <img src={review.productImageUrlSnapshot} alt={review.productNameSnapshot} className="w-10 h-10 object-contain rounded border border-gray-100" />
                    )}
                    <p className="text-xs text-gray-700 line-clamp-2 flex-1">{review.productNameSnapshot}</p>
                  </div>
                  <div className="flex items-center justify-between">
                    <div className="flex gap-0.5">
                      {[1, 2, 3, 4, 5].map(star => (
                        <Star key={star} size={14} className={star <= review.rating ? 'text-yellow-400 fill-yellow-400' : 'text-gray-300 fill-gray-300'} />
                      ))}
                    </div>
                    <span className={`text-xs ${status.className}`}>{status.label}</span>
                  </div>
                </div>
              );
            })}
          </div>

          {totalPages > 1 && (
            <div className="flex justify-center items-center gap-1 mt-6">
              <button
                onClick={() => setPage(p => p - 1)}
                disabled={page === 1}
                className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
              >
                ←
              </button>
              {getPageNumbers(page, totalPages).map((p, i) =>
                p === '...' ? (
                  <span key={`dots-${i}`} className="px-3 py-2 text-gray-400">...</span>
                ) : (
                  <button
                    key={p}
                    onClick={() => setPage(p as number)}
                    className={`px-3 py-2 rounded border transition-colors ${
                      page === p
                        ? 'bg-green-600 text-white border-green-600'
                        : 'border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600'
                    }`}
                  >
                    {p}
                  </button>
                )
              )}
              <button
                onClick={() => setPage(p => p + 1)}
                disabled={page === totalPages}
                className="px-3 py-2 rounded border border-gray-300 text-gray-600 hover:border-green-600 hover:text-green-600 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
              >
                →
              </button>
            </div>
          )}
        </>
      )}
    </>
  );
}
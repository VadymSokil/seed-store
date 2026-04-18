import { useState } from 'react';
import { X, Star } from 'lucide-react';
import type { AccountReview } from '../../api/reviewsApi';
import { updateReview, deleteReview } from '../../api/reviewsApi';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';

interface Props {
  review: AccountReview;
  onClose: () => void;
  onUpdated: () => void;
  onDeleted: () => void;
}

const formatDateTime = (iso: string) =>
  new Date(iso).toLocaleString('uk-UA', {
    day: '2-digit', month: '2-digit', year: 'numeric',
    hour: '2-digit', minute: '2-digit'
  });

const statusLabel: Record<string, { label: string; className: string }> = {
  pending: { label: 'На модерації', className: 'text-orange-500' },
  approved: { label: 'Опубліковано', className: 'text-green-600' },
  rejected: { label: 'Відхилено', className: 'text-red-500' },
};

const StarRating = ({ value, onChange }: { value: number; onChange?: (v: number) => void }) => (
  <div className="flex gap-1">
    {[1, 2, 3, 4, 5].map(star => (
      <button key={star} type="button" onClick={() => onChange?.(star)} className={onChange ? 'cursor-pointer' : 'cursor-default'}>
        <Star size={20} className={star <= value ? 'text-yellow-400 fill-yellow-400' : 'text-gray-300 fill-gray-300'} />
      </button>
    ))}
  </div>
);

export default function ReviewDetailsModal({ review, onClose, onUpdated, onDeleted }: Props) {
  useModalScrollLock(true);

  const [isEditing, setIsEditing] = useState(false);
  const [formRating, setFormRating] = useState(review.rating);
  const [formText, setFormText] = useState(review.text ?? '');
  const [formError, setFormError] = useState('');
  const [formLoading, setFormLoading] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);

  const status = statusLabel[review.statusCode] ?? { label: review.statusCode, className: 'text-gray-500' };

  const handleUpdate = async () => {
    setFormError('');
    setFormLoading(true);
    try {
      await updateReview(review.id, { rating: formRating, text: formText || undefined });
      onUpdated();
      onClose();
    } catch {
      setFormError('Помилка. Спробуйте ще раз');
    } finally {
      setFormLoading(false);
    }
  };

  const handleDelete = async () => {
    setDeleteLoading(true);
    try {
      await deleteReview(review.id);
      onDeleted();
      onClose();
    } catch {
    } finally {
      setDeleteLoading(false);
    }
  };

  const rowClass = 'flex justify-between gap-4 py-1.5 border-b border-gray-50 last:border-0';
  const labelClass = 'text-sm text-gray-400 shrink-0';
  const valueClass = 'text-sm text-gray-700 text-right';

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-lg shadow-xl flex flex-col max-h-[90vh]">

        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <h2 className="font-semibold text-gray-800">Відгук</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>

        <div className="overflow-y-auto flex-1 p-4 flex flex-col gap-4">

          <div className="flex items-center gap-3">
            {review.productImageUrlSnapshot && (
              <img src={review.productImageUrlSnapshot} alt={review.productNameSnapshot} className="w-14 h-14 object-contain rounded border border-gray-200" />
            )}
            <p className="text-sm text-gray-700 line-clamp-2">{review.productNameSnapshot}</p>
          </div>

          {!isEditing ? (
            <>
              <div className="flex flex-col">
                <div className={rowClass}>
                  <span className={labelClass}>Оцінка</span>
                  <StarRating value={review.rating} />
                </div>
                <div className={rowClass}>
                  <span className={labelClass}>Статус</span>
                  <span className={`text-sm ${status.className}`}>{status.label}</span>
                </div>
                <div className={rowClass}>
                  <span className={labelClass}>Дата</span>
                  <span className={valueClass}>{formatDateTime(review.createdAt)}</span>
                </div>
                {review.updatedAt && (
                  <div className={rowClass}>
                    <span className={labelClass}>Редаговано</span>
                    <span className={valueClass}>{formatDateTime(review.updatedAt)}</span>
                  </div>
                )}
                <div className={rowClass}>
                  <span className={labelClass}>Коментар</span>
                  <span className={valueClass}>{review.text || '—'}</span>
                </div>
                <div className={rowClass}>
                  <span className={labelClass}>Коментар модератора</span>
                  <span className={valueClass}>{review.moderatorComment || '—'}</span>
                </div>
              </div>

              {review.reply && (
              <div className="bg-green-50 border border-green-100 rounded p-3">
                <p className="text-xs text-green-700 font-medium mb-1">Відповідь магазину:</p>
                <p className="text-sm text-gray-700">{review.reply}</p>
                {review.replyCreatedAt && (
                  <p className="text-xs text-gray-400 mt-1">{formatDateTime(review.replyCreatedAt)}</p>
                )}
                {review.replyUpdatedAt && (
                  <p className="text-xs text-gray-400 mt-0.5">Редаговано: {formatDateTime(review.replyUpdatedAt)}</p>
                )}
              </div>
            )}
                
            </>
          ) : (
            <div className="flex flex-col gap-3">
              <div>
                <p className="text-xs text-gray-400 mb-1">Оцінка *</p>
                <StarRating value={formRating} onChange={setFormRating} />
              </div>
              <div>
                <p className="text-xs text-gray-400 mb-1">Коментар</p>
                <textarea
                  value={formText}
                  onChange={e => setFormText(e.target.value)}
                  rows={3}
                  className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 resize-none"
                  placeholder="Поділіться враженнями про товар..."
                />
              </div>
              {formError && <p className="text-xs text-red-500">{formError}</p>}
            </div>
          )}
        </div>

        <div className="p-4 flex flex-col gap-2 border-t border-gray-100">
          {!isEditing ? (
            <>
              <button
                onClick={() => setIsEditing(true)}
                className="w-full bg-green-600 text-white py-2 rounded text-sm hover:bg-green-700 transition-colors font-medium"
              >
                Редагувати
              </button>
              <button
                onClick={handleDelete}
                disabled={deleteLoading}
                className="w-full border border-gray-300 text-gray-600 py-2 rounded text-sm hover:border-red-400 hover:text-red-500 transition-colors font-medium disabled:opacity-60"
              >
                {deleteLoading ? 'Видалення...' : 'Видалити'}
              </button>
            </>
          ) : (
            <>
              <button
                onClick={handleUpdate}
                disabled={formLoading}
                className="w-full bg-green-600 text-white py-2 rounded text-sm hover:bg-green-700 transition-colors font-medium disabled:opacity-60"
              >
                {formLoading ? 'Збереження...' : 'Зберегти'}
              </button>
              <button
                onClick={() => { setIsEditing(false); setFormError(''); }}
                className="w-full border border-gray-300 text-gray-600 py-2 rounded text-sm hover:border-gray-400 transition-colors font-medium"
              >
                Скасувати
              </button>
            </>
          )}
        </div>

      </div>
    </div>
  );
}
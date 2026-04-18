import { useState } from 'react';
import { Pencil, Check, X } from 'lucide-react';
import { useAuthStore } from '../../store/useAuthStore';
import { useNavigate } from 'react-router-dom';
import { logout } from '../../api/authApi';
import { changeName, changeEmail, changePhone, changePassword, deleteAccount } from '../../api/accountApi';
import EmailVerifyModal from '../modals/EmailVerifyModal';
import DeleteAccountModal from '../modals/DeleteAccountModal';
import PasswordInput from '../ui/PasswordInput';

type EditField = 'name' | 'email' | 'phone' | 'password' | null;

const inputClass = 'w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600';

export default function CabinetProfileSection() {
  const navigate = useNavigate();
  const account = useAuthStore(s => s.account);
  const setAccount = useAuthStore(s => s.setAccount);

  const [edit, setEdit] = useState<EditField>(null);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isEmailVerifyOpen, setIsEmailVerifyOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [middleName, setMiddleName] = useState('');
  const [newEmail, setNewEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const openEdit = (field: EditField) => {
    setError('');
    setEdit(field);
    if (field === 'name' && account) {
      setFirstName(account.firstName);
      setLastName(account.lastName);
      setMiddleName(account.middleName ?? '');
    }
    if (field === 'phone' && account) setPhone(account.phoneNumber ?? '');
    if (field === 'email' && account) setNewEmail(account.email);
  };

  const cancelEdit = () => { setEdit(null); setError(''); };

  const handleChangeName = async () => {
    setError('');
    if (!firstName || !lastName) { setError('Заповніть обов\'язкові поля'); return; }
    setIsLoading(true);
    try {
      await changeName({ firstName, lastName, middleName });
      setAccount({ ...account!, firstName, lastName, middleName });
      cancelEdit();
    } catch { setError('Помилка. Спробуйте ще раз'); }
    finally { setIsLoading(false); }
  };

  const handleChangeEmail = async () => {
    setError('');
    if (!newEmail) { setError('Введіть email'); return; }
    setIsLoading(true);
    try {
      await changeEmail(newEmail);
      cancelEdit();
      setIsEmailVerifyOpen(true);
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'email_taken') setError('Цей email вже використовується');
      else setError('Помилка. Спробуйте ще раз');
    } finally { setIsLoading(false); }
  };

  const handleChangePhone = async () => {
    setError('');
    setIsLoading(true);
    try {
      await changePhone(phone || null);
      setAccount({ ...account!, phoneNumber: phone || null });
      cancelEdit();
    } catch { setError('Помилка. Спробуйте ще раз'); }
    finally { setIsLoading(false); }
  };

  const handleChangePassword = async () => {
    setError('');
    if (!oldPassword || !newPassword || !confirmPassword) { setError('Заповніть усі поля'); return; }
    if (newPassword !== confirmPassword) { setError('Паролі не збігаються'); return; }
    if (newPassword.length < 8) { setError('Новий пароль має бути не менше 8 символів'); return; }
    setIsLoading(true);
    try {
      await changePassword({ oldPassword, newPassword });
      cancelEdit();
      setOldPassword(''); setNewPassword(''); setConfirmPassword('');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_password') setError('Невірний поточний пароль');
      else setError('Помилка. Спробуйте ще раз');
    } finally { setIsLoading(false); }
  };

  const handleDeleteAccount = async () => {
    try {
      await deleteAccount();
      setAccount(null);
      navigate('/');
    } catch { setError('Помилка видалення акаунту'); }
  };

  const handleLogout = async () => {
    await logout();
    setAccount(null);
    navigate('/');
  };

  if (!account) return null;

  const fields: { key: EditField; label: string; display: string }[] = [
    { key: 'name', label: 'ФІО', display: `${account.lastName} ${account.firstName} ${account.middleName ?? ''}`.trim() },
    { key: 'email', label: 'Email', display: account.email },
    { key: 'phone', label: 'Телефон', display: account.phoneNumber ?? 'Не вказано' },
    { key: 'password', label: 'Пароль', display: '••••••••' },
  ];

  return (
    <>
      {isEmailVerifyOpen && <EmailVerifyModal newEmail={newEmail} onClose={() => setIsEmailVerifyOpen(false)} />}
      {isDeleteModalOpen && <DeleteAccountModal onConfirm={handleDeleteAccount} onClose={() => setIsDeleteModalOpen(false)} />}

      <div className="flex flex-col gap-4">
        {fields.map(({ key, label, display }) => (
          <div key={key} className="border border-gray-200 rounded-lg p-4">
            <div className="flex items-center justify-between mb-2">
              <p className="text-xs text-gray-400 uppercase tracking-wide">{label}</p>
              {edit !== key && (
                <button onClick={() => openEdit(key)} className="text-gray-400 hover:text-green-600 transition-colors">
                  <Pencil size={15} />
                </button>
              )}
            </div>

            {edit === key ? (
              <div className="flex flex-col gap-2">
                {key === 'name' && (
                  <>
                    <input placeholder="Прізвище *" value={lastName} onChange={e => setLastName(e.target.value)} className={inputClass} />
                    <input placeholder="Ім'я *" value={firstName} onChange={e => setFirstName(e.target.value)} className={inputClass} />
                    <input placeholder="По батькові" value={middleName} onChange={e => setMiddleName(e.target.value)} className={inputClass} />
                  </>
                )}
                {key === 'email' && (
                  <input type="email" placeholder="Новий email" value={newEmail} onChange={e => setNewEmail(e.target.value)} className={inputClass} />
                )}
                {key === 'phone' && (
                  <input type="tel" placeholder="+380XXXXXXXXX" value={phone} onChange={e => setPhone(e.target.value)} className={inputClass} />
                )}
                {key === 'password' && (
                  <>
                    <PasswordInput placeholder="Поточний пароль" value={oldPassword} onChange={setOldPassword} className={`${inputClass} pr-9`} />
                    <PasswordInput placeholder="Новий пароль" value={newPassword} onChange={setNewPassword} className={`${inputClass} pr-9`} />
                    <PasswordInput placeholder="Підтвердіть новий пароль" value={confirmPassword} onChange={setConfirmPassword} className={`${inputClass} pr-9`} />
                  </>
                )}
                {error && <p className="text-xs text-red-500">{error}</p>}
                <div className="flex gap-2">
                  <button
                    onClick={key === 'name' ? handleChangeName : key === 'email' ? handleChangeEmail : key === 'phone' ? handleChangePhone : handleChangePassword}
                    disabled={isLoading}
                    className="flex items-center gap-1 text-sm text-green-600 hover:text-green-700 transition-colors disabled:opacity-60"
                  >
                    <Check size={15} /> Зберегти
                  </button>
                  <button onClick={cancelEdit} className="flex items-center gap-1 text-sm text-gray-400 hover:text-red-500 transition-colors">
                    <X size={15} /> Скасувати
                  </button>
                </div>
              </div>
            ) : (
              <p className="text-sm text-gray-800">{display}</p>
            )}
          </div>
        ))}

        <div className="flex items-center justify-between pt-2">
          <button onClick={handleLogout} className="text-sm text-gray-400 hover:text-red-500 transition-colors">Вийти</button>
          <button onClick={() => setIsDeleteModalOpen(true)} className="text-sm text-gray-400 hover:text-red-500 transition-colors">Видалити акаунт</button>
        </div>
      </div>
    </>
  );
}
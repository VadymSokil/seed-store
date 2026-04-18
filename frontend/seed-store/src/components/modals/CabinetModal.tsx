import { useState, useRef } from 'react';
import { X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { Turnstile } from '@marsidev/react-turnstile';
import type { TurnstileInstance } from '@marsidev/react-turnstile';
import { useModalScrollLock } from '../../hooks/useModalScrollLock';
import { register, verifyEmail, resendVerificationCode, login, forgotPassword, verifyResetCode, resetPassword, resendResetCode } from '../../api/authApi';
import { useAuthStore } from '../../store/useAuthStore';
import CodeInput from '../ui/CodeInput';
import { useCountdown } from '../../hooks/useCountdown';
import { getAccount } from '../../api/accountApi';
import PasswordInput from '../ui/PasswordInput';

type ModalView = 'choice' | 'login' | 'register' | 'verify' | 'forgot' | 'verify-reset' | 'reset-password';

interface CabinetModalProps {
  onClose: () => void;
}

const SITE_KEY = import.meta.env.VITE_TURNSTILE_SITE_KEY as string;

const CabinetModal = ({ onClose }: CabinetModalProps) => {
  const navigate = useNavigate();
  const setAccount = useAuthStore(s => s.setAccount);

  const [view, setView] = useState<ModalView>('choice');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [middleName, setMiddleName] = useState('');
  const [code, setCode] = useState('');
  const [resetToken, setResetToken] = useState('');
  const [rememberMe, setRememberMe] = useState(true);
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [turnstileToken, setTurnstileToken] = useState('');

  const loginTurnstileRef = useRef<TurnstileInstance>(null);
  const registerTurnstileRef = useRef<TurnstileInstance>(null);

  const { formatted: verifyTimer, expired: verifyExpired } = useCountdown(900, view === 'verify');
  const { formatted: resetTimer, expired: resetExpired } = useCountdown(900, view === 'verify-reset');

  useModalScrollLock(true);

  const titles: Record<ModalView, string> = {
    choice: 'Кабінет',
    login: 'Вхід',
    register: 'Реєстрація',
    verify: 'Підтвердження пошти',
    forgot: 'Відновлення пароля',
    'verify-reset': 'Підтвердження коду',
    'reset-password': 'Новий пароль',
  };

  const handleBack = () => {
    setError('');
    setTurnstileToken('');
    if (view === 'login' || view === 'register') setView('choice');
    if (view === 'verify') setView('register');
    if (view === 'forgot') setView('login');
    if (view === 'verify-reset') setView('forgot');
    if (view === 'reset-password') setView('verify-reset');
  };

  const handleLogin = async () => {
    setError('');
    if (!email || !password) { setError('Заповніть усі поля'); return; }
    if (password.length < 8) { setError('Пароль має бути не менше 8 символів'); return; }
    if (!turnstileToken) { setError('Пройдіть перевірку'); return; }
    setIsLoading(true);
    try {
      await login({ email, password, rememberMe, turnstileToken });
      const account = await getAccount();
      setAccount(account);
      onClose();
      navigate('/cabinet');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_captcha') setError('Помилка перевірки. Спробуйте ще раз');
      else setError('Невірний email або пароль');
      loginTurnstileRef.current?.reset();
      setTurnstileToken('');
    } finally {
      setIsLoading(false);
    }
  };

  const handleRegister = async () => {
    setError('');
    if (!email || !password || !confirmPassword || !firstName || !lastName) { setError('Заповніть усі обов\'язкові поля'); return; }
    if (password.length < 8) { setError('Пароль має бути не менше 8 символів'); return; }
    if (password !== confirmPassword) { setError('Паролі не збігаються'); return; }
    if (!turnstileToken) { setError('Пройдіть перевірку'); return; }
    setIsLoading(true);
    try {
      await register({ email, password, confirmPassword, firstName, lastName, middleName, turnstileToken });
      setView('verify');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_captcha') setError('Помилка перевірки. Спробуйте ще раз');
      else if (status === 'email_taken') setError('Цей email вже використовується');
      else setError('Помилка реєстрації. Спробуйте ще раз');
      registerTurnstileRef.current?.reset();
      setTurnstileToken('');
    } finally {
      setIsLoading(false);
    }
  };

  const handleVerify = async () => {
    setError('');
    if (!code) { setError('Введіть код підтвердження'); return; }
    setIsLoading(true);
    try {
      await verifyEmail({ email, code });
      const account = await getAccount();
      setAccount(account);
      onClose();
      navigate('/cabinet');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_code') setError('Невірний код');
      else if (status === 'code_expired') setError('Код застарів. Запросіть новий');
      else setError('Помилка підтвердження. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const handleResend = async () => {
    setError('');
    setCode('');
    setIsLoading(true);
    try {
      await resendVerificationCode(email);
    } catch {
      setError('Не вдалося надіслати код. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const handleForgotPassword = async () => {
    setError('');
    if (!email) { setError('Введіть email'); return; }
    setIsLoading(true);
    try {
      await forgotPassword(email);
      setView('verify-reset');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'not_found') setError('Акаунт з таким email не знайдено');
      else setError('Помилка. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const handleVerifyResetCode = async () => {
    setError('');
    if (!code) { setError('Введіть код підтвердження'); return; }
    setIsLoading(true);
    try {
      const token = await verifyResetCode(email, code);
      setResetToken(token);
      setCode('');
      setView('reset-password');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_code') setError('Невірний код');
      else if (status === 'code_expired') setError('Код застарів. Запросіть новий');
      else setError('Помилка. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const handleResendResetCode = async () => {
    setError('');
    setCode('');
    setIsLoading(true);
    try {
      await resendResetCode(email);
    } catch {
      setError('Не вдалося надіслати код. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  const handleResetPassword = async () => {
    setError('');
    if (!newPassword || !confirmPassword) { setError('Заповніть усі поля'); return; }
    if (newPassword !== confirmPassword) { setError('Паролі не збігаються'); return; }
    setIsLoading(true);
    try {
      await resetPassword({ email, password: newPassword, token: resetToken });
      setView('login');
      setNewPassword('');
      setConfirmPassword('');
    } catch (err: unknown) {
      const status = (err as { response?: { data?: string } }).response?.data;
      if (status === 'invalid_token') setError('Невірний токен. Почніть процес заново');
      else setError('Помилка. Спробуйте ще раз');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40">
      <div className="bg-white rounded-lg w-full max-w-sm shadow-xl">

        <div className="flex justify-between items-center p-4 border-b border-gray-100">
          <div className="flex items-center gap-2">
            {view !== 'choice' && (
              <button onClick={handleBack} className="text-gray-400 hover:text-gray-600 transition-colors text-sm">←</button>
            )}
            <h2 className="font-semibold text-gray-800">{titles[view]}</h2>
          </div>
          <button onClick={onClose} className="text-gray-400 hover:text-red-400 transition-colors">
            <X size={20} />
          </button>
        </div>

        <div className="p-4">

          {view === 'choice' && (
            <div className="flex flex-col gap-3">
              <button onClick={() => setView('login')} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium">Увійти</button>
              <button onClick={() => setView('register')} className="w-full border border-green-600 text-green-600 py-2 rounded hover:bg-green-50 transition-colors font-medium">Зареєструватись</button>
            </div>
          )}

          {view === 'login' && (
            <div className="flex flex-col gap-3">
              <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600" />
              <PasswordInput placeholder="Пароль" value={password} onChange={setPassword} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 pr-9" />
              <label className="flex items-center gap-2 cursor-pointer text-sm text-gray-600 select-none">
                <input type="checkbox" checked={rememberMe} onChange={e => setRememberMe(e.target.checked)} className="accent-green-600" />
                Запам'ятати мене
              </label>
              <Turnstile
                ref={loginTurnstileRef}
                siteKey={SITE_KEY}
                onSuccess={setTurnstileToken}
                onExpire={() => setTurnstileToken('')}
                onError={() => setTurnstileToken('')}
                options={{ theme: 'light' }}
              />
              {error && <p className="text-xs text-red-500">{error}</p>}
              <button onClick={handleLogin} disabled={isLoading} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed">
                {isLoading ? 'Завантаження...' : 'Увійти'}
              </button>
              <button onClick={() => { setError(''); setView('forgot'); }} className="text-sm text-gray-400 hover:text-green-600 transition-colors text-center">Забули пароль?</button>
            </div>
          )}

          {view === 'register' && (
            <div className="flex flex-col gap-3">
              <input type="text" placeholder="Прізвище *" value={lastName} onChange={e => setLastName(e.target.value)} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600" />
              <input type="text" placeholder="Ім'я *" value={firstName} onChange={e => setFirstName(e.target.value)} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600" />
              <input type="text" placeholder="По батькові" value={middleName} onChange={e => setMiddleName(e.target.value)} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600" />
              <input type="email" placeholder="Email *" value={email} onChange={e => setEmail(e.target.value)} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600" />
              <PasswordInput placeholder="Пароль *" value={password} onChange={setPassword} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 pr-9" />
              <PasswordInput placeholder="Підтвердіть пароль *" value={confirmPassword} onChange={setConfirmPassword} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 pr-9" />
              <Turnstile
                ref={registerTurnstileRef}
                siteKey={SITE_KEY}
                onSuccess={setTurnstileToken}
                onExpire={() => setTurnstileToken('')}
                onError={() => setTurnstileToken('')}
                options={{ theme: 'light' }}
              />
              {error && <p className="text-xs text-red-500">{error}</p>}
              <button onClick={handleRegister} disabled={isLoading} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed">
                {isLoading ? 'Завантаження...' : 'Зареєструватись'}
              </button>
            </div>
          )}

          {view === 'verify' && (
            <div className="flex flex-col gap-3">
              <p className="text-sm text-gray-500">На адресу <span className="font-medium text-gray-700">{email}</span> надіслано код підтвердження.</p>
              <p className={`text-xs ${verifyExpired ? 'text-red-500' : 'text-gray-400'}`}>Код дійсний: {verifyTimer}</p>
              <CodeInput value={code} onChange={setCode} />
              {error && <p className="text-xs text-red-500">{error}</p>}
              <button onClick={handleVerify} disabled={isLoading} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed">
                {isLoading ? 'Завантаження...' : 'Підтвердити'}
              </button>
              <button onClick={handleResend} disabled={isLoading} className="text-sm text-gray-400 hover:text-green-600 transition-colors text-center disabled:opacity-60 disabled:cursor-not-allowed">Надіслати код повторно</button>
            </div>
          )}

          {view === 'forgot' && (
            <div className="flex flex-col gap-3">
              <p className="text-sm text-gray-500">Введіть email вашого акаунту і ми надішлемо код для відновлення пароля.</p>
              <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600" />
              {error && <p className="text-xs text-red-500">{error}</p>}
              <button onClick={handleForgotPassword} disabled={isLoading} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed">
                {isLoading ? 'Завантаження...' : 'Надіслати код'}
              </button>
            </div>
          )}

          {view === 'verify-reset' && (
            <div className="flex flex-col gap-3">
              <p className="text-sm text-gray-500">На адресу <span className="font-medium text-gray-700">{email}</span> надіслано код підтвердження.</p>
              <p className={`text-xs ${resetExpired ? 'text-red-500' : 'text-gray-400'}`}>Код дійсний: {resetTimer}</p>
              <CodeInput value={code} onChange={setCode} />
              {error && <p className="text-xs text-red-500">{error}</p>}
              <button onClick={handleVerifyResetCode} disabled={isLoading} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed">
                {isLoading ? 'Завантаження...' : 'Підтвердити'}
              </button>
              <button onClick={handleResendResetCode} disabled={isLoading} className="text-sm text-gray-400 hover:text-green-600 transition-colors text-center disabled:opacity-60 disabled:cursor-not-allowed">Надіслати код повторно</button>
            </div>
          )}

          {view === 'reset-password' && (
            <div className="flex flex-col gap-3">
              <PasswordInput placeholder="Новий пароль" value={newPassword} onChange={setNewPassword} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 pr-9" />
              <PasswordInput placeholder="Підтвердіть пароль" value={confirmPassword} onChange={setConfirmPassword} className="w-full border border-gray-300 rounded px-3 py-2 text-sm outline-none focus:border-green-600 pr-9" />
              {error && <p className="text-xs text-red-500">{error}</p>}
              <button onClick={handleResetPassword} disabled={isLoading} className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition-colors font-medium text-sm disabled:opacity-60 disabled:cursor-not-allowed">
                {isLoading ? 'Завантаження...' : 'Змінити пароль'}
              </button>
            </div>
          )}

        </div>
      </div>
    </div>
  );
};

export default CabinetModal;
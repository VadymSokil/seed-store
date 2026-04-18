import api from './axiosInstance';

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  middleName: string;
  turnstileToken: string;
}

export interface VerifyEmailRequest {
  email: string;
  code: string;
}

export const register = (data: RegisterRequest): Promise<void> =>
  api.post('/api/auth/registration', data).then(r => r.data);

export const verifyEmail = (data: VerifyEmailRequest): Promise<void> =>
  api.post('/api/auth/verify-email', data).then(r => r.data);

export const resendVerificationCode = (email: string): Promise<void> =>
  api.post('/api/auth/resend-verification-code', { email }).then(r => r.data);


export interface LoginRequest {
  email: string;
  password: string;
  rememberMe: boolean;
  turnstileToken: string;
}

export const login = (data: LoginRequest): Promise<void> =>
  api.post('/api/auth/login', data).then(r => r.data);

export const logout = (): Promise<void> =>
  api.post('/api/auth/logout').then(r => r.data);


export interface ResetPasswordRequest {
  email: string;
  password: string;
  token: string;
}

export const forgotPassword = (email: string): Promise<void> =>
  api.post('/api/auth/forgot-password', { email }).then(r => r.data);

export const verifyResetCode = (email: string, code: string): Promise<string> =>
  api.post('/api/auth/verify-reset-code', { email, code }).then(r => r.data);

export const resetPassword = (data: ResetPasswordRequest): Promise<void> =>
  api.post('/api/auth/reset-password', data).then(r => r.data);

export const resendResetCode = (email: string): Promise<void> =>
  api.post('/api/auth/resend-reset-code', { email }).then(r => r.data);
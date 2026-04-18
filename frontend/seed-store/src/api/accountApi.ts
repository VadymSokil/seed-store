import api from './axiosInstance';

export interface AccountInfo {
  id: number
  firstName: string;
  lastName: string;
  middleName: string;
  email: string;
  phoneNumber: string | null;
}

export interface ChangeNameRequest {
  firstName: string;
  lastName: string;
  middleName: string;
}

export interface ChangePasswordRequest {
  oldPassword: string;
  newPassword: string;
}

export interface ConfirmEmailChangeRequest {
  newEmail: string;
  code: string;
}

export const getAccount = (): Promise<AccountInfo> =>
  api.get<AccountInfo>('/api/account').then(r => r.data);

export const changeName = (data: ChangeNameRequest): Promise<void> =>
  api.put('/api/account/name', data).then(r => r.data);

export const changeEmail = (newEmail: string): Promise<void> =>
  api.put('/api/account/email', { newEmail }).then(r => r.data);

export const confirmEmailChange = (data: ConfirmEmailChangeRequest): Promise<void> =>
  api.put('/api/account/confirm-email', data).then(r => r.data);

export const resendEmailChangeCode = (): Promise<void> =>
  api.post('/api/account/resend-email-code').then(r => r.data);

export const changePhone = (phoneNumber: string | null): Promise<void> =>
  api.put('/api/account/phone', { phoneNumber }).then(r => r.data);

export const changePassword = (data: ChangePasswordRequest): Promise<void> =>
  api.put('/api/account/password', data).then(r => r.data);

export const deleteAccount = (): Promise<void> =>
  api.delete('/api/account').then(r => r.data);
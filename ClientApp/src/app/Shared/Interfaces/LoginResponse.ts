export interface LoginResponse {
  bearerToken: string;
  refreshToken: string;
  bearerExpiresAt: Date;
  refreshExpiresAt: Date;
}

import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { LoginResponse } from './Shared/Interfaces/LoginResponse';
import {CookieService} from "ngx-cookie-service";
import {BearerToken} from "./Shared/Interfaces/BearerToken";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient,
              private cookies: CookieService,
              private router: Router) { }

  public login(username: string, password: string): void {
    this.http.post<LoginResponse>(`${this.baseUrl}/users/login`, {username: username, password: password}).subscribe(x => {
      this.saveTokens(x);
      const today = new Date();
      this.router.navigateByUrl(`year/${today.getFullYear()}/month/${today.getMonth() + 1}`);
    })
  }

  public refreshToken(): void {
    const refreshToken = this.cookies.get('refreshToken')
    this.http.post<LoginResponse>(`${this.baseUrl}/users/token`, {token: refreshToken}).subscribe(x => {
      this.saveTokens(x);
    }, err => {
      this.router.navigate(['login']);
    })
  }

  private saveTokens(tokens: LoginResponse): void {
    this.cookies.set('refreshToken', tokens.refreshToken, {sameSite: "Strict", expires: new Date(tokens.refreshExpiresAt)});
    const bearerToken = {token: tokens.bearerToken, expiresAt: tokens.bearerExpiresAt} as BearerToken;
    localStorage.setItem('bearerToken', JSON.stringify(bearerToken) );
  }
}

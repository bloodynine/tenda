import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {CookieService} from "ngx-cookie-service";
import {Router} from "@angular/router";
import { environment } from "../../../environments/environment";
import { LoginResponse } from "../Interfaces/LoginResponse";
import { BearerToken } from "../Interfaces/BearerToken";
import { Observable } from "rxjs";

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

  public refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.cookies.get('refreshToken')
    return this.http.post<LoginResponse>(`${this.baseUrl}/users/token`, {token: refreshToken});
  }

  public saveTokens(tokens: LoginResponse): void {
    this.cookies.set('refreshToken', tokens.refreshToken, {sameSite: "Strict", expires: new Date(tokens.refreshExpiresAt), path: '/'});
    const bearerToken = {token: tokens.bearerToken, expiresAt: tokens.bearerExpiresAt} as BearerToken;
    localStorage.setItem('bearerToken', JSON.stringify(bearerToken) );
  }
}

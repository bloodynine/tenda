import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { BearerToken } from "./Shared/Interfaces/BearerToken";
import { LoginService } from "./Shared/Services/login.service";
import { catchError, filter, switchMap } from "rxjs/operators";
import { StateService } from "./state.service";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  private refreshToken$: BehaviorSubject<any> = new BehaviorSubject<any>("defaultval");
  private isRefreshing: boolean = false;

  constructor(
    private loginService: LoginService,
  ) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.url.includes('login') || request.url.includes('token')) {
      return next.handle(request);
    }
    let bearerToken = this.GetBearerToken();
    if (bearerToken && new Date(bearerToken.expiresAt) > new Date()) {
      return next.handle(this.AddAuthToRequest(request, bearerToken.token));
    }
    return this.refreshToken(request, next);
  }

  private AddAuthToRequest(request: HttpRequest<any>, bearerToken: string): HttpRequest<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${bearerToken}`)
    return request.clone({ headers: headers });
  }

  private GetBearerToken(): BearerToken | undefined {
    const tokenString = localStorage.getItem('bearerToken');
    if (tokenString) {
      let bearerToken: BearerToken;
      bearerToken = JSON.parse(tokenString);
      return bearerToken;
    }
    return undefined;
  }

  private refreshToken(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshToken$.next(null);

      return this.loginService.refreshToken().pipe(
        switchMap((token) => {
          this.isRefreshing = false;
          this.refreshToken$.next(token.bearerToken);
          this.loginService.saveTokens(token);
          const bearer = this.GetBearerToken();
          return next.handle(this.AddAuthToRequest(request, bearer!.token))
        }), catchError(e => {
          this.isRefreshing = false;
          this.refreshToken$.next("");
          this.loginService.SignOut();
          return next.handle(request.clone({url: ""}))
        }))
    } else {
      return this.refreshToken$.pipe(filter(x => x != null),
        switchMap(jwt => {
          return next.handle(this.AddAuthToRequest(request, jwt))
        }))
    }

  }
}

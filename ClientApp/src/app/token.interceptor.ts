import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Observable} from 'rxjs';
import {LoginService} from "./login.service";
import {BearerToken} from "./Shared/Interfaces/BearerToken";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private loginService: LoginService,
  ) {
  }
  isRefreshingToken: boolean = false;
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    let bearerToken = TokenInterceptor.GetBearerToken();
    if (bearerToken && !this.isRefreshingToken) {
      if (new Date(bearerToken.expiresAt) > new Date()) {
        return next.handle(TokenInterceptor.AddAuthToRequest(request, bearerToken));
      } else {
        this.isRefreshingToken = true;
        this.loginService.refreshToken();
        bearerToken = TokenInterceptor.GetBearerToken();
        if (bearerToken) {
          this.isRefreshingToken = false;
          return next.handle(TokenInterceptor.AddAuthToRequest(request, bearerToken));
        }
      }
    }
    return next.handle(request);
  }

  private static AddAuthToRequest(request: HttpRequest<any>, bearerToken: BearerToken): HttpRequest<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${bearerToken.token}`)
    return request.clone({headers: headers});
  }


  private static GetBearerToken(): BearerToken | undefined {
    const tokenString = localStorage.getItem('bearerToken');
    if (tokenString) {
      let bearerToken: BearerToken;
      bearerToken = JSON.parse(tokenString);
      return bearerToken;
    }
    return undefined;
  }
}

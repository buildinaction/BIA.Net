import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AuthService } from '../services/auth.service';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(public authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (request.url.indexOf(environment.urlLogin) > -1 || request.url.indexOf(environment.urlLog) > -1) {
      return next.handle(request);
    }
    if (this.isRefreshing === false) {
      return this.launchRequest(request, next);
    } else {
      return this.waitLogin(request, next);
    }
  }

  private launchRequest(request: HttpRequest<any>, next: HttpHandler) {
    const jwtToken = this.authService.getToken();
    request = this.addToken(request, jwtToken);

    return next.handle(request).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(request, next);
        } else {
          return throwError(error);
        }
      })
    );
  }

  private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      withCredentials: false,
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (this.isRefreshing === false) {
      return this.login(request, next);
    } else {
      return this.waitLogin(request, next);
    }
  }

  private login(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.isRefreshing = true;
    this.authService.logout();
    return this.authService.login().pipe(
      switchMap((authInfo: AuthInfo) => {
        this.isRefreshing = false;
        return next.handle(this.addToken(request, authInfo.token));
      })
    );
  }

  private waitLogin(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.authInfo$.pipe(
      take(1),
      switchMap((authInfo) => {
        return next.handle(this.addToken(request, authInfo ? authInfo.token : ''));
      })
    );
  }
}

export const biaTokenInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: TokenInterceptor,
  multi: true
};

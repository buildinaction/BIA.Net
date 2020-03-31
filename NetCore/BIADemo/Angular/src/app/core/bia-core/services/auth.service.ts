import { Injectable, Injector } from '@angular/core';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map, filter } from 'rxjs/operators';
import { AbstractDas } from './abstract-das.service';
import { AuthInfo } from 'src/app/shared/bia-shared/model/auth-info';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends AbstractDas<AuthInfo> {
  private authInfoSubject: BehaviorSubject<AuthInfo | null> = new BehaviorSubject<AuthInfo | null>(null);
  public authInfo$: Observable<AuthInfo | null> = this.authInfoSubject
    .asObservable()
    .pipe(filter((authInfo: AuthInfo | null) => authInfo !== null && authInfo !== undefined));

  constructor(injector: Injector) {
    super(injector, 'Auth');
  }

  public logout() {
    this.authInfoSubject.next(null);
  }

  public login(): Observable<AuthInfo> {
    return this.http.get<AuthInfo>(this.route + 'login').pipe(
      map((authInfo: AuthInfo) => {
        this.authInfoSubject.next(authInfo);
        return authInfo;
      })
    );
  }

  public hasPermission(permission: string): boolean {
    return this.checkPermission(this.authInfoSubject.value, permission);
  }

  public hasPermissionObs(permission: string): Observable<boolean> {
    if (!permission) {
      return of(true);
    }
    return this.authInfo$.pipe(
      map((authInfo: AuthInfo | null) => {
        return this.checkPermission(authInfo, permission);
      })
    );
  }

  public getToken(): string {
    const authInfo = this.authInfoSubject.value;
    if (authInfo) {
      return authInfo.token;
    }
    return '';
  }

  private checkPermission(authInfo: AuthInfo | null, permission: string) {
    if (!permission) {
      return true;
    }
    if (authInfo) {
      return authInfo.permissions.some((p) => p === permission) === true;
    }
    return false;
  }
}

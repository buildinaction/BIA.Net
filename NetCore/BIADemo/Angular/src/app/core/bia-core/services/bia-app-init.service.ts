import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from './auth.service';
import { Subscription, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BiaAppInitService implements OnDestroy {
  private sub: Subscription;
  constructor(private authService: AuthService) {}
  Init() {
    return new Promise<void>((resolve, reject) => {
      this.sub = this.authService
        .login()
        .pipe(
          catchError((error) => {
            window.location.href = environment.urlErrorPage + '?num=' + error.status;
            return throwError(error);
          })
        )
        .subscribe(() => resolve());
    });
  }

  ngOnDestroy() {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}

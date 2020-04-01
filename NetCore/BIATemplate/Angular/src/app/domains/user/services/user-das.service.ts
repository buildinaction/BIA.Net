import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class UserDas extends AbstractDas<User> {
  constructor(injector: Injector) {
    super(injector, 'users');
  }

  public getAllByFilter(filter: string): Observable<Array<User>> {
    return this.http.get<Array<User>>(`${this.route}?filter=${filter}`);
  }

  public getAllADByFilter(filter: string): Observable<Array<User>> {
    return this.http.get<Array<User>>(`${this.route}fromAD?filter=${filter}`);
  }

  public synchronize() {
    return this.http.get(`${this.route}synchronize`);
  }
}

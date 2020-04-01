import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { View } from '../model/view';
import { DefaultView } from '../model/defaultView';

@Injectable({
  providedIn: 'root'
})
export class ViewDas extends AbstractDas<View> {
  constructor(injector: Injector) {
    super(injector, 'views');
  }

  public getAll(): Observable<Array<View>> {
    return this.http.get<Array<View>>(`${this.route}`);
  }

  public deleteUserView(id: number) {
    return this.http.delete<void>(`${this.route}userView/${id}`);
  }

  public setDefaultUserView(defaultView: DefaultView) {
    return this.http.put<void>(`${this.route}setDefaultUserView`, defaultView);
  }
}

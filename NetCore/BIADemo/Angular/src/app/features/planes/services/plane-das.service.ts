import { Injectable, Injector } from '@angular/core';
import { Plane, PlaneListItem } from '../model/plane';
import { AbstractDas } from 'src/app/core/bia-core/services/abstract-das.service';
import { LazyLoadEvent } from 'primeng';
import { Observable } from 'rxjs';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

@Injectable({
  providedIn: 'root'
})
export class PlaneDas extends AbstractDas<Plane> {
  constructor(injector: Injector) {
    super(injector, 'Planes');
  }
  getAllByPost(event: LazyLoadEvent): Observable<DataResult<PlaneListItem[]>> {
    return this.getListItemsByPost<PlaneListItem>(event);
  }
}

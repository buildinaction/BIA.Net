import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, pluck, switchMap, startWith } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  failure,
  load,
  loadAll,
  loadAllSuccess,
  loadSuccess
} from './member-roles-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { RoleDas } from '../services/role-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class RolesEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAll) /* When action is dispatched */,
      startWith(loadAll()),
      /* Hit the Roles Index endpoint of our REST API */
      /* Dispatch LoadAllSuccess action to the central store with id list returned by the backend as id*/
      /* 'Roles Reducers' will take care of the rest */
      switchMap(() =>
        this.roleDas.getList().pipe(
          map((roles) => loadAllSuccess({ roles })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  load$ = createEffect(() =>
    this.actions$.pipe(
      ofType(load),
      pluck('id'),
      switchMap((id) =>
        this.roleDas.get(id).pipe(
          map((role) => loadSuccess({ role })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        )
      )
    )
  );

  constructor(private actions$: Actions, private roleDas: RoleDas, private biaMessageService: BiaMessageService) {}
}

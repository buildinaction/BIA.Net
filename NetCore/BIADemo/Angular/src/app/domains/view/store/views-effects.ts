import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError, map, switchMap, pluck } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { failure, loadAllSuccess, loadAll, removeUserView, setDefaultUserView } from './views-actions';
import { BiaMessageService } from 'src/app/core/bia-core/services/bia-message.service';
import { ViewDas } from '../services/view-das.service';

/**
 * Effects file is for isolating and managing side effects of the application in one place
 * Http requests, Sockets, Routing, LocalStorage, etc
 */

@Injectable()
export class ViewsEffects {
  loadAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadAll) /* When action is dispatched */,
      switchMap((action) => {
        return this.viewDas.getAll().pipe(
          map((views) => loadAllSuccess({ views })),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  deleteView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(removeUserView) /* When action is dispatched */,
      pluck('id'),
      switchMap((id) => {
        return this.viewDas.deleteUserView(id).pipe(
          map(() => {
            this.biaMessageService.showDeleteSuccess();
            return loadAll();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  setDefaultUserView$ = createEffect(() =>
    this.actions$.pipe(
      ofType(setDefaultUserView),
      switchMap((action) => {
        return this.viewDas.setDefaultUserView(action).pipe(
          map(() => {
            this.biaMessageService.showUpdateSuccess();
            return loadAll();
          }),
          catchError((err) => {
            this.biaMessageService.showError();
            return of(failure({ error: err }));
          })
        );
      })
    )
  );

  constructor(private actions$: Actions, private viewDas: ViewDas, private biaMessageService: BiaMessageService) {}
}

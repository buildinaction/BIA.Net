import { createAction, props } from '@ngrx/store';
import { BIALazyLoadEvent } from 'src/app/shared/bia-shared/model/bia-lazyloadEvent';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes] Load all by post (PageMode)', props<{ event: BIALazyLoadEvent }>());

export const load = createAction('[Planes] Load (PageMode)', props<{ id: number }>());

export const create = createAction('[Planes] Create (PageMode)', props<{ plane: Plane }>());

export const update = createAction('[Planes] Update (PageMode)', props<{ plane: Plane }>());

export const remove = createAction('[Planes] Remove (PageMode)', props<{ id: number }>());

export const multiRemove = createAction('[Planes] Multi Remove (PageMode)', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes] Load all by post success (PageMode)',
  props<{ result: DataResult<Plane[]>; event: BIALazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes] Load success (PageMode)', props<{ plane: Plane }>());

export const failure = createAction('[Planes] Failure (PageMode)', props<{ error: any }>());







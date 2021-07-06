import { createAction, props } from '@ngrx/store';
import { BIALazyLoadEvent } from 'src/app/shared/bia-shared/model/bia-lazyloadEvent';
import { Plane } from '../model/plane';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Planes] Load all by post', props<{ event: BIALazyLoadEvent }>());

export const load = createAction('[Planes] Load', props<{ id: number }>());

export const create = createAction('[Planes] Create', props<{ plane: Plane }>());

export const update = createAction('[Planes] Update', props<{ plane: Plane }>());

export const remove = createAction('[Planes] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Planes] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Planes] Load all by post success',
  props<{ result: DataResult<Plane[]>; event: BIALazyLoadEvent }>()
);

export const loadSuccess = createAction('[Planes] Load success', props<{ plane: Plane }>());

export const failure = createAction('[Planes] Failure', props<{ error: any }>());

export const openDialogEdit = createAction('[Planes] Open dialog edit');

export const closeDialogEdit = createAction('[Planes] Close dialog edit');

export const openDialogNew = createAction('[Planes] Open dialog new');

export const closeDialogNew = createAction('[Planes] Close dialog new');

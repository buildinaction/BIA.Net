import { createAction, props } from '@ngrx/store';
import { BIALazyLoadEvent } from 'src/app/shared/bia-shared/model/bia-lazyloadEvent';
import { PlaneType } from '../model/plane-type';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[PlanesTypes] Load all by post', props<{ event: BIALazyLoadEvent }>());

export const load = createAction('[PlanesTypes] Load', props<{ id: number }>());

export const create = createAction('[PlanesTypes] Create', props<{ planeType: PlaneType }>());

export const update = createAction('[PlanesTypes] Update', props<{ planeType: PlaneType }>());

export const remove = createAction('[PlanesTypes] Remove', props<{ id: number }>());

export const multiRemove = createAction('[PlanesTypes] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[PlanesTypes] Load all by post success',
  props<{ result: DataResult<PlaneType[]>; event: BIALazyLoadEvent }>()
);

export const loadSuccess = createAction('[PlanesTypes] Load success', props<{ planeType: PlaneType }>());

export const failure = createAction('[PlanesTypes] Failure', props<{ error: any }>());

export const openDialogEdit = createAction('[PlanesTypes] Open dialog edit');

export const closeDialogEdit = createAction('[PlanesTypes] Close dialog edit');

export const openDialogNew = createAction('[PlanesTypes] Open dialog new');

export const closeDialogNew = createAction('[PlanesTypes] Close dialog new');

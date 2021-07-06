import { createAction, props } from '@ngrx/store';
import { BIALazyLoadEvent } from 'src/app/shared/bia-shared/model/bia-lazyloadEvent';
import { Site } from '../model/site/site';
import { SiteInfo } from '../model/site/site-info';
import { DataResult } from 'src/app/shared/bia-shared/model/data-result';

export const loadAllByPost = createAction('[Sites] Load all by post', props<{ event: BIALazyLoadEvent }>());

export const load = createAction('[Sites] Load', props<{ id: number }>());

export const create = createAction('[Sites] Create', props<{ site: Site }>());

export const update = createAction('[Sites] Update', props<{ site: Site }>());

export const remove = createAction('[Sites] Remove', props<{ id: number }>());

export const multiRemove = createAction('[Sites] Multi Remove', props<{ ids: number[] }>());

export const loadAllByPostSuccess = createAction(
  '[Sites] Load all by post success',
  props<{ result: DataResult<SiteInfo[]>; event: BIALazyLoadEvent }>()
);

export const loadSuccess = createAction('[Sites] Load success', props<{ site: Site }>());

export const failure = createAction('[Sites] Failure', props<{ error: any }>());

export const openDialogEdit = createAction('[Sites] Open dialog edit');

export const closeDialogEdit = createAction('[Sites] Close dialog edit');

export const openDialogNew = createAction('[Sites] Open dialog new');

export const closeDialogNew = createAction('[Sites] Close dialog new');

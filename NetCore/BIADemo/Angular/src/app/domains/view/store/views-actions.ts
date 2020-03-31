import { createAction, props } from '@ngrx/store';
import { View } from '../model/view';
import { DefaultView } from '../model/defaultView';

export const loadAll = createAction('[Domain Views] Load all');

export const loadAllSuccess = createAction('[Domain Views] Load all success', props<{ views: View[] }>());

export const removeUserView = createAction('[Domain Views] Remove user view', props<{ id: number}>());

export const setDefaultUserView = createAction('[Domain Views] Set default user view', props<DefaultView>());

export const failure = createAction('[Domain Views] Failure', props<{ error: any }>());

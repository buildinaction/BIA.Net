import { createAction, props } from '@ngrx/store';
import { Role } from '../model/role';

export const loadAllRoles = createAction('[Domain Roles] Load all');

export const load = createAction('[Domain Roles] Load', props<{ id: number }>());

export const loadAllSuccess = createAction('[Domain Roles] Load all success', props<{ roles: Role[] }>());

export const loadSuccess = createAction('[Domain Roles] Load success', props<{ role: Role }>());

export const failure = createAction('[Domain Roles] Failure', props<{ error: any }>());

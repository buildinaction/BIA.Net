import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllSuccess } from './views-actions';
import { View } from '../model/view';

// This adapter will allow is to manipulate views (mostly CRUD operations)
export const viewsAdapter = createEntityAdapter<View>({
  selectId: (view: View) => view.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<View> {
//   ids: string[] | number[];
//   entities: { [id: string]: View };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<View> {
  // additional props here
}

export const INIT_STATE: State = viewsAdapter.getInitialState({
  // additional props default values here
});

export const viewReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { views }) => viewsAdapter.addAll(views, state))
);

export const getViewById = (id: number) => (state: State) => state.entities[id];
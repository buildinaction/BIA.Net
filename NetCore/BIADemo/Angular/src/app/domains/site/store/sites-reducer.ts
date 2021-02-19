import { EntityState, createEntityAdapter } from '@ngrx/entity';
import { createReducer, on } from '@ngrx/store';
import { loadAllSuccess, loadSuccess } from './sites-actions';
import { Site } from '../model/site';

// This adapter will allow is to manipulate sites (mostly CRUD operations)
export const sitesAdapter = createEntityAdapter<Site>({
  selectId: (site: Site) => site.id,
  sortComparer: false
});

// -----------------------------------------
// The shape of EntityState
// ------------------------------------------
// interface EntityState<Site> {
//   ids: string[] | number[];
//   entities: { [id: string]: Site };
// }
// -----------------------------------------
// -> ids arrays allow us to sort data easily
// -> entities map allows us to access the data quickly without iterating/filtering though an array of objects

export interface State extends EntityState<Site> {
  // additional props here
}

export const INIT_STATE: State = sitesAdapter.getInitialState({
  // additional props default values here
});

export const siteReducers = createReducer<State>(
  INIT_STATE,
  on(loadAllSuccess, (state, { sites }) => sitesAdapter.setAll(sites, state)),
  on(loadSuccess, (state, { site }) => sitesAdapter.upsertOne(site, state))
);

export const getSiteById = (id: number) => (state: State) => state.entities[id];



















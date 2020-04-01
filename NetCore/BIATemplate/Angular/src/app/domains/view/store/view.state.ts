import * as fromViews from './views-reducer';
import { Action, combineReducers, createFeatureSelector, createSelector } from '@ngrx/store';

export interface ViewsState {
  views: fromViews.State;
}

/** Provide reducers with AoT-compilation compliance */
export function reducers(state: ViewsState | undefined, action: Action) {
  return combineReducers({
    views: fromViews.viewReducers
  })(state, action);
}

/**
 * The createFeatureSelector function selects a piece of state from the root of the state object.
 * This is used for selecting feature states that are loaded eagerly or lazily.
 */

export const getViewsState = createFeatureSelector<ViewsState>('domain-views');

export const getViewsEntitiesState = createSelector(
  getViewsState,
  (state) => state.views
);

export const { selectAll: getAllViews } = fromViews.viewsAdapter.getSelectors(
  getViewsEntitiesState
);

export const getViewById = (id: number) =>
  createSelector(
    getViewsEntitiesState,
    fromViews.getViewById(id)
  );

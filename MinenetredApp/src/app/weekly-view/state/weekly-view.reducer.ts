import * as fromRoot from '../../Core/app.state';
import { WeeklyViewActionTypes, WeeklyViewActions } from '../state/weeklyView.actions'
import { createFeatureSelector, createSelector } from '@ngrx/store';

export interface State extends fromRoot.State {
    weeklyView: WeeklyViewState;
}
export interface WeeklyViewState {
    WarningMessage: string;
    SelectedProjectName: string;
    date: string;
}

const initialState: WeeklyViewState = {
    WarningMessage: '',
    SelectedProjectName: '',
    date: '',
}
const getWeeklyViewState = createFeatureSelector<WeeklyViewState>('weeklyView');

export const getWarningMessage = createSelector(
    getWeeklyViewState,
    state => state.WarningMessage
);
export const getSelectedDate = createSelector(
    getWeeklyViewState,
    state => state.date
);
export const getProjectName = createSelector(
    getWeeklyViewState,
    state => state.SelectedProjectName
);

export function reducer(state = initialState, action: WeeklyViewActions): WeeklyViewState {
    switch (action.type) {
        case WeeklyViewActionTypes.SetWarningMessage:
            return {
                ...state,
                WarningMessage: action.payload
            };
        case WeeklyViewActionTypes.SetSelectedProjectName:
            return {
                ...state,
                SelectedProjectName: action.payload
            };
        case WeeklyViewActionTypes.SetFormatedDate:
            if (action.payload === '' || action.payload.length <= 10) {
                return {
                    ...state,
                    date: action.payload.substr(action.payload.length - 10)
                };
            }
            return {
                ...state,
                date: action.payload.substr(action.payload.length - 10)
            };
        default:
            return state;
    }
}
import * as fromRoot  from '../../Core/app.state';
import {WeeklyViewActionTypes, WeeklyViewActions} from '../state/weeklyView.actions'
import { createFeatureSelector, createSelector } from '@ngrx/store';
export interface State extends fromRoot.State{
    WeeklyView: WeeklyViewState;
}
export interface WeeklyViewState{
    WarningMessage : string;
}

const initialState : WeeklyViewState = {
    WarningMessage : '',
}
const getWeeklyViewState = createFeatureSelector<WeeklyViewState>('WeeklyView');

export const getWarningMessage = createSelector(
    getWeeklyViewState,
     state => state.WarningMessage
     );

export function reducer(state = initialState, action : WeeklyViewActions) : WeeklyViewState {
    switch (action.type){
        case WeeklyViewActionTypes.SetWarningMessage :
        return{
            ...state,
            WarningMessage : action.payload
        };
        default :
        return state;
    }
}
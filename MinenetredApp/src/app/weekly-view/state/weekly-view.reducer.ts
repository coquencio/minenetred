import * as fromRoot  from '../../Core/app.state';
export interface State extends fromRoot.State{
    WeeklyView: WeeklyViewState;
}
export interface WeeklyViewState{
    WarningMessage : string;
}

export function reducer(state : WeeklyViewState, action) : WeeklyViewState {
    switch (action.type){
        case 'SET_WARNING_MESSAGE' :
        return{
            ...state,
            WarningMessage : action.payload
        };
        default :
        return state;
    }
}
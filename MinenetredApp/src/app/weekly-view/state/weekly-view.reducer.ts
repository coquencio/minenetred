import * as fromRoot  from '../../Core/app.state';
export interface State extends fromRoot.State{
    WeeklyView: WeeklyViewState;
}
export interface WeeklyViewState{
    WarningMessage : string;
}

const initialState : WeeklyViewState = {
    WarningMessage : '',
}

export function reducer(state = initialState, action) : WeeklyViewState {
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
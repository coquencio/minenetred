import { Action } from '@ngrx/store';

export enum WeeklyViewActionTypes{
    SetWarningMessage = 'SET_WARNING_MESSAGE',
    SetSelectedProjectName = 'SET_SELECTED_PROJECT_NAME',
    SetFormatedDate = 'SET_FORMATED_DATE',

}

export class SetWarningMessage implements Action{
    readonly type = WeeklyViewActionTypes.SetWarningMessage;
    constructor (public payload : string){}
}
export class SetSelectedProjectName implements Action{
    readonly type = WeeklyViewActionTypes.SetSelectedProjectName;
    constructor (public payload : string){}
}
export class SetFormatedDate implements Action{
    readonly type = WeeklyViewActionTypes.SetFormatedDate;
    constructor (public payload : string){}
}
export type WeeklyViewActions = SetWarningMessage | SetSelectedProjectName | SetFormatedDate;
// Union type needed if more actions will be added "| exampleActionClass"
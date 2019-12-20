import { Action } from '@ngrx/store';

export enum WeeklyViewActionTypes{
    SetWarningMessage = 'SET_WARNING_MESSAGE',
}

export class SetWarningMessage implements Action{
    readonly type = WeeklyViewActionTypes.SetWarningMessage;
    constructor (public payload : string){}
}

export type WeeklyViewActions = SetWarningMessage;
//Union type needed if more actions will be added "| exampleActionClass" 
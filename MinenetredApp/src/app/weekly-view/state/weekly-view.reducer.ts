export function reducer(state, action){
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
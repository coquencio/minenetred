export function reducer(state, action){
    switch (action.type){
        
        case 'SET_WARNING_MESSAGE' : 
        console.log('existing state: ' + state);
        console.log('payload: ' + action.payload);
        return{
            ...state,
            WarningMessage : action.payload
            
        };
        default :
        return state;
    }
}
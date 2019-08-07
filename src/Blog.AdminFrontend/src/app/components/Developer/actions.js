import { useReducer } from 'react'

function useActions(reducer) {
   const [state, dispatch] = useReducer(reducer, reducer())
   return [state, actions(dispatch)]
}

function actions(dispatch) {
   return {
      load: result => dispatch({ type: 'LOAD', result }),
      updateIds: data => dispatch({ type: 'UPDATE_IDS', data }),
      toSaveMode: () => dispatch({ type: 'GOTO_SAVE_MODE' }),
      showErrors: data => dispatch({ type: 'SHOW_ERRORS', data }),
      restart: () => dispatch({ type: 'RESTART' }),
      updateDeveloper: change => dispatch({ type: 'UPDATE_DEVELOPER', change }),
      addExperience: () => dispatch({ type: 'ADD_EXPERIENCE' }),
      updateExperience: change => dispatch({ type: 'UPDATE_EXPERIENCE', change }),
      deleteExperience: id => dispatch({ type: 'DELETE_EXPERIENCE', id })
   }
}

export { useActions }

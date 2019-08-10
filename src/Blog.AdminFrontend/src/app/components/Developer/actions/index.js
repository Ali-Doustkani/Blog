import { useReducer } from 'react'
import { reducer, STATUS } from './reducer'

function useActions() {
   const [state, dispatch] = useReducer(reducer, reducer())
   return [state, actions(dispatch)]
}

function actions(dispatch) {
   return {
      load: result => dispatch({ type: 'LOAD', result }),
      updateIds: data => dispatch({ type: 'UPDATE_IDS', data }),
      showErrors: data => dispatch({ type: 'SHOW_ERRORS', data }),
      restart: () => dispatch({ type: 'RESTART' }),
      updateDeveloper: change => dispatch({ type: 'UPDATE_DEVELOPER', change }),
      addExperience: () => dispatch({ type: 'ADD_EXPERIENCE' }),
      updateExperience: change => dispatch({ type: 'UPDATE_EXPERIENCE', change }),
      deleteExperience: id => dispatch({ type: 'DELETE_EXPERIENCE', id }),
      removeServerErrors: () => dispatch({ type: 'REMOVE_SERVER_ERRORS' }),
      toSaving: () => dispatch({ type: 'TO_SAVING' }),
      toLoading: () => dispatch({ type: 'TO_LOADING' }),
      toIdle: () => dispatch({ type: 'TO_IDLE' })
   }
}

export { useActions, STATUS }

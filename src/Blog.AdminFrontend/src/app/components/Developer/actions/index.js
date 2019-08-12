import { useReducer } from 'react'
import { STATUS, initialState } from './initials'
import { dict, evolve } from '../../../utils/fn'
import { load } from './_loading'
import { addExperience, updateExperience, deleteExperience } from './_experiences'
import { showErrors, updateIds, updateDeveloper, prepareForSave } from './_developer'

const reducer = (state, action) => {
   const selectBy = dict(
      ['LOAD', load],
      ['ADD_EXPERIENCE', addExperience],
      ['DELETE_EXPERIENCE', deleteExperience],
      ['UPDATE_EXPERIENCE', updateExperience],
      ['UPDATE_DEVELOPER', updateDeveloper],
      ['UPDATE_IDS', updateIds],
      ['SHOW_ERRORS', showErrors],
      ['REMOVE_SERVER_ERRORS', prepareForSave],
      ['TO_IDLE', evolve({ status: STATUS.IDLE })],
      ['TO_SAVING', evolve({ status: STATUS.SAVING })],
      ['TO_LOADING', evolve({ status: STATUS.LOADING })]
   )
   return selectBy(action.type)(state, action)
}

function useActions() {
   const [state, dispatch] = useReducer(reducer, initialState)

   const actions = {
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

   return [state, actions]
}

export { useActions, STATUS }

import { useReducer } from 'react'
import uuid from 'uuid/v1'
import { STATUS, initialState } from './initials'
import { dict, addItem, updateItem, deleteItem, today } from '../../../utils/fn'
import { load } from './_loading'
import { updateIds, updateDeveloper } from './_developer'

const newExperience = () => ({
   id: uuid(),
   company: '',
   position: '',
   startDate: today(),
   endDate: '',
   content: ''
})

const newSideProject = () => ({
   id: uuid(),
   title: '',
   content: ''
})

const newEducation = () => ({
   id: uuid(),
   degree: '',
   university: '',
   startDate: '',
   endDate: ''
})

const reducer = (state, action) => {
   const selectBy = dict(
      ['LOAD', load],
      ['ADD_EXPERIENCE', addItem('experiences', newExperience)],
      ['DELETE_EXPERIENCE', deleteItem('experiences')],
      ['UPDATE_EXPERIENCE', updateItem('experiences')],
      ['ADD_SIDEPROJECT', addItem('sideProjects', newSideProject)],
      ['UPDATE_SIDEPROJECT', updateItem('sideProjects')],
      ['DELETE_SIDEPROJECT', deleteItem('sideProjects')],
      ['ADD_EDUCATION', addItem('educations', newEducation)],
      ['UPDATE_EDUCATION', updateItem('educations')],
      ['DELETE_EDUCATION', deleteItem('educations')],
      ['UPDATE_DEVELOPER', updateDeveloper],
      ['UPDATE_IDS', updateIds],
      ['TO_IDLE', state => ({ ...state, status: STATUS.IDLE })],
      ['TO_SAVING', state => ({ ...state, status: STATUS.SAVING })],
      ['TO_LOADING', state => ({ ...state, status: STATUS.LOADING })]
   )
   return selectBy(action.type)(state, action)
}

function useActions() {
   const [state, dispatch] = useReducer(reducer, initialState)

   const actions = {
      load: result => dispatch({ type: 'LOAD', result }),
      updateIds: data => dispatch({ type: 'UPDATE_IDS', data }),
      restart: () => dispatch({ type: 'RESTART' }),
      updateDeveloper: change => dispatch({ type: 'UPDATE_DEVELOPER', change }),
      addExperience: () => dispatch({ type: 'ADD_EXPERIENCE' }),
      updateExperience: change => dispatch({ type: 'UPDATE_EXPERIENCE', change }),
      deleteExperience: id => dispatch({ type: 'DELETE_EXPERIENCE', id }),
      addSideProject: () => dispatch({ type: 'ADD_SIDEPROJECT' }),
      updateSideProject: change => dispatch({ type: 'UPDATE_SIDEPROJECT', change }),
      deleteSideProject: id => dispatch({ type: 'DELETE_SIDEPROJECT', id }),
      addEducation: () => dispatch({ type: 'ADD_EDUCATION' }),
      updateEducation: change => dispatch({ type: 'UPDATE_EDUCATION', change }),
      deleteEducation: id => dispatch({ type: 'DELETE_EDUCATION', id }),
      toSaving: () => dispatch({ type: 'TO_SAVING' }),
      toLoading: () => dispatch({ type: 'TO_LOADING' }),
      toIdle: () => dispatch({ type: 'TO_IDLE' })
   }

   return [state, actions]
}

export { useActions, STATUS }

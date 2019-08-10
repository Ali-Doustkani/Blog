import uuid from 'uuid/v1'
import prepareForSave from './prepareForSave'
import { emptyValidator, richtextEmptyValidator, map } from '../../../utils'

const STATUS = {
   LOADING: 1,
   PREPARING_TO_SAVE: 2,
   SAVING: 3,
   IDLE: 4,
   STOPPED: 5
}

const initialState = {
   status: STATUS.LOADING,
   hasError: false,
   save: false,
   errorMessage: '',
   summary: '',
   summaryErrors: [],
   skills: '',
   skillsErrors: [],
   experiences: [],
   experiencesErrors: []
}

const load = (state, action) => {
   if (action.result.status === 'ok') {
      return action.result.data
         ? {
              ...state,
              status: STATUS.IDLE,
              ...action.result.data,
              experiences: action.result.data.experiences.map(validateExperience)
           }
         : { ...state, status: STATUS.IDLE, summary: '', experiences: [] }
   }
   return { ...state, status: STATUS.STOPPED, errorMessage: action.result.data }
}

const addExperience = state => {
   const now = new Date()
   const newExperience = validateExperience({
      id: uuid(),
      company: '',
      position: '',
      startDate: `${now.getFullYear()}-${String(now.getMonth()).padStart(2, 0)}-${String(
         now.getDay()
      ).padStart(2, 0)}`,
      endDate: ''
   })
   const experiences = [...state.experiences, newExperience]
   return { ...state, experiences }
}

const deleteExperience = (state, action) => {
   return { ...state, experiences: state.experiences.filter(x => x.id !== action.id) }
}

const validateExperience = experience => {
   experience.companyErrors = emptyValidator('company', experience.companyErrors)(
      experience.company
   )
   experience.positionErrors = emptyValidator('position', experience.positionErrors)(
      experience.position
   )

   experience.startDateErrors = emptyValidator('start date', experience.startDateErrors)(
      experience.startDate
   )
   experience.endDateErrors = emptyValidator('end date', experience.endDateErrors)(
      experience.endDate
   )
   experience.contentErrors = richtextEmptyValidator('content', experience.contentErrors)(
      experience.content
   )
   return experience
}

const updateExperience = (state, action) => {
   return {
      ...state,
      experiences: state.experiences.map(exp => {
         if (exp.id === action.change.id) {
            Object.assign(exp, action.change)
            validateExperience(exp)
            return exp
         }
         return exp
      })
   }
}

const updateDeveloper = (state, action) => {
   const newState = { ...state, ...action.change }
   newState.summaryErrors = richtextEmptyValidator('summary', state.summaryErrors)(newState.summary)
   newState.skillsErrors = emptyValidator('skills', state.skillsErrors)(newState.skills)
   return newState
}

const updateIds = (state, action) => {
   const ids = action.data
   return {
      ...state,
      status: STATUS.IDLE,
      experiences: state.experiences.map((exp, i) => ({ ...exp, id: ids.experiences[i] }))
   }
}

const showErrors = (state, action) => {
   const result = { ...state, status: STATUS.IDLE }
   map(action.data, result)
   return result
}

const reducer = (state, action) => {
   if (!state) {
      return initialState
   }

   switch (action.type) {
      case 'LOAD':
         return load(state, action)
      case 'ADD_EXPERIENCE':
         return addExperience(state)
      case 'DELETE_EXPERIENCE':
         return deleteExperience(state, action)
      case 'UPDATE_EXPERIENCE':
         return updateExperience(state, action)
      case 'UPDATE_DEVELOPER':
         return updateDeveloper(state, action)
      case 'UPDATE_IDS':
         return updateIds(state, action)
      case 'SHOW_ERRORS':
         return showErrors(state, action)
      case 'TO_PREPARE_FOR_SAVE':
         return prepareForSave(state)
      case 'TO_IDLE':
         return { ...state, status: STATUS.IDLE }
      case 'TO_SAVING':
         return { ...state, status: STATUS.SAVING }
      case 'TO_LOADING':
         return { ...state, status: STATUS.LOADING }
      default:
         return state
   }
}

export { reducer, STATUS }

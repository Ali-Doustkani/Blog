import uuid from 'uuid/v1'
import { isEmpty, isRichtextEmtpy } from '../utils/validations'

const load = action => {
   if (action.data.status === 'ok') {
      if (action.data.developer) {
         return { isLoading: false, ...action.data.developer }
      }
      return { isLoading: false, summary: '', experiences: [] }
   }
   return { isLoading: false, errorMessage: action.data.message }
}

const newExperience = state => {
   const now = new Date()
   const newExperience = {
      id: uuid(),
      company: '',
      position: '',
      startDate: `${now.getFullYear()}-${String(now.getMonth()).padStart(2, 0)}-${String(
         now.getDay()
      ).padStart(2, 0)}`,
      endDate: ''
   }
   const experiences = [...state.experiences, newExperience]
   return { ...state, experiences }
}

const deleteExperience = (state, action) => {
   return { ...state, experiences: state.experiences.filter(x => x.id !== action.id) }
}

const validateExperience = experience => {
   experience.companyError = isEmpty(experience.company)
   experience.positionError = isEmpty(experience.position)
   experience.startDateError = isEmpty(experience.startDate)
   experience.endDateError = isEmpty(experience.endDate)
   experience.contentError = isRichtextEmtpy(experience.content)
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

const extractData = state => {
   const { isLoading, ...developer } = state
   return developer
}

const updateDeveloper = (state, action) => {
   const newState = { ...state, ...action.change }
   newState.summaryError = isRichtextEmtpy(newState.summary)
   newState.skillsError = isEmpty(newState.skills)
   return newState
}

const reducer = (state, action) => {
   switch (action.type) {
      case 'RESTART':
         return { isLoading: true }
      case 'LOAD':
         return load(action)
      case 'NEW_EXPERIENCE':
         return newExperience(state)
      case 'DELETE_EXPERIENCE':
         return deleteExperience(state, action)
      case 'UPDATE_EXPERIENCE':
         return updateExperience(state, action)
      case 'EXTRACT_DATA':
         return extractData(state)
      case 'UPDATE_DEVELOPER':
         return updateDeveloper(state, action)
      default:
         return state
   }
}

export default reducer

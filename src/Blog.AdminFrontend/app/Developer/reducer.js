import uuid from 'uuid/v1'
import { isEmpty, isRichtextEmtpy } from '../utils/validations'

const load = (state, action) => {
   if (action.result.status === 'ok') {
      return action.result.data
         ? { ...state, isLoading: false, ...action.result.data }
         : { ...state, isLoading: false, summary: '', experiences: [] }
   }
   return { ...state, isLoading: false, errorMessage: action.result.data }
}

const newExperience = state => {
   const now = new Date()
   const newExperience = {
      id: uuid(),
      company: '',
      companyErrors: [],
      position: '',
      positionErrors: [],
      startDate: `${now.getFullYear()}-${String(now.getMonth()).padStart(2, 0)}-${String(
         now.getDay()
      ).padStart(2, 0)}`,
      startDateErrors: [],
      endDate: '',
      endDateErrors: []
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

const updateDeveloper = (state, action) => {
   const newState = { ...state, ...action.change }
   if (isRichtextEmtpy(newState.summary)) {
      newState.summaryErrors = ['summary is required']
   }
   if (isEmpty(newState.skills)) {
      newState.skillsErrors = ['skills is required']
   }
   return newState
}

const updateIds = (state, action) => {
   const ids = action.data
   return {
      ...state,
      experiences: state.experiences.map((exp, i) => ({ ...exp, id: ids.experiences[i] }))
   }
}

const showErrors = (state, action) => {
   return {
      ...state,
      summaryErrors: action.data.summary || [],
      skillsErrors: action.data.skills || []
   }
}

const reducer = (state, action) => {
   switch (action.type) {
      case 'RESTART':
         return { isLoading: true }
      case 'LOAD':
         return load(state, action)
      case 'NEW_EXPERIENCE':
         return newExperience(state)
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
      default:
         return state
   }
}

export default reducer

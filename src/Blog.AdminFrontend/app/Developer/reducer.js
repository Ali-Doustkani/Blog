import uuid from 'uuid/v1'

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

const assignFrom = source => target =>
   target.id === source.id ? Object.assign(target, source) : target

const updateExperience = (state, action) => {
   return { ...state, experiences: state.experiences.map(assignFrom(action.experience)) }
}

const extractData = state => {
   const { isLoading, ...developer } = state
   return developer
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
   }
}

export default reducer

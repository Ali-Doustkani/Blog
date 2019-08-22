import uuid from 'uuid/v1'
import { update } from '../../../utils/fn'

const createExperience = () => ({
   id: uuid(),
   company: '',
   position: '',
   startDate: today(),
   endDate: '',
   content: ''
})

const addExperience = state => ({
   ...state,
   experiences: [...state.experiences, createExperience()]
})

const deleteExperience = (state, action) => ({
   ...state,
   experiences: state.experiences.filter(x => x.id !== action.id)
})

const updateExperience = (state, action) => ({
   ...state,
   experiences: state.experiences.map(update(action.change))
})

function today() {
   const now = new Date()
   return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, 0)}-${String(
      now.getDate()
   ).padStart(2, 0)}`
}

export { addExperience, updateExperience, deleteExperience }

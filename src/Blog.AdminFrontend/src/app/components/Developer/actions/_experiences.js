import { evolve, take } from '../../../utils/fn'
import uuid from 'uuid/v1'

const initialExperienceErrors = experience => ({
   ...experience,
   companyErrors: [],
   positionErrors: [],
   startDateErrors: [],
   endDateErrors: [],
   contentErrors: []
})

const createExperience = () =>
   initialExperienceErrors({
      id: uuid(),
      company: '',
      position: '',
      startDate: today(),
      endDate: ''
   })

const addExperience = evolve(
   take(op => ({ experiences: [...op.state.experiences, createExperience()] }))
)

const deleteExperience = evolve(
   take(op => ({ experiences: op.state.experiences.filter(x => x.id !== op.action.id) }))
)

const updateExperience = evolve(
   take(op => ({ experiences: op.state.experiences.map(update(op.action.change)) }))
)

const update = change => experience => {
   if (change.id === experience.id) {
      const result = { ...experience, ...change }
      //validateExperience(result)
      return result
   }
   return experience
}

function today() {
   const now = new Date()
   return `${now.getFullYear()}-${String(now.getMonth()).padStart(2, 0)}-${String(
      now.getDay()
   ).padStart(2, 0)}`
}

export { addExperience, updateExperience, deleteExperience, initialExperienceErrors }

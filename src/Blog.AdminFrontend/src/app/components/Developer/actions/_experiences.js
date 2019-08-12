import { evolve, take } from '../../../utils/fn'
import { emptyValidator, richtextEmptyValidator } from '../../../utils'
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
      validateExperience(result)
      return result
   }
   return experience
}

function validateExperience(experience) {
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

function today() {
   const now = new Date()
   return `${now.getFullYear()}-${String(now.getMonth()).padStart(2, 0)}-${String(
      now.getDay()
   ).padStart(2, 0)}`
}

export { addExperience, updateExperience, deleteExperience, initialExperienceErrors }

import { STATUS } from './reducer'

const clientError = err => err.type === 1

const selectClientErrors = experience => ({
   ...experience,
   companyErrors: experience.companyErrors.filter(clientError),
   positionErrors: experience.positionErrors.filter(clientError),
   startDateErrors: experience.startDateErrors.filter(clientError),
   endDateErrors: experience.endDateErrors.filter(clientError),
   contentErrors: experience.contentErrors.filter(clientError)
})

const anyError = state =>
   Boolean(
      state.summaryErrors.length ||
         state.skillsErrors.length ||
         state.experiencesErrors.length ||
         state.experiences.some(
            exp =>
               exp.companyErrors.length ||
               exp.positionErrors.length ||
               exp.startDateErrors.length ||
               exp.endDateErrors.length ||
               exp.contentErrors.length
         )
   )

const prepareForSave = state => {
   const newState = {
      ...state,
      status: STATUS.PREPARING_TO_SAVE,
      summaryErrors: state.summaryErrors.filter(clientError),
      skillsErrors: state.skillsErrors.filter(clientError),
      experiencesErrors: state.experiencesErrors.filter(clientError),
      experiences: state.experiences.map(selectClientErrors)
   }
   newState.hasError = anyError(newState)
   return newState
}

export default prepareForSave

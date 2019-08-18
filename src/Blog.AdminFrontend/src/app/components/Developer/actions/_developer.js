import { evolve, take } from '../../../utils/fn'
import { emptyValidator, richtextEmptyValidator } from '../../../utils'
import { writeErrors } from './_serverErrorWriter'
import { STATUS } from './initials'

const updateDeveloper = (state, action) => {
   const newState = { ...state, ...action.change }
   newState.summaryErrors = richtextEmptyValidator('summary', state.summaryErrors)(newState.summary)
   newState.skillsErrors = emptyValidator('skills', state.skillsErrors)(newState.skills)
   return newState
}

const updateIds = evolve(
   take(op => ({ experiences: op.state.experiences.map(setId(op.action.data.experiences)) })),
   { status: STATUS.IDLE }
)

const setId = ids => (exp, index) => ({ ...exp, id: ids[index] })

const showErrors = (state, action) => {
   const result = { ...state, status: STATUS.IDLE }
   writeErrors(action.data, result)
   return result
}

const clientError = err => err.type === 1

const clientErrors = op => ({
   companyErrors: op.state.companyErrors.filter(clientError),
   positionErrors: op.state.positionErrors.filter(clientError),
   startDateErrors: op.state.startDateErrors.filter(clientError),
   endDateErrors: op.state.endDateErrors.filter(clientError),
   contentErrors: op.state.contentErrors.filter(clientError)
})

const selectClientErrors = evolve(take(clientErrors))

const prepareForSave = evolve(
   take(op => ({
      summaryErrors: op.state.summaryErrors.filter(clientError),
      skillsErrors: op.state.skillsErrors.filter(clientError),
      experiencesErrors: op.state.experiencesErrors.filter(clientError),
      experiences: op.state.experiences.map(selectClientErrors)
   })),
   { status: STATUS.PREPARING_TO_SAVE }
)

export { updateDeveloper, updateIds, showErrors, prepareForSave }

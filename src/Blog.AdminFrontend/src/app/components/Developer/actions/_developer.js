import { writeErrors } from './_serverErrorWriter'
import { STATUS } from './initials'

const updateDeveloper = (state, action) => ({ ...state, ...action.change })

const updateIds = (state, action) => ({
   ...state,
   experiences: state.experiences.map(setId(action.data.experiences)),
   status: STATUS.IDLE
})

const setId = ids => (exp, index) => ({ ...exp, id: ids[index] })

const showErrors = (state, action) => {
   const result = { ...state, status: STATUS.IDLE }
   writeErrors(action.data, result)
   return result
}

const clientError = err => err.type === 1

const selectClientErrors = experience => ({
   ...experience,
   companyErrors: experience.companyErrors.filter(clientError),
   positionErrors: experience.positionErrors.filter(clientError),
   startDateErrors: experience.startDateErrors.filter(clientError),
   endDateErrors: experience.endDateErrors.filter(clientError),
   contentErrors: experience.contentErrors.filter(clientError)
})

const prepareForSave = state => ({
   ...state,
   summaryErrors: state.summaryErrors.filter(clientError),
   skillsErrors: state.skillsErrors.filter(clientError),
   experiencesErrors: state.experiencesErrors.filter(clientError),
   experiences: state.experiences.map(selectClientErrors),
   status: STATUS.PREPARING_TO_SAVE
})

export { updateDeveloper, updateIds, showErrors, prepareForSave }

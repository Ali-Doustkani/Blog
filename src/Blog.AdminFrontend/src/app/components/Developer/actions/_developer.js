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

export { updateDeveloper, updateIds, showErrors }

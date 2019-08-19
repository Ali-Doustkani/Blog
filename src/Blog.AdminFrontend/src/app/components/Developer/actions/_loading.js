import { initialState, STATUS } from './initials'
import { initialExperienceErrors } from './_experiences'
import { ifElse } from '../../../utils/fn'

const loadOK = ifElse(
   action => action.result.data,
   (state, action) => ({
      ...state,
      ...action.result.data,
      experiences: action.result.data.experiences.map(initialExperienceErrors),
      status: STATUS.IDLE
   }),
   () => ({ ...initialState, status: STATUS.IDLE })
)

const loadFatal = (state, action) => ({
   ...state,
   errorMessage: action.result.data,
   status: STATUS.STOPPED
})

const load = ifElse(action => action.result.status === 'ok', loadOK, loadFatal)

export { load }

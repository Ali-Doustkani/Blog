import { initialState, STATUS } from './initials'
import { ifElse } from '../../../utils/fn'

const loadOK = ifElse(
   action => action.result.data,
   (state, action) => ({
      ...state,
      ...action.result.data,
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

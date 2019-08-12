import { initialState, STATUS } from './initials'
import { initialExperienceErrors } from './_experiences'
import { evolve, evolveFrom, take, ifElse } from '../../../utils/fn'

const loadOK = ifElse(
   action => action.result.data,
   evolve(
      take(op => op.action.result.data),
      take(op => ({
         experiences: op.action.result.data.experiences.map(initialExperienceErrors)
      })),
      { status: STATUS.IDLE }
   ),
   evolveFrom(initialState, { status: STATUS.IDLE })
)

const loadFatal = evolve(take(op => ({ errorMessage: op.action.result.data })), {
   status: STATUS.STOPPED
})

const load = ifElse(action => action.result.status === 'ok', loadOK, loadFatal)

export { load }

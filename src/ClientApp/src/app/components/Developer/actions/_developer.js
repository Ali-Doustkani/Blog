import { STATUS } from './initials'

const updateDeveloper = (state, action) => ({ ...state, ...action.change })

const updateIds = (state, action) => ({
   ...state,
   experiences: state.experiences.map(setId(action.data.experiences)),
   status: STATUS.IDLE
})

const setId = ids => (exp, index) => ({ ...exp, id: ids[index] })

export { updateDeveloper, updateIds }

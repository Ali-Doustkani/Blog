import { updateDeveloper, updateIds } from './_developer'
import { initialState, STATUS } from './initials'

describe('updating', () => {
   it('updates fields', () => {
      let state = updateDeveloper(initialState, { change: { summary: 'summary' } })
      state = updateDeveloper(state, { change: { skills: 'C#' } })

      expect(state).toMatchSnapshot()
   })
})

describe('updating ids', () => {
   it('sets ids by order', () => {
      const state = {
         experiences: [
            {
               id: 'a',
               company: 'Parmis'
            },
            {
               id: 'b',
               company: 'Lodgify'
            }
         ]
      }
      const newState = updateIds(state, { data: { experiences: [1, 2] } })

      expect(newState.experiences[0].id).toBe(1)
      expect(newState.experiences[1].id).toBe(2)
   })

   it('sets status', () => {
      const state = {
         status: STATUS.LOADING,
         experiences: []
      }
      const newState = updateIds(state, { data: { experiences: [] } })

      expect(newState.status).toBe(STATUS.IDLE)
   })
})

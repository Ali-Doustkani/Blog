import { updateDeveloper, showErrors, updateIds, prepareForSave } from './_developer'
import { initialState, STATUS } from './initials'
import { richtextEmptyValidator, emptyValidator } from '../../../utils'

jest.mock('./_serverErrorWriter')
jest.mock('../../../utils')

describe('updating', () => {
   it('updates fields', () => {
      emptyValidator.mockImplementation(() => () => [])
      richtextEmptyValidator.mockImplementation(() => () => [])
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

describe('showErrors', () => {
   it('sets status to idle', () => {
      const initial = {
         status: STATUS.LOADING
      }
      const state = showErrors(initial, { data: null })

      expect(state.status).toBe(STATUS.IDLE)
   })
})

describe('prepare for saving', () => {
   it('removes all errors with server type', () => {
      const init = {
         summaryErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
         skillsErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
         experiencesErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
         experiences: [
            {
               companyErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
               positionErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
               startDateErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
               endDateErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
               contentErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }]
            }
         ]
      }

      const newState = prepareForSave(init)

      const errors = [{ type: 1, message: 'err1' }]
      expect(newState.summaryErrors).toEqual(errors)
      expect(newState.skillsErrors).toEqual(errors)
      expect(newState.experiencesErrors).toEqual(errors)
      expect(newState.experiences[0].companyErrors).toEqual(errors)
      expect(newState.experiences[0].positionErrors).toEqual(errors)
      expect(newState.experiences[0].startDateErrors).toEqual(errors)
      expect(newState.experiences[0].endDateErrors).toEqual(errors)
      expect(newState.experiences[0].contentErrors).toEqual(errors)
   })
})

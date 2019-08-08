import reducer from './reducer'
import { map } from '../../utils'
jest.mock('../../utils')

describe('loading', () => {
   const initial = {
      isLoading: true
   }

   it('create default values when no developer exists', () => {
      const newState = reducer(initial, {
         type: 'LOAD',
         result: {
            status: 'ok',
            data: null
         }
      })

      expect(newState.isLoading).toBe(false)
      expect(newState.summary).toBe('')
      expect(newState.experiences).toEqual([])
   })

   it('create error message on fatal happend', () => {
      const newState = reducer(initial, {
         type: 'LOAD',
         result: {
            status: 'fatal',
            data: 'Error Happened'
         }
      })

      expect(newState).toEqual({
         isLoading: false,
         errorMessage: 'Error Happened'
      })
   })
})

it('update experience ids', () => {
   const initial = {
      experiences: [
         {
            id: 1,
            company: 'Lodgify'
         },
         {
            id: 2,
            company: 'Parmis'
         }
      ]
   }

   const newState = reducer(initial, { type: 'UPDATE_IDS', data: { experiences: [101, 202] } })

   expect(newState.experiences.map(exp => exp.id)).toEqual([101, 202])
})

it('delete an existing experience', () => {
   const initial = {
      summary: 'will not change',
      experiences: [
         {
            id: 1,
            company: 'Lodgify',
            position: 'C# Developer',
            startDate: '2019-01-01',
            endDate: '2020-01-01'
         }
      ]
   }

   const newState = reducer(initial, { type: 'DELETE_EXPERIENCE', id: 1 })

   expect(newState.summary).toBe('will not change')
   expect(newState.experiences).toEqual([])
})

it('enables the form when showing errors', () => {
   map.mockImplementation((errors, state) => state)
   const init = { disabled: true }
   const newState = reducer(init, { type: 'SHOW_ERRORS', errors: null })

   expect(newState.disabled).toEqual(false)
})

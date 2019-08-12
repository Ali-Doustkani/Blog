import { addExperience, deleteExperience, updateExperience } from './_experiences'
import { initialState } from './initials'
import { emptyValidator, richtextEmptyValidator } from '../../../utils'
import uuid from 'uuid/v1'

jest.mock('uuid/v1')
jest.mock('../../../utils')

afterEach(jest.restoreAllMocks)

describe('adding', () => {
   it('adds a new valid experience', () => {
      uuid.mockReturnValue('1')
      jest.spyOn(global, 'Date').mockImplementation(() => ({
         getFullYear: () => 2012,
         getMonth: () => 5,
         getDay: () => 20
      }))

      const newState = addExperience(initialState)

      expect(newState.experiences[0].id).toBe('1')
      expect(newState.experiences[0].startDate).toBe('2012-05-20')
      expect(newState.experiences[0].endDate).toBe('')
      expect(newState).toMatchSnapshot()
   })

   it('sets the id of new experience', () => {
      uuid.mockReturnValue('123')
      const newState = addExperience(initialState)
      expect(newState.experiences[0].id).toBe('123')
   })

   it('sets startDate value to today', () => {
      jest.spyOn(global, 'Date').mockImplementation(() => ({
         getFullYear: () => 2019,
         getMonth: () => 5,
         getDay: () => 2
      }))
      const newState = addExperience(initialState)
      expect(newState.experiences[0].startDate).toBe('2019-05-02')
   })
})

describe('deleting', () => {
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

      const newState = deleteExperience(initial, { id: 1 })

      expect(newState.summary).toBe('will not change')
      expect(newState.experiences).toEqual([])
   })
})

describe('updating', () => {
   it('updates company fields', () => {
      uuid.mockReturnValue(1)
      emptyValidator.mockImplementation(() => () => [])
      richtextEmptyValidator.mockImplementation(() => () => [])
      let state = addExperience(initialState)
      state = updateExperience(state, { change: { id: 1, company: 'Parmis' } })
      state = updateExperience(state, { change: { id: 1, position: 'JS' } })
      state = updateExperience(state, { change: { id: 1, startDate: '2013-01-02' } })
      state = updateExperience(state, { change: { id: 1, endDate: '2015-01-01' } })
      state = updateExperience(state, { change: { id: 1, content: 'Description' } })

      expect(state).toMatchSnapshot()
   })

   it('validates company fields', () => {
      uuid.mockReturnValue(1)
      emptyValidator.mockImplementation(field => () => field + 'Error')
      richtextEmptyValidator.mockImplementation(field => () => field + 'Error')
      let state = addExperience(initialState)
      state = updateExperience(state, { change: { id: 1, company: 'Parmis' } })
      state = updateExperience(state, { change: { id: 1, position: 'JS' } })
      state = updateExperience(state, { change: { id: 1, startDate: '2013-01-02' } })
      state = updateExperience(state, { change: { id: 1, endDate: '2015-01-01' } })
      state = updateExperience(state, { change: { id: 1, content: 'Description' } })

      expect(state).toMatchSnapshot()
   })
})

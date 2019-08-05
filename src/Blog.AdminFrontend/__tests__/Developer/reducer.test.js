import reducer from '../../app/Developer/reducer'
import * as validators from '../../app/utils/validations'
import uuid from 'uuid/v1'
jest.mock('uuid/v1')
jest.mock('../../app/utils/validations')

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

   it('sets the developer info', () => {
      const data = {
         summary: 'life of a programmer',
         experiences: [
            {
               id: 1,
               position: 'back-end developer',
               company: 'Parmis',
               startDate: '2015-01-01',
               endDate: '2016-01-01'
            }
         ]
      }

      const newState = reducer(initial, {
         type: 'LOAD',
         result: { status: 'ok', data }
      })

      expect(newState.isLoading).toBe(false)
      expect(newState.summary).toBe(data.summary)
      expect(newState.experiences).toEqual(data.experiences)
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

describe('manipulating developer', () => {
   it('updates developer fields', () => {
      const initial = {
         summary: '<p></p>',
         skills: 'none',
         experiences: ''
      }

      const newState = reducer(initial, {
         type: 'UPDATE_DEVELOPER',
         change: { summary: '<p>New Summary</p>', skills: 'C#, JS' }
      })

      expect(newState.summary).toBe('<p>New Summary</p>')
      expect(newState.skills).toBe('C#, JS')
   })

   it('validate', () => {
      validators.isRichtextEmtpy.mockReturnValue(true)
      const initial = {
         summary: '<p>TEXT</p>'
      }

      const newState = reducer(initial, {
         type: 'UPDATE_DEVELOPER',
         change: { summary: '<p></p>' }
      })

      expect(newState.summaryErrors).toEqual(['summary is required'])
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

      const newState = reducer(initial, {
         type: 'UPDATE_IDS',
         data: { experiences: [101, 202] }
      })

      expect(newState.experiences.map(exp => exp.id)).toEqual([101, 202])
   })
})

describe('manipulating experiences', () => {
   it('add new experience', () => {
      global.Date = jest.fn(() => {
         return {
            getFullYear: () => 2019,
            getMonth: () => 8,
            getDay: () => 2
         }
      })
      uuid.mockReturnValue(123)
      const initial = {
         summary: 'will not change',
         experiences: []
      }
      const newState = reducer(initial, { type: 'NEW_EXPERIENCE' })

      expect(newState.summary).toBe('will not change')
      expect(newState.experiences).toEqual([
         {
            id: 123,
            company: '',
            companyErrors: [],
            position: '',
            positionErrors: [],
            startDate: '2019-08-02',
            startDateErrors: [],
            endDate: '',
            endDateErrors: []
         }
      ])
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

   it('update an existing experience', () => {
      validators.isEmpty.mockReturnValue(false)
      validators.isRichtextEmtpy.mockReturnValue(false)

      const initial = {
         experiences: [
            {
               id: 1,
               company: 'Bellin',
               position: 'C# Developer',
               startDate: '2019-01-01',
               endDate: '2020-01-01',
               content: '<p contenteditable>worked as a beck-end developer</p>'
            }
         ]
      }
      const change = {
         id: 1,
         company: 'Lodgify',
         position: 'Back-end developer',
         startDate: '2020-01-01',
         endDate: '2022-01-01'
      }

      const newState = reducer(initial, {
         type: 'UPDATE_EXPERIENCE',
         change
      })

      expect(newState.experiences).toEqual([
         {
            id: 1,
            company: 'Lodgify',
            companyError: false,
            position: 'Back-end developer',
            positionError: false,
            startDate: '2020-01-01',
            startDateError: false,
            endDate: '2022-01-01',
            endDateError: false,
            content: '<p contenteditable>worked as a beck-end developer</p>',
            contentError: false
         }
      ])
   })

   it('validate', () => {
      validators.isEmpty.mockImplementation(input => input === '')
      validators.isRichtextEmtpy.mockReturnValue(false)

      const state1 = {
         experiences: [
            {
               id: 1,
               company: 'Lodgify',
               position: 'C# Developer',
               startDate: '2019-01-01',
               endDate: '2020-01-01',
               content: '<p contenteditable="">worked as a developer</p>'
            }
         ]
      }

      const state2 = reducer(state1, {
         type: 'UPDATE_EXPERIENCE',
         change: {
            id: 1,
            company: ''
         }
      })

      const state3 = reducer(state2, {
         type: 'UPDATE_EXPERIENCE',
         change: {
            id: 1,
            position: ''
         }
      })

      expect(state3.experiences[0]).toEqual({
         id: 1,
         company: '',
         companyError: true,
         position: '',
         positionError: true,
         startDate: '2019-01-01',
         startDateError: false,
         endDate: '2020-01-01',
         endDateError: false,
         content: '<p contenteditable="">worked as a developer</p>',
         contentError: false
      })
   })
})

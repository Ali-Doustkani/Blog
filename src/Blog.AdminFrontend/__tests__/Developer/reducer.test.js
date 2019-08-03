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
         data: {
            status: 'ok',
            developer: null
         }
      })

      expect(newState.isLoading).toBe(false)
      expect(newState.summary).toBe('')
      expect(newState.experiences).toEqual([])
   })

   it('sets the developer info', () => {
      const developer = {
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
         data: { status: 'ok', developer }
      })

      expect(newState.isLoading).toBe(false)
      expect(newState.summary).toBe(developer.summary)
      expect(newState.experiences).toEqual(developer.experiences)
   })

   it('create error message when error happend', () => {
      const newState = reducer(initial, {
         type: 'LOAD',
         data: {
            status: 'error',
            message: 'Error Happened'
         }
      })

      expect(newState).toEqual({
         isLoading: false,
         errorMessage: 'Error Happened'
      })
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
            position: '',
            startDate: '2019-08-02',
            endDate: ''
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

   it('check validation for input fields', () => {
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

it('extract developer', () => {
   const initial = {
      isLoading: false,
      summary: 'about a developer',
      experiences: []
   }
   const newState = reducer(initial, { type: 'EXTRACT_DATA' })
   expect(newState).toEqual({
      summary: 'about a developer',
      experiences: []
   })
})

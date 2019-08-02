import reducer from '../../Developer/reducer'
import uuid from 'uuid/v1'
jest.mock('uuid/v1')

describe('loading', () => {
   const initial = {
      isLoading: true
   }

   it('create default values when no developer exists', () => {
      const newState = reducer(initial, {
         type: 'LOAD',
         developer: null
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
         developer
      })

      expect(newState.isLoading).toBe(false)
      expect(newState.summary).toBe(developer.summary)
      expect(newState.experiences).toEqual(developer.experiences)
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
      const initial = {
         experiences: [
            {
               id: 1,
               company: 'Bellin',
               position: 'C# Developer',
               startDate: '2019-01-01',
               endDate: '2020-01-01'
            }
         ]
      }
      const newExperience = {
         id: 1,
         company: 'Lodgify',
         position: 'Back-end developer',
         startDate: '2020-01-01',
         endDate: '2022-01-01'
      }

      const newState = reducer(initial, {
         type: 'UPDATE_EXPERIENCE',
         experience: newExperience
      })

      expect(newState.experiences).toEqual([newExperience])
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

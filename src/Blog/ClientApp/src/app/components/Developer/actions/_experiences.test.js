import { addExperience, deleteExperience, updateExperience } from './_experiences'
import { initialState } from './initials'
import uuid from 'uuid/v1'

jest.mock('uuid/v1')

afterEach(jest.restoreAllMocks)

it('adds a new valid experience', () => {
   uuid.mockReturnValue(123)
   jest.spyOn(global, 'Date').mockImplementation(() => ({
      getFullYear: () => 2012,
      getMonth: () => 4,
      getDate: () => 20
   }))
   const init = {
      experiences: []
   }
   const newState = addExperience(init)

   expect(newState.experiences).toEqual([
      {
         id: 123,
         company: '',
         position: '',
         startDate: '2012-05-20',
         endDate: '',
         content: ''
      }
   ])
})

it('deletes an existing experience', () => {
   const initial = {
      experiences: [{ id: 1 }, { id: 2 }]
   }

   const newState = deleteExperience(initial, { id: 1 })

   expect(newState.experiences).toEqual([{ id: 2 }])
})

it('updates an experience', () => {
   uuid.mockReturnValue(1)
   const init = {
      experiences: [
         {
            id: 1,
            company: 'Lodgify',
            position: 'C# Developer',
            startDate: '2010-01-01',
            endDate: '2011-01-01',
            content: 'DESC'
         }
      ]
   }

   let state = updateExperience(init, { change: { id: 1, company: 'Parmis' } })
   state = updateExperience(state, { change: { id: 1, position: 'JS' } })
   state = updateExperience(state, { change: { id: 1, startDate: '2013-01-02' } })
   state = updateExperience(state, { change: { id: 1, endDate: '2015-01-01' } })
   state = updateExperience(state, { change: { id: 1, content: 'Description' } })

   expect(state.experiences).toEqual([
      {
         id: 1,
         company: 'Parmis',
         position: 'JS',
         startDate: '2013-01-02',
         endDate: '2015-01-01',
         content: 'Description'
      }
   ])
})

it('keeps the state intact', () => {
   uuid.mockReturnValueOnce(1)
   uuid.mockReturnValueOnce(2)
   jest.spyOn(global, 'Date').mockImplementation(() => ({
      getFullYear: () => 2019,
      getMonth: () => 6,
      getDate: () => 2
   }))

   let state = addExperience(initialState)
   state = addExperience(state)
   state = updateExperience(state, { change: { id: 2, company: 'Lodgify' } })
   state = updateExperience(state, { change: { id: 2, position: 'C# Developer' } })
   state = deleteExperience(state, { id: 2 })

   expect(state).toMatchSnapshot()
})

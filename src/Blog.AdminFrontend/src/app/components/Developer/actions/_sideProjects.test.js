import uuid from 'uuid/v1'
import { initialState } from './initials'
import { addSideProject, deleteSideProject, updateSideProject } from './_sideProjects'

jest.mock('uuid/v1')

afterEach(jest.restoreAllMocks)

it('adds a new side project', () => {
   uuid.mockReturnValue(123)
   const initial = {
      sideProjects: []
   }
   const state = addSideProject(initial)

   expect(state.sideProjects).toEqual([{ id: 123, title: '', content: '' }])
})

it('deletes a side project', () => {
   const initial = {
      sideProjects: [
         { id: 1, title: 'Richtext', content: 'Richtext Desc' },
         { id: 2, title: 'CodePrac', content: 'CodePrac Desc' }
      ]
   }

   const state = deleteSideProject(initial, { id: 2 })

   expect(state.sideProjects).toEqual([
      {
         id: 1,
         title: 'Richtext',
         content: 'Richtext Desc'
      }
   ])
})

it('updates a side project', () => {
   const init = {
      sideProjects: [
         { id: 1, title: 'Richtext', content: 'Richtext Desc' },
         { id: 2, title: 'CodePrac', content: 'CodePrac Desc' }
      ]
   }

   let state = updateSideProject(init, { change: { id: 2, title: 'Daily Code Practice' } })
   state = updateSideProject(state, { change: { id: 2, content: 'Description' } })

   expect(state.sideProjects).toEqual([
      { id: 1, title: 'Richtext', content: 'Richtext Desc' },
      { id: 2, title: 'Daily Code Practice', content: 'Description' }
   ])
})

it('keeps the state intact', () => {
   uuid.mockReturnValueOnce(1)
   uuid.mockReturnValueOnce(2)

   let state = addSideProject(initialState)
   state = addSideProject(state)
   state = updateSideProject(state, { change: { id: 1, title: 'Richtext' } })
   state = updateSideProject(state, { change: { id: 1, content: 'Desc' } })
   state = deleteSideProject(state, { id: 2 })

   expect(state).toMatchSnapshot()
})

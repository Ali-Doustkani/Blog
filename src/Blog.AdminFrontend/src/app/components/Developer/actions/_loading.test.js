import { load } from './_loading'
import { initialState } from './initials'

it('create default values when no developer exists', () => {
   const newState = load(initialState, {
      result: {
         status: 'ok',
         data: null
      }
   })

   expect(newState).toMatchSnapshot()
})

it('put form in STOPPED mode when fatal happens', () => {
   const newState = load(initialState, {
      result: {
         status: 'fatal',
         data: 'Error Happened'
      }
   })

   expect(newState).toMatchSnapshot()
})

it('stores experience format', () => {
   const state = load(initialState, {
      result: {
         status: 'ok',
         data: {
            experiences: [{ id: 1, company: 'Lodgify' }]
         }
      }
   })

   expect(state).toMatchSnapshot()
})

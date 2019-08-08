import { map } from './serverErrorMapper'

it('creates a property for an object', () => {
   const obj = { nameErrors: [] }
   map([{ error: 'isRequired', path: ['name'] }], obj)

   expect(obj).toEqual({ nameErrors: [{ type: 2, message: 'name is required' }] })
})

it('does not create the property if there is already one', () => {
   const obj = { nameErrors: [{ type: 1, message: 'error' }] }
   map([{ error: 'isRequired', path: ['name'] }], obj)

   expect(obj).toEqual({
      nameErrors: [{ type: 1, message: 'error' }, { type: 2, message: 'name is required' }]
   })
})

it('sets nested properties', () => {
   const obj = { info: { personal: { nameErrors: [] } } }
   map([{ error: 'isRequired', path: ['info', 'personal', 'name'] }], obj)

   expect(obj).toEqual({
      info: {
         personal: {
            nameErrors: [{ type: 2, message: 'name is required' }]
         }
      }
   })
})

it('sets properties for array items', () => {
   const obj = { users: [{}, { nameErrors: [] }] }
   map([{ error: 'isRequired', path: ['users', 1, 'name'] }], obj)

   expect(obj).toEqual({
      users: [{}, { nameErrors: [{ type: 2, message: 'name is required' }] }]
   })
})

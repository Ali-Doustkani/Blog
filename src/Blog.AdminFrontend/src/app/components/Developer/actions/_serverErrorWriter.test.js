import { writeErrors } from './_serverErrorWriter'

it('writes the error in the array', () => {
   const state = { nameErrors: [] }
   writeErrors({ name: ['the name field is required'] }, state)

   expect(state.nameErrors).toEqual([{ type: 2, message: 'the name field is required' }])
})

it('writes multiple errors in the array', () => {
   const state = { nameErrors: [] }
   writeErrors({ name: ['error1', 'error2'] }, state)

   expect(state.nameErrors).toEqual([
      { type: 2, message: 'error1' },
      { type: 2, message: 'error2' }
   ])
})

it('throws error if the error property does not exist', () => {
   const state = {}
   expect(() => writeErrors({ name: ['error'] }, state)).toThrow(
      'error array does not exist in the state'
   )
})

it('sets nested properties', () => {
   const obj = { info: { personal: { nameErrors: [] } } }
   writeErrors({ 'info.personal.name': ['the name field is required'] }, obj)

   expect(obj).toEqual({
      info: {
         personal: {
            nameErrors: [{ type: 2, message: 'the name field is required' }]
         }
      }
   })
})

it('sets properties for array items', () => {
   const obj = { users: [{}, { nameErrors: [] }] }
   writeErrors({ 'users[1].name': ['name field is required'] }, obj)

   expect(obj).toEqual({
      users: [{}, { nameErrors: [{ type: 2, message: 'name field is required' }] }]
   })
})

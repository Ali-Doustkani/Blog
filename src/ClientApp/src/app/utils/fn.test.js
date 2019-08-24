import { dict, ifElse, addItem, updateItem, deleteItem } from './fn'

describe('dict', () => {
   const a = {},
      b = {},
      c = {},
      func = dict(['a', a], ['b', b], ['c', c])

   it('selects the correct func', () => {
      expect(func('a')).toBe(a)
      expect(func('b')).toBe(b)
      expect(func('c')).toBe(c)
   })

   it('throws when invalid key is passed', () => {
      expect(() => func('d')).toThrow('Invalid key for pair')
   })
})

describe('ifElse', () => {
   it('returns the first argument when true', () => {
      const first = (state, action) => 'first: ' + state.value + action.value
      const second = (state, action) => 'second: ' + state.value + action.value
      const fn = ifElse(() => true, first, second)
      const result = fn({ value: 'a' }, { value: 'b' })
      expect(result).toBe('first: ab')
   })

   it('returns the second argument when false', () => {
      const first = (state, action) => 'first: ' + state.value + action.value
      const second = (state, action) => 'second: ' + state.value + action.value
      const fn = ifElse(() => false, first, second)
      const result = fn({ value: 'a' }, { value: 'b' })
      expect(result).toBe('second: ab')
   })
})

describe('state manipulation', () => {
   it('adds a new item', () => {
      const init = { values: [] }
      const newState = addItem('values', () => ({ id: 1, name: 'Ali' }))(init)
      expect(newState).toEqual({ values: [{ id: 1, name: 'Ali' }] })
   })

   it('deletes an existing value', () => {
      const initial = {
         values: [{ id: 1 }, { id: 2 }]
      }
      const newState = deleteItem('values')(initial, { id: 1 })
      expect(newState).toEqual({ values: [{ id: 2 }] })
   })

   it('updates an item', () => {
      const init = {
         values: [{ id: 1, name: 'Ali', surname: 'Doustkani' }]
      }

      const update = updateItem('values')
      let state = update(init, { change: { id: 1, name: 'Haniye' } })
      state = update(state, { change: { id: 1, surname: 'Doosty' } })

      expect(state).toEqual({ values: [{ id: 1, name: 'Haniye', surname: 'Doosty' }] })
   })

   it('keeps the state intact', () => {
      let id = 1
      const getId = () => {
         id++
         return id
      }

      const initialState = { values: [] }
      const add = addItem('values', () => ({ id: getId(), name: '', surname: '' }))
      const update = updateItem('values')
      const deleteIt = deleteItem('values')

      let state = add(initialState)
      state = add(state)
      state = update(state, { change: { id: 2, name: 'Ali' } })
      state = update(state, { change: { id: 2, surname: 'Doustkani' } })
      state = deleteIt(state, { id: 2 })

      expect(state).toMatchSnapshot()
   })
})

import { dict, evolve, evolveFrom, take, ifElse } from './fn'

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

describe('evolve', () => {
   it('adds value step results', () => {
      const step1 = { age: 30 }

      const fn = evolve(step1)

      const result = fn({ country: 'Iran' })

      expect(result).toEqual({
         age: 30,
         country: 'Iran'
      })
   })

   it('adds function step results', () => {
      const step1 = () => ({ name: 'ali' })
      const step2 = op => ({ position: op.action.position })

      const fn = evolve(step1, step2)

      const result = fn({ country: 'Iran' }, { position: 'React Developer' })

      expect(result).toEqual({
         name: 'ali',
         position: 'React Developer',
         country: 'Iran'
      })
   })
})

describe('evolveFrom', () => {
   it('adds function steps', () => {
      const step1 = take(op => ({ position: op.action.position }))
      const init = { name: 'ali' }
      const fn = evolveFrom(init, step1)
      const result = fn({ state: 'none taken' }, { position: 'React Developer' })
      expect(result).toEqual({
         name: 'ali',
         position: 'React Developer'
      })
   })

   it('adds value steps', () => {
      const step1 = { position: 'React Developer' }
      const init = { name: 'ali' }
      const fn = evolveFrom(init, step1)
      const result = fn({ state: 'none taken' })
      expect(result).toEqual({
         name: 'ali',
         position: 'React Developer'
      })
   })
})

describe('take', () => {
   it('selects the appropriate property', () => {
      const state = { data: 'a' }
      const action = { data: 'b' }
      const fn = take(op => op.state.data + op.action.data)
      const result = fn({ state, action })
      expect(result).toBe('ab')
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

import { dict, ifElse } from './fn'

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

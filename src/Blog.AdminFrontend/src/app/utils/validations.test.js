import { emptyValidator, richtextEmptyValidator } from './validations'

describe('returning of server errors', () => {
   it('returns both validator errors & server errors', () => {
      const currentErrors = [
         { type: 1, message: 'client error' },
         { type: 2, message: 'server error' }
      ]
      const check = emptyValidator('age', currentErrors)

      expect(check('')).toEqual([
         { type: 2, message: 'server error' },
         { type: 1, message: 'age is required' }
      ])
   })

   it('returns only server errors when there is no rule', () => {
      const check = emptyValidator('', [
         { type: 1, message: 'client error' },
         { type: 2, message: 'server error' }
      ])
      expect(check('ali')).toEqual([{ type: 2, message: 'server error' }])
   })
})

describe('not empty rule', () => {
   it('returns nothing for correct notEmpty rule', () => {
      const check = emptyValidator('name')
      expect(check('Ali')).toEqual([])
   })

   describe('failures', () => {
      const check = emptyValidator('name')
      let result

      test('null', () => {
         result = check(null)
      })

      test('undefined', () => {
         result = check(undefined)
      })

      test('empty string', () => {
         result = check('')
      })

      test('whitespace string', () => {
         result = check('  ')
      })

      afterEach(() => {
         expect(result).toEqual([{ type: 1, message: 'name is required' }])
      })
   })
})

describe('not richtext empty rule', () => {
   const check = richtextEmptyValidator('content')

   describe('successes', () => {
      let result

      test('other tags', () => {
         result = check('<a>TEXT</a>')
      })

      test('non-tag text', () => {
         result = check('ali')
      })

      afterEach(() => {
         expect(result).toEqual([])
      })
   })

   describe('failures', () => {
      let result

      test('empty tag without attribute', () => {
         result = check('<p> </p>')
      })

      test('tag with multiple attributes', () => {
         result = check('<p contenteditable="" class="sth"> </p>')
      })

      test('non-tag text', () => {
         result = check('')
      })

      afterEach(() => {
         expect(result).toEqual([{ type: 1, message: 'content is required' }])
      })
   })
})

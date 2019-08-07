import { emitChange, renderClassNames } from './utils'

test('emitChange calls the onChange func', () => {
   expect.assertions(1)
   const props = {
      id: 1,
      name: 'position',
      onChange: e => expect(e).toEqual({ id: 1, position: 'C++ Developer' })
   }
   emitChange(props)({ target: { value: 'C++ Developer' } })
})

test('renderClassNames returns original class', () => {
   expect(renderClassNames({ className: 'text-box' })).toBe('text-box')
})

test('renderClassNames returns incorrect class if any error', () => {
   expect(renderClassNames({ name: 'name', nameErrors: ['err'] })).toBe('incorrect')
})

test('renderClassNames returns class and incorrect', () => {
   expect(renderClassNames({ name: 'name', className: 'text-box', nameErrors: ['err'] })).toBe(
      'text-box incorrect'
   )
})

import { isEmpty, isRichtextEmtpy } from '../../app/utils/validations'

describe('isEmpty', () => {
   test('null', () => {
      expect(isEmpty(null)).toBe(true)
   })
   test('undefined', () => {
      expect(isEmpty(undefined)).toBe(true)
   })
   test('empty string', () => {
      expect(isEmpty('')).toBe(true)
   })
   test('whitespace string', () => {
      expect(isEmpty('  ')).toBe(true)
   })
   test('non-whitespace string', () => {
      expect(isEmpty('ALI')).toBe(false)
   })
})

describe('isRichtextEmpty', () => {
   test('empty tag without attribute', () => {
      expect(isRichtextEmtpy('<p> </p>')).toBe(true)
   })
   test('tag with multiple attributes', () => {
      expect(isRichtextEmtpy('<p contenteditable="" class="sth"> </p>')).toBe(true)
   })
   test('other tags', () => {
      expect(isRichtextEmtpy('<a>TEXT</a>')).toBe(false)
   })
   test('non-tag text', () => {
      expect(isRichtextEmtpy('ali')).toBe(false)
      expect(isRichtextEmtpy('')).toBe(true)
   })
})

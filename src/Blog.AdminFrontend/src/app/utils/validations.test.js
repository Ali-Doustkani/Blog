import { isEmpty, isRichtextEmpty, getDuplicate, hasOverlap, getOverlap } from './validations'

describe('isEmpty', () => {
   it('returns nothing for correct notEmpty rule', () => {
      expect(isEmpty('Ali')).toBe(false)
   })

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
})

describe('isRichtextEmpty', () => {
   test('other tags', () => {
      expect(isRichtextEmpty('<a>TEXT</a>')).toBe(false)
   })

   test('non-tag text', () => {
      expect(isRichtextEmpty('ali')).toBe(false)
      expect(isRichtextEmpty('')).toBe(true)
   })

   test('empty tag without attribute', () => {
      expect(isRichtextEmpty('<p> </p>')).toBe(true)
   })

   test('tag with multiple attributes', () => {
      expect(isRichtextEmpty('<p contenteditable="" class="sth"> </p>')).toBe(true)
   })
})

describe('getDuplicate', () => {
   it('checks for duplication', () => {
      const dup = getDuplicate({
         values: [{ id: 1, name: 'Ali' }, { id: 2, name: 'Hasan' }, { id: 3, name: 'Ali' }],
         prop: 'name'
      })
      expect(dup).toEqual({ id: 3, name: 'Ali' })
   })

   it('returns undefined when there is no duplicate', () => {
      const dup = getDuplicate({
         values: [{ id: 1, name: 'Ali' }, { id: 2, name: 'Hasan' }],
         prop: 'name'
      })
      expect(dup).toBeUndefined()
   })

   it('does not compare objects with empty fields', () => {
      const dup = getDuplicate({
         values: [{ id: 1, name: 'Ali' }, { id: 2, name: '' }, { id: 3, name: '' }],
         prop: 'name'
      })
      expect(dup).toBeUndefined()
   })

   it('compares multiple props', () => {
      const dup = getDuplicate({
         values: [
            { id: 1, name: 'Ali', surname: 'Doustkani' },
            { id: 2, name: 'Ali', surname: 'Moradi' },
            { id: 3, name: 'Ali', surname: 'Doustkani' }
         ],
         prop: ['name', 'surname']
      })
      expect(dup).toEqual({ id: 3, name: 'Ali', surname: 'Doustkani' })
   })
})

describe('overlaps', () => {
   test.each([
      [
         'A',
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         { startDate: '2010-6-1', endDate: '2011-6-1' },
         true
      ],
      [
         'B',
         { startDate: '2010-6-1', endDate: '2011-6-1' },
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         true
      ],
      [
         'C',
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         true
      ],
      [
         'D',
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         { startDate: '2012-1-1', endDate: '2013-1-1' },
         false
      ],
      [
         'E',
         { startDate: '2012-1-1', endDate: '2013-1-1' },
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         false
      ],
      [
         'F',
         { startDate: '2011-1-1', endDate: '2012-1-1' },
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         false
      ],
      [
         'G',
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         { startDate: '2011-1-1', endDate: '2012-1-1' },
         false
      ],
      [
         'H',
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         { startDate: '2010-6-1', endDate: '2011-1-1' },
         true
      ],
      [
         'I',
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         { startDate: '2010-1-1', endDate: '2010-6-1' },
         true
      ],
      [
         'J',
         { startDate: '2010-6-1', endDate: '2011-1-1' },
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         true
      ],
      [
         'K',
         { startDate: '2010-1-1', endDate: '2010-6-1' },
         { startDate: '2010-1-1', endDate: '2011-1-1' },
         true
      ]
   ])('Case %s', (tag, item1, item2, result) => {
      expect(hasOverlap(item1, item2)).toBe(result)
   })

   it('gets overlaps in an array', () => {
      const [overlapper, overlapped] = getOverlap([
         { id: 1, startDate: '2010-1-1', endDate: '2011-1-1' },
         { id: 2, startDate: '2011-1-1', endDate: '2012-1-1' },
         { id: 3, startDate: '2010-8-1', endDate: '2011-1-1' }
      ])
      expect(overlapper.id).toBe(3)
      expect(overlapped.id).toBe(1)
   })

   it('gets undefined when there is no overlap', () => {
      const [overlapper, overlapped] = getOverlap([
         { id: 1, startDate: '2010-1-1', endDate: '2011-1-1' },
         { id: 2, startDate: '2011-1-1', endDate: '2012-1-1' },
         { id: 3, startDate: '2012-1-1', endDate: '2013-1-1' }
      ])
      expect(overlapper).toBeUndefined()
      expect(overlapped).toBeUndefined()
   })
})

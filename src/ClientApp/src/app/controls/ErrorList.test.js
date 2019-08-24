import React from 'react'
import { shallow } from 'enzyme'
import { ErrorList } from './ErrorList'

it('shows an error', () => {
   const errorlist = shallow(<ErrorList errors="error" />)
   expect(errorlist.children().length).toBe(1)
   expect(errorlist.childAt(0).text()).toBe('error')
})

it('shows array of errors', () => {
   const errorlist = shallow(<ErrorList errors={['err1', 'err2']} />).find('ul')
   expect(errorlist.children().length).toBe(2)
   expect(errorlist.childAt(0).text()).toBe('err1')
   expect(errorlist.childAt(1).text()).toBe('err2')
})

it('shows array of error arrays', () => {
   const errorlist = shallow(<ErrorList errors={[['err1'], ['err2', 'err3']]} />).find('ul')
   expect(errorlist.children().length).toBe(3)
   expect(errorlist.childAt(0).text()).toBe('err1')
   expect(errorlist.childAt(1).text()).toBe('err2')
   expect(errorlist.childAt(2).text()).toBe('err3')
})

it('ignores undefined values in arrays', () => {
   const errorlist = shallow(<ErrorList errors={[['err1'], undefined, ['err2', 'err3']]} />).find(
      'ul'
   )
   expect(errorlist.children().length).toBe(3)
   expect(errorlist.childAt(0).text()).toBe('err1')
   expect(errorlist.childAt(1).text()).toBe('err2')
   expect(errorlist.childAt(2).text()).toBe('err3')
})

it('shows nothing if there is no array', () => {
   expect(shallow(<ErrorList />).isEmptyRender()).toBe(true)
})

it('shows nothing for empty strings', () => {
   expect(shallow(<ErrorList errors="" />).isEmptyRender()).toBe(true)
})

it('shows nothing if errors array is empty', () => {
   expect(shallow(<ErrorList errors={[]} />).isEmptyRender()).toBe(true)
})

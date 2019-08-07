import React from 'react'
import { shallow } from 'enzyme'
import { ErrorList } from '../../app/Components/ErrorList'

it('shows errors', () => {
   const errorlist = shallow(<ErrorList errors={['err1', 'err2']} />).find('ul')
   expect(errorlist.children().length).toBe(2)
   expect(errorlist.childAt(0).text()).toBe('err1')
   expect(errorlist.childAt(1).text()).toBe('err2')
})

it('shows nothing if there is no array', () => {
   expect(shallow(<ErrorList />).isEmptyRender()).toBe(true)
})

it('shows nothing if errors array is empty', () => {
   expect(shallow(<ErrorList errors={[]} />).isEmptyRender()).toBe(true)
})

it('uses naming conventions if no errors prop is available', () => {
   const list = shallow(
      <ErrorList
         companyErrors={['1']}
         positionErrors={[]}
         contentErrors={undefined}
         surnameErrors={['2', '3']}
      />
   ).find('ul')
   expect(list.children().length).toBe(3)
   expect(list.childAt(0).text()).toBe('1')
   expect(list.childAt(1).text()).toBe('2')
   expect(list.childAt(2).text()).toBe('3')
})

import React from 'react'
import { render, shallow, mount } from 'enzyme'
import { Richtext } from './Richtext'
import { ErrorList } from './ErrorList'

it('sets hasFocus class when focused', () => {
   const richtext = shallow(<Richtext name="content" />)

   expect(
      richtext
         .find('article')
         .parent()
         .hasClass('hasFocus')
   ).toBe(false)

   richtext
      .find('article')
      .parent()
      .simulate('focus')

   expect(
      richtext
         .find('article')
         .parent()
         .hasClass('hasFocus')
   ).toBe(true)

   richtext
      .find('article')
      .parent()
      .simulate('blur')

   expect(
      richtext
         .find('article')
         .parent()
         .hasClass('hasFocus')
   ).toBe(false)
})

it('sets innerHtml with naming convention', () => {
   expect(
      render(<Richtext name="content" content="<p>TEXT</p>" />)
         .find('article')
         .html()
   ).toBe('<p>TEXT</p>')
})

it('fires the change event with id and new value', () => {
   mount(
      <Richtext
         id={1}
         name="content"
         content="<p>TEXT</p>"
         onChange={e => {
            expect(e.id).toBe(1)
            expect(e.content).toBe('<p>TEXT</p>')
         }}
      />
   )
      .find('article')
      .simulate('blur')
})

it('does not show errors when showErrors is false', () => {
   expect(
      shallow(<Richtext name="sth" showErrors={false} errors="error1" />).find(ErrorList).length
   ).toBe(0)
   expect(
      shallow(<Richtext name="sth" showErrors={true} errors="error1" />).find(ErrorList).length
   ).toBe(1)
})

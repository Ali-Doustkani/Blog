import React from 'react'
import { shallow } from 'enzyme'
import Message from './Message'

it('displays the message', () => {
   expect(
      shallow(<Message message="The Message" appearance="error" />)
         .childAt(1)
         .text()
   ).toBe('The Message')
})

it('sets the success classes', () => {
   expect(
      shallow(<Message message="MSG" appearance="success" />)
         .find('i')
         .hasClass('fa-check')
   ).toBe(true)
})

it('sets the error classes', () => {
   expect(
      shallow(<Message message="MSG" appearance="error" />)
         .find('i')
         .hasClass('fa-times')
   ).toBe(true)
})

it('calls dismiss when clicked', () => {
   expect.assertions(1)
   shallow(
      <Message
         message="MSG"
         appearance="error"
         onDismiss={() => {
            expect(true).toBe(true)
         }}
      />
   )
      .find('a')
      .simulate('click')
})

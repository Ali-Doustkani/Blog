import React from 'react'
import { shallow } from 'enzyme'
import { Input } from './Input'

it('renders name', () => {
   expect(
      shallow(<Input name="the-input" />)
         .find('input')
         .prop('name')
   ).toBe('the-input')
})

it('renders text without type', () => {
   expect(
      shallow(<Input name="inp" />)
         .find('input')
         .prop('type')
   ).toBe('text')
})

it('renders type', () => {
   expect(
      shallow(<Input name="inp" type="date" />)
         .find('input')
         .prop('type')
   ).toBe('date')
})

it('renders placeholder from name if its not available', () => {
   expect(
      shallow(<Input name="surname" />)
         .find('input')
         .prop('placeholder')
   ).toBe('Surname')
})

it('renders placeholder', () => {
   expect(
      shallow(<Input name="surname" placeholder="Name" />)
         .find('input')
         .prop('placeholder')
   ).toBe('Name')
})

it('renders value with naming conventions', () => {
   expect(
      shallow(<Input name="company" company="Lodgify" />)
         .find('input')
         .prop('defaultValue')
   ).toBe('Lodgify')
})

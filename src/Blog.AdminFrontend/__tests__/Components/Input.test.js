import React from 'react'
import { Input } from '../../app/Components'
import { render, shallow } from 'enzyme'

it('renders name', () => {
   expect(render(<Input name="the-input" />).attr('name')).toBe('the-input')
})

it('renders text without type', () => {
   expect(render(<Input name="inp" />).attr('type')).toBe('text')
})

it('renders type', () => {
   expect(render(<Input name="inp" type="date" />).attr('type')).toBe('date')
})

it('renders placeholder from name if its not available', () => {
   expect(render(<Input name="surname" />).attr('placeholder')).toBe('Surname')
})

it('renders placeholder', () => {
   expect(render(<Input name="surname" placeholder="Name" />).attr('placeholder')).toBe('Name')
})

it('renders classname', () => {
   expect(render(<Input name="surname" className="text-box" />).attr('class')).toBe('text-box')
})

it('renders incorrect class if any error', () => {
   expect(render(<Input name="name" nameErrors={['err']} />).attr('class')).toBe('incorrect')
})

it('renders classname and incorrect', () => {
   expect(
      render(<Input name="name" className="text-box" nameErrors={['er']} />).attr('class')
   ).toBe('text-box incorrect')
})

it('renders incorrect classname with naming convention', () => {
   expect(render(<Input name="company" companyErrors={['err']} />).attr('class')).toBe('incorrect')
})

it('renders value with naming conventions', () => {
   expect(render(<Input name="company" company="Lodgify" />).attr('value')).toBe('Lodgify')
})

it('fires the change event with id and new value', () => {
   expect.assertions(2)
   shallow(
      <Input
         id="1"
         name="surname"
         onChange={e => {
            expect(e.id).toBe('1')
            expect(e.surname).toBe('ali')
         }}
      />
   ).simulate('change', { target: { value: 'ali' } })
})

import React from 'react'
import { shallow } from 'enzyme'
import { Textarea } from './Textarea'

it('displays a label', () => {
   const text = shallow(<Textarea name="company" label="COMPANY" />)
   expect(text.find('label').text()).toBe('COMPANY')
})

it('displays the value prop', () => {
   const text = shallow(<Textarea name="company" company="Lodgify" />)
   expect(text.find('textarea').prop('defaultValue')).toBe('Lodgify')
})

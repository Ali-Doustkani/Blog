import React from 'react'
import { shallow } from 'enzyme'
import Experience from './Experience'

const sample = {
   company: 'Bellin',
   position: 'C# Developer',
   startDate: '2018-03-29',
   endDate: '2019-04-29',
   content: '<p>Worked as a developer</p>'
}

it('displays company', () => {
   const position = shallow(<Experience {...sample} />)
      .find('Input')
      .findWhere(x => x.prop('name') === 'company')
   expect(position.prop('company')).toBe('Bellin')
})

it('displays position', () => {
   const position = shallow(<Experience {...sample} />)
      .find('Input')
      .findWhere(x => x.prop('name') === 'position')
   expect(position.prop('position')).toBe('C# Developer')
})

it('displays startDate', () => {
   const position = shallow(<Experience {...sample} />)
      .find('Input')
      .findWhere(x => x.prop('name') === 'startDate')
   expect(position.prop('startDate')).toBe('2018-03-29')
})

it('displays endDate', () => {
   const position = shallow(<Experience {...sample} />)
      .find('Input')
      .findWhere(x => x.prop('name') === 'endDate')
   expect(position.prop('endDate')).toBe('2019-04-29')
})

it('displays content', () => {
   const position = shallow(<Experience {...sample} />)
      .find('Richtext')
      .findWhere(x => x.prop('name') === 'content')
   expect(position.prop('content')).toBe('<p>Worked as a developer</p>')
})

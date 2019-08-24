import React from 'react'
import { shallow } from 'enzyme'
import SideProject from './SideProject'
import { Input, Richtext, ToolbarButton } from 'Controls'

const sample = {
   id: 12,
   title: 'Richtext',
   content: '<p>Desc</p>',
   onDelete: jest.fn()
}

it('displays title', () => {
   const title = shallow(<SideProject {...sample} />)
      .find(Input)
      .findWhere(x => x.prop('name') === 'title')
   expect(title.prop('title')).toBe('Richtext')
})

it('displays content', () => {
   const content = shallow(<SideProject {...sample} />)
      .find(Richtext)
      .findWhere(x => x.prop('name') === 'content')
   expect(content.prop('content')).toBe('<p>Desc</p>')
})

it('calls delete when delete button clicked', () => {
   expect.assertions(1)
   const btn = shallow(
      <SideProject
         {...sample}
         onDelete={id => {
            expect(id).toBe(12)
         }}
      />
   ).find(ToolbarButton)
   btn.simulate('click')
})

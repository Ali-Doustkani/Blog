import React from 'react'
import { shallow } from 'enzyme'
import PostItem from './PostItem'

it('displays NotPublish tag for not published items', () => {
   expect(
      shallow(<PostItem id={1} title="PostA" published={false} />).find('span.tag').length
   ).toBe(1)
   expect(shallow(<PostItem id={2} title="PostB" published={true} />).find('span.tag').length).toBe(
      0
   )
})

it('displays buttons on mouse over', () => {
   const wrapper = shallow(<PostItem id={1} title="PostA" />)
   expect(wrapper.find('.button-container').hasClass('hidden')).toBe(true)
   wrapper.find('li').simulate('mouseover')
   expect(wrapper.find('.button-container').hasClass('visible')).toBe(true)
})

it('hides buttons when mouse out', () => {
   const wrapper = shallow(<PostItem id={1} title="PostA" />)
   wrapper
      .find('li')
      .simulate('mouseover')
      .simulate('mouseout')
   expect(wrapper.find('.button-container').hasClass('hidden')).toBe(true)
})

it('doesnt display buttons related to published items', () => {
   const wrapper = shallow(<PostItem id={1} title="PostA" published={true} />)
   wrapper.find('li').simulate('mouseover')
   expect(wrapper.findWhere(x => x.prop('data-testid') === 'publish-button').length).toBe(0)
   expect(wrapper.findWhere(x => x.prop('data-testid') === 'preview-button').length).toBe(0)
   expect(wrapper.findWhere(x => x.prop('data-testid') === 'view-button').length).toBe(1)
})

it('displays buttons related to published items', () => {
   const wrapper = shallow(<PostItem id={1} title="PostA" published={false} />)
   wrapper.find('li').simulate('mouseover')
   expect(wrapper.findWhere(x => x.prop('data-testid') === 'publish-button').length).toBe(1)
   expect(wrapper.findWhere(x => x.prop('data-testid') === 'preview-button').length).toBe(1)
})

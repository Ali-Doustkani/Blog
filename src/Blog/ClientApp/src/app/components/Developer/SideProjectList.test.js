import React from 'react'
import { shallow } from 'enzyme'
import { Button } from 'Controls'
import SideProject from './SideProject'
import SideProjectList from './SideProjectList'

it('displays a <SideProject/> for each object', () => {
   const source = [{ id: 1 }, { id: 2 }]
   const list = shallow(
      <SideProjectList sideProjects={source} errors={[]} onAdd={jest.fn()} />
   ).find(SideProject)
   expect(list.length).toBe(2)
   expect(list.findWhere(x => x.prop('id') === 1).length).toBe(1)
   expect(list.findWhere(x => x.prop('id') === 2).length).toBe(1)
})

it('calls for delete when a <SideProject/> needs to be deleted', () => {
   expect.assertions(1)
   shallow(
      <SideProjectList
         sideProjects={[{ id: 1 }]}
         errors={[]}
         onAdd={jest.fn()}
         onDelete={id => {
            expect(id).toBe(1)
         }}
      />
   )
      .find(SideProject)
      .prop('onDelete')(1)
})

it('calls for change when a <SideProject/> is changed', () => {
   expect.assertions(1)
   shallow(
      <SideProjectList
         sideProjects={[{ id: 1 }]}
         errors={[]}
         onAdd={jest.fn()}
         onChange={id => {
            expect(id).toBe(12)
         }}
      />
   )
      .find(SideProject)
      .prop('onChange')(12)
})

it('calls for add', () => {
   const fn = jest.fn()
   shallow(<SideProjectList sideProjects={[]} errors={[]} onAdd={fn} />)
      .find(Button)
      .simulate('click')
   expect(fn).toHaveBeenCalled()
})

it('passes each side project errors to its <SideProject/>', () => {
   const list = shallow(
      <SideProjectList
         onAdd={jest.fn()}
         sideProjects={[{ id: 1 }, { id: 2 }]}
         errors={[{ errorId: 11 }, { errorId: 22 }]}
      />
   )
   expect(list.findWhere(x => x.prop('id') === 1).prop('errorId')).toBe(11)
   expect(list.findWhere(x => x.prop('id') === 2).prop('errorId')).toBe(22)
})

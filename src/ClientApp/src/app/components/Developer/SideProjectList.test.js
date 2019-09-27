import React from 'react'
import { shallow } from 'enzyme'
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

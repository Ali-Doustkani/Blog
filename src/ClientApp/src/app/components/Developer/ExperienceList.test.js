import React from 'react'
import { shallow } from 'enzyme'
import ExperienceList from './ExperienceList'
import Experience from './Experience'

it('displays an experience component for each experience object', () => {
   const list = shallow(
      <ExperienceList experiences={[{ id: 11 }, { id: 22 }]} errors={[]} onAdd={jest.fn()} />
   ).find(Experience)
   expect(list.length).toBe(2)
   expect(list.findWhere(x => x.prop('id') === 11).length).toBe(1)
   expect(list.findWhere(x => x.prop('id') === 22).length).toBe(1)
})

it('passes each experience errors to its <Experience/>', () => {
   const list = shallow(
      <ExperienceList
         onAdd={jest.fn()}
         experiences={[{ id: 1 }, { id: 2 }]}
         errors={[{ errorId: 11 }, { errorId: 22 }]}
      />
   ).find(Experience)
   expect(list.length).toBe(2)
   expect(list.findWhere(x => x.prop('id') === 1).prop('errorId')).toBe(11)
   expect(list.findWhere(x => x.prop('id') === 2).prop('errorId')).toBe(22)
})

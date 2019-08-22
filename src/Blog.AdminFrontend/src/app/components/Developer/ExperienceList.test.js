import React from 'react'
import { shallow } from 'enzyme'
import { Button } from 'Controls'
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

it('calls for delete when an experience needs to be deleted', () => {
   expect.assertions(1)
   const list = shallow(
      <ExperienceList
         experiences={[{ id: 1 }]}
         errors={[]}
         onAdd={jest.fn()}
         onDelete={id => expect(id).toBe(1)}
      />
   )
   list.find(Experience).prop('onDelete')(1)
})

it('calls for change when an experience is changed', () => {
   expect.assertions(1)
   const list = shallow(
      <ExperienceList
         experiences={[{ id: 1 }]}
         errors={[]}
         onAdd={jest.fn()}
         onChange={e => expect(e).toEqual({ id: 1 })}
      />
   )
   list.find(Experience).prop('onChange')({ id: 1 })
})

it('calls for add when an experience needs to be added', () => {
   const fn = jest.fn()
   shallow(<ExperienceList experiences={[]} onAdd={fn} />)
      .find(Button)
      .simulate('click')

   expect(fn).toHaveBeenCalled()
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

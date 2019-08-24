import React from 'react'
import { shallow } from 'enzyme'
import { Button } from 'Controls'
import Education from './Education'
import EducationList from './EducationList'

it('displays an <Education> for each education', () => {
   const educations = shallow(
      <EducationList educations={[{ id: 1 }, { id: 2 }]} errors={[]} onAdd={jest.fn()} />
   ).find(Education)
   expect(educations.length).toBe(2)
   expect(educations.findWhere(x => x.prop('id') === 1).length).toBe(1)
   expect(educations.findWhere(x => x.prop('id') === 2).length).toBe(1)
})

it('forwards onDelete', () => {
   expect.assertions(1)
   shallow(
      <EducationList
         educations={[{ id: 1 }]}
         errors={[]}
         onAdd={jest.fn()}
         onDelete={id => expect(id).toBe(1)}
      />
   )
      .find(Education)
      .prop('onDelete')(1)
})

it('forwards onAdd', () => {
   const func = jest.fn()
   shallow(<EducationList educations={[{ id: 1 }]} errors={[]} onAdd={func} />)
      .find(Button)
      .simulate('click')
   expect(func).toHaveBeenCalled()
})

it('forwards onChange', () => {
   const func = jest.fn()
   shallow(<EducationList educations={[{ id: 1 }]} errors={[]} onAdd={jest.fn()} onChange={func} />)
      .find(Education)
      .prop('onChange')()
   expect(func).toHaveBeenCalled()
})

import React from 'react'
import { shallow } from 'enzyme'
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

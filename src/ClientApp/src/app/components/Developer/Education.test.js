import React from 'react'
import { shallow } from 'enzyme'
import { Input, ToolbarButton, ErrorList } from 'Controls'
import Education from './Education'

const sample = {
   id: 1,
   degree: 'B.S.',
   university: 'S&C',
   startDate: '2010-1-1',
   endDate: '2011-1-1'
}

it('displays degree', () => {
   expect(
      shallow(<Education {...sample} />)
         .find(Input)
         .findWhere(x => x.prop('name') === 'degree')
         .prop('degree')
   ).toBe('B.S.')
})

it('displays university', () => {
   expect(
      shallow(<Education {...sample} />)
         .find(Input)
         .findWhere(x => x.prop('name') === 'university')
         .prop('university')
   ).toBe('S&C')
})

it('displays start date', () => {
   expect(
      shallow(<Education {...sample} />)
         .find(Input)
         .findWhere(x => x.prop('name') === 'startDate')
         .prop('startDate')
   ).toBe('2010-1-1')
})

it('displays end date', () => {
   expect(
      shallow(<Education {...sample} />)
         .find(Input)
         .findWhere(x => x.prop('name') === 'endDate')
         .prop('endDate')
   ).toBe('2011-1-1')
})

it('calls onDelete when delete button clicked', () => {
   expect.assertions(1)
   shallow(<Education onDelete={id => expect(id).toBe(1)} {...sample} />)
      .find(ToolbarButton)
      .simulate('click')
})

it('displays errors', () => {
   expect(
      shallow(
         <Education
            degreeErrors={['err1']}
            universityErrors={['err2']}
            startDateErrors={['err3']}
            endDateErrors={['err4']}
            {...sample}
         />
      )
         .find(ErrorList)
         .prop('errors')
   ).toEqual([['err1'], ['err2'], ['err3'], ['err4']])
})

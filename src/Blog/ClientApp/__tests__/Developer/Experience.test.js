import React from 'react'
import '@testing-library/jest-dom/extend-expect'
import { render, cleanup, fireEvent } from '@testing-library/react'
import Experience from '../../Developer/Experience'

afterEach(cleanup)

it('display an experience', () => {
   const sample = {
      company: 'Bellin',
      position: 'C# Developer',
      startDate: '2018-03-29',
      endDate: '2019-04-29',
      content: '<p>Worked as a developer</p>'
   }

   const { getByDisplayValue, getByTestId, getByText } = render(<Experience {...sample} />)
   expect(getByDisplayValue('Bellin')).toBeVisible()
   expect(getByDisplayValue('C# Developer')).toBeVisible()
   expect(getByTestId('startDate-input')).toHaveValue('2018-03-29')
   expect(getByTestId('endDate-input')).toHaveValue('2019-04-29')
   expect(getByText('Worked as a developer'))
})

it('call onChange when data is changed', () => {
   const experience = {}
   const { getByTestId } = render(<Experience onChange={e => Object.assign(experience, e)} />)

   fireEvent.change(getByTestId('company-input'), { target: { value: 'Lodgify' } })
   fireEvent.change(getByTestId('position-input'), { target: { value: 'C# Developer' } })
   fireEvent.change(getByTestId('startDate-input'), { target: { value: '2012-01-10' } })
   fireEvent.change(getByTestId('endDate-input'), { target: { value: '2014-10-20' } })
   getByTestId('content-richtext').innerHTML = '<p>content</p>'
   fireEvent.blur(getByTestId('content-richtext'))

   expect(experience).toEqual({
      company: 'Lodgify',
      position: 'C# Developer',
      startDate: '2012-01-10',
      endDate: '2014-10-20',
      content: '<p>content</p>'
   })
})

it('set incorrect class on company error', () => {
   let { getByTestId } = render(<Experience companyError="error" />)
   expect(getByTestId('company-input')).toHaveClass('incorrect')
   cleanup()
   getByTestId = render(<Experience />).getByTestId
   expect(getByTestId('company-input')).not.toHaveClass('incorrect')
})

it('set incorrect class on position error', () => {
   let { getByTestId } = render(<Experience positionError="error" />)
   expect(getByTestId('position-input')).toHaveClass('incorrect')
   cleanup()
   getByTestId = render(<Experience />).getByTestId
   expect(getByTestId('position-input')).not.toHaveClass('incorrect')
})

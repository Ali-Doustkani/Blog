import React from 'react'
import '@testing-library/jest-dom/extend-expect'
import { render, cleanup, fireEvent } from '@testing-library/react'
import Experience from '../../app/Developer/Experience'

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

it('make company field red when has error', () => {
   let { getByTestId } = render(<Experience companyErrors={['error']} />)
   expect(getByTestId('company-input')).toHaveClass('incorrect')
   cleanup()
   getByTestId = render(<Experience />).getByTestId
   expect(getByTestId('company-input')).not.toHaveClass('incorrect')
})

it('make position field red when has error', () => {
   let { getByTestId } = render(<Experience positionErrors={['error']} />)
   expect(getByTestId('position-input')).toHaveClass('incorrect')
   cleanup()
   getByTestId = render(<Experience />).getByTestId
   expect(getByTestId('position-input')).not.toHaveClass('incorrect')
})

it('make startDate field red when has error', () => {
   let { getByTestId } = render(<Experience startDateErrors={['error1']} />)
   expect(getByTestId('startDate-input')).toHaveClass('incorrect')
   cleanup()
   getByTestId = render(<Experience />).getByTestId
   expect(getByTestId('startDate-input')).not.toHaveClass('incorrect')
})

it('make endDate field red when has error', () => {
   let { getByTestId } = render(<Experience endDateErrors={['error']} />)
   expect(getByTestId('endDate-input')).toHaveClass('incorrect')
   cleanup()
   getByTestId = render(<Experience />).getByTestId
   expect(getByTestId('endDate-input')).not.toHaveClass('incorrect')
})

it('show list of all field errors', () => {
   let { getByTestId } = render(
      <Experience companyErrors={['err1', 'err2']} endDateErrors={['err3']} />
   )
   const list = getByTestId('error-list')
   expect(list.childNodes.length).toBe(3)
   expect(list.childNodes[0].textContent).toBe('err1')
   expect(list.childNodes[1].textContent).toBe('err2')
   expect(list.childNodes[2].textContent).toBe('err3')
})

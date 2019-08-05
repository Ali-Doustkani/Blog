import React from 'react'
import { render, cleanup } from '@testing-library/react'
import '@testing-library/jest-dom/extend-expect'
import { ErrorList } from '../../app/Components/ErrorList'

afterEach(cleanup)

it('shows errors', () => {
   const { getByTestId } = render(<ErrorList errors={['err1', 'err2']} />)
   expect(getByTestId('error-list').childNodes.length).toBe(2)
   expect(getByTestId('error-list').childNodes[0].textContent).toBe('err1')
   expect(getByTestId('error-list').childNodes[1].textContent).toBe('err2')
})

it('shows nothing if there is no array', () => {
   const { queryByTestId } = render(<ErrorList />)
   expect(queryByTestId('error-list')).not.toBeInTheDocument()
})

it('shows nothing if errors array is empty', () => {
   const { queryByTestId } = render(<ErrorList errors={[]} />)
   expect(queryByTestId('error-list')).not.toBeInTheDocument()
})

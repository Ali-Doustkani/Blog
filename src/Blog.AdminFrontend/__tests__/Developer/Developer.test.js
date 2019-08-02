import React from 'react'
import '@testing-library/jest-dom/extend-expect'
import { render, cleanup, fireEvent, wait } from '@testing-library/react'
import Developer from '../../app/Developer/Developer'
import reducer from '../../app/Developer/reducer'

jest.mock('../../app/Developer/reducer')
jest.mock('../../app/Developer/services')

const experience1 = {
   id: 1,
   company: 'Lodgify',
   position: 'C# Developer',
   startDate: '2019-01-01',
   endDate: '2020-01-01'
}

const emptyState = {
   isLoading: false,
   summary: '',
   experiences: []
}

const fullState = {
   isLoading: false,
   summary: '<p>life of a programmer</p>',
   experiences: [experience1]
}

afterEach(cleanup)

describe('loader when loading data', () => {
   it('not finished yet', () => {
      reducer.mockReturnValue({ isLoading: true })
      const { getByTestId } = render(<Developer />)
      expect(getByTestId('loader-component')).toBeVisible()
   })

   it('finished', async () => {
      reducer.mockReturnValue({ isLoading: false, summary: '', experiences: [] })
      const { queryByTestId } = render(<Developer />)
      await wait(() => expect(queryByTestId('loader-component')).not.toBeInTheDocument())
   })
})

describe('experiences', () => {
   it('show on load', async () => {
      reducer.mockReturnValue(fullState)
      const { getByTestId } = render(<Developer />)
      await wait(() => expect(getByTestId('experience-component')).toBeInTheDocument())
   })

   it('add experience', async () => {
      reducer.mockImplementation((state, action) => {
         switch (action.type) {
            case 'LOAD':
               return emptyState
            case 'NEW_EXPERIENCE':
               return { ...state, experiences: [experience1] }
         }
      })

      const { getByTestId, queryByTestId, queryAllByTestId } = render(<Developer />)

      await wait(() => {
         expect(queryByTestId('experience-component')).not.toBeInTheDocument()
         fireEvent.click(getByTestId('addExperience-button'))
         expect(queryAllByTestId('experience-component').length).toBe(1)
      })
   })

   it('delete experience', async () => {
      reducer.mockImplementation((state, action) => {
         switch (action.type) {
            case 'LOAD':
               return fullState
            case 'DELETE_EXPERIENCE':
               return { ...state, experiences: [] }
         }
      })

      const { getByTestId, queryAllByTestId } = render(<Developer />)

      await wait(() => {
         expect(queryAllByTestId('experience-component').length).toBe(1)
         fireEvent.click(getByTestId('deleteExperience-button'))
         expect(queryAllByTestId('experience-component').length).toBe(0)
      })
   })
})

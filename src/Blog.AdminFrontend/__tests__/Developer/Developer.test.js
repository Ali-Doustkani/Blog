import React from 'react'
import '@testing-library/jest-dom/extend-expect'
import { render, cleanup, fireEvent, wait } from '@testing-library/react'
import Developer from '../../Developer/Developer'
import * as mockServer from '../../Developer/services'

const developer = {
   summary: '<p>summary</p>',
   experiences: [
      {
         id: 1,
         company: 'Parmis',
         position: 'developer',
         startDate: '2015-01-01',
         endDate: '2017-01-01',
         content: '<p>parmis is a good company</p>'
      }
   ]
}

jest.mock('../../Developer/services')

afterEach(() => {
   cleanup()
   mockServer.getDeveloper.mockReset()
})

describe('loader when loading data', () => {
   it('not finished yet', () => {
      mockServer.getDeveloper.mockResolvedValue(developer)
      const { getByTestId } = render(<Developer />)
      expect(getByTestId('loader-component')).toBeVisible()
   })

   it('finished', async () => {
      mockServer.getDeveloper.mockResolvedValue(developer)
      const { queryByTestId } = render(<Developer />)
      await wait(() => expect(queryByTestId('loader-component')).not.toBeInTheDocument())
   })

   it('show empty developer if no developer exists', async () => {
      let developer
      mockServer.saveDeveloper.mockImplementation(dev => (developer = dev))
      mockServer.getDeveloper.mockResolvedValue(null)
      const { getByTestId } = render(<Developer />)
      await wait(() => fireEvent.click(getByTestId('save-button')))
      expect(developer).toEqual({
         summary: '',
         experiences: []
      })
   })
})

describe('experiences', () => {
   it('show on load', async () => {
      mockServer.getDeveloper.mockResolvedValue(developer)
      const { getByTestId } = render(<Developer />)
      await wait(() => expect(getByTestId('experience-component')).toBeInTheDocument())
   })

   it('add experience', async () => {
      const { getByTestId, queryByTestId, queryAllByTestId } = render(<Developer />)
      await wait(() => {
         expect(queryByTestId('experience-component')).not.toBeInTheDocument()
         fireEvent.click(getByTestId('addExperience-button'))
         fireEvent.click(getByTestId('addExperience-button'))
         expect(queryAllByTestId('experience-component').length).toBe(2)
      })
   })

   it('delete experience', async () => {
      mockServer.getDeveloper.mockResolvedValue(developer)
      const { getByTestId, queryAllByTestId } = render(<Developer />)
      await wait(() => {
         expect(queryAllByTestId('experience-component').length).toBe(1)
         fireEvent.click(getByTestId('deleteExperience-button'))
         expect(queryAllByTestId('experience-component').length).toBe(0)
      })
   })

   it('update experience', async () => {
      expect.assertions(1)
      mockServer.getDeveloper.mockResolvedValue(developer)
      const { getByTestId } = render(<Developer />)

      let experiences
      mockServer.saveDeveloper.mockImplementation(dev => (experiences = dev.experiences))

      await wait(() => {
         fireEvent.change(getByTestId('company-input'), { target: { value: 'Lodgify' } })
         fireEvent.change(getByTestId('position-input'), { target: { value: 'C# Developer' } })
         fireEvent.change(getByTestId('startDate-input'), { target: { value: '2019-01-01' } })
         fireEvent.change(getByTestId('endDate-input'), { target: { value: '2020-01-01' } })
         getByTestId('content-richtext').innerHTML = '<p>lodgify is a good company</p>'
         fireEvent.blur(getByTestId('content-richtext'))
         fireEvent.click(getByTestId('save-button'))
      })

      expect(experiences).toEqual([
         {
            id: 1,
            company: 'Lodgify',
            position: 'C# Developer',
            startDate: '2019-01-01',
            endDate: '2020-01-01',
            content: '<p>lodgify is a good company</p>'
         }
      ])
   })
})

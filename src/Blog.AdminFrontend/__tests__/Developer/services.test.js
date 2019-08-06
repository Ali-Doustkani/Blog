import fetchMock from '../__mocks__/fetchMock'
import { getDeveloper, saveDeveloper } from '../../app/Developer/services'

const sample = {
   summary: 'started to code as a young boy',
   skills: 'c#, javascript',
   experiences: []
}

beforeEach(fetchMock.reset)

describe('GET', () => {
   it('handles 200', async () => {
      fetchMock.status(200).data({ name: 'developer' })

      expect(await getDeveloper()).toEqual({
         status: 'ok',
         data: { name: 'developer' }
      })
   })

   it('handles 204', async () => {
      fetchMock.status(204)

      expect(await getDeveloper()).toEqual({
         status: 'ok',
         data: null
      })
   })

   it('handles 404', async () => {
      fetchMock.status(404)

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'server not found'
      })
   })

   it('handles other status', async () => {
      fetchMock.status(500).data('error message')

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'error message'
      })
   })

   it('handles corrupted data', async () => {
      fetchMock.status(200).throwOnData('corrupted data')

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'corrupted data'
      })
   })

   it('handles ERR_CONNECTION_REFUSED', async () => {
      fetchMock.throw('Error')

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'Error'
      })
   })
})

describe('PUT', () => {
   it('handles 200', async () => {
      fetchMock.status(200).data({ data: 'response' })

      expect(await saveDeveloper(sample)).toEqual({
         status: 'ok',
         data: { data: 'response' }
      })
      expect(fetchMock.body).toEqual(JSON.stringify(sample))
   })

   it('handles 200 with corrupted content', async () => {
      fetchMock.status(200).throwOnData('corrupted data')

      expect(await saveDeveloper(sample)).toEqual({
         status: 'fatal',
         data: 'corrupted data'
      })
   })

   it('handles 201', async () => {
      fetchMock.status(201).data({ data: 'result' })

      expect(await saveDeveloper(sample)).toEqual({
         status: 'ok',
         data: { data: 'result' }
      })
   })

   it('handles 400', async () => {
      fetchMock.status(400).data({ summary: ['the summary field is required'] })

      expect(await saveDeveloper(sample)).toEqual({
         status: 'error',
         data: { summary: ['the summary field is required'] }
      })
   })

   it('handles 404', async () => {
      fetchMock.status(404)

      expect(await saveDeveloper(sample)).toEqual({
         status: 'fatal',
         data: 'server not found'
      })
   })

   it('handles other statuses', async () => {
      fetchMock.status(500).data('error on server')

      expect(await saveDeveloper(sample)).toEqual({
         status: 'fatal',
         data: 'error on server'
      })
   })

   it('handle ERR_CONNECTION_REFUSED', async () => {
      fetchMock.throw('ERROR')

      expect(await saveDeveloper(sample)).toEqual({
         status: 'fatal',
         data: 'ERROR'
      })
   })

   it('extracts api info from ui', async () => {
      fetchMock.status(200)

      await saveDeveloper({
         summary: 'summary',
         summaryErrors: ['err'],
         skills: 'skills',
         skillsErrors: [],
         experiences: [
            {
               id: 1,
               company: 'company',
               companyErrors: ['err'],
               position: 'position',
               positionErrors: [],
               startDate: '2001-01-01',
               startDateErrors: [],
               endDate: '2002-01-01',
               endDateErrors: [],
               content: 'content',
               contentErrors: []
            }
         ]
      })

      expect(fetchMock.body).toBe(
         JSON.stringify({
            summary: 'summary',
            skills: 'skills',
            experiences: [
               {
                  id: 1,
                  company: 'company',
                  position: 'position',
                  startDate: '2001-01-01',
                  endDate: '2002-01-01',
                  content: 'content'
               }
            ]
         })
      )
   })
})

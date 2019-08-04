import fetchMock from '../__mocks__/fetchMock'
import { getDeveloper, saveDeveloper } from '../../app/Developer/services'

beforeEach(fetchMock.reset)

describe('GET', () => {
   it('handle 200', async () => {
      fetchMock.status(200).data({ name: 'developer' })

      expect(await getDeveloper()).toEqual({
         status: 'ok',
         data: { name: 'developer' }
      })
   })

   it('handle 204', async () => {
      fetchMock.status(204)

      expect(await getDeveloper()).toEqual({
         status: 'ok',
         data: null
      })
   })

   it('handle 404', async () => {
      fetchMock.status(404)

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'server not found'
      })
   })

   it('handle other status', async () => {
      fetchMock.status(500).data('error message')

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'error message'
      })
   })

   it('handle corrupted data', async () => {
      fetchMock.status(200).throwOnData('corrupted data')

      expect(await getDeveloper()).toEqual({
         status: 'fatal',
         data: 'corrupted data'
      })
   })
})

describe('PUT', () => {
   it('handle 200', async () => {
      fetchMock.status(200).data({ data: 'response' })
      const developer = { name: 'ali' }

      expect(await saveDeveloper(developer)).toEqual({
         status: 'ok',
         data: { data: 'response' }
      })
      expect(fetchMock.body).toEqual(JSON.stringify(developer))
   })

   it('handle 200 with corrupted content', async () => {
      fetchMock.status(200).throwOnData('corrupted data')

      expect(await saveDeveloper()).toEqual({
         status: 'fatal',
         data: 'corrupted data'
      })
   })

   it('handle 201', async () => {
      fetchMock.status(201).data({ data: 'result' })

      expect(await saveDeveloper()).toEqual({
         status: 'ok',
         data: { data: 'result' }
      })
   })

   it('handle 400', async () => {
      fetchMock.status(400).data({ summary: ['the summary field is required'] })

      expect(await saveDeveloper()).toEqual({
         status: 'error',
         data: { summary: ['the summary field is required'] }
      })
   })

   it('handle 404', async () => {
      fetchMock.status(404)

      expect(await saveDeveloper()).toEqual({
         status: 'fatal',
         data: 'server not found'
      })
   })
   it('handle other statuses', async () => {
      fetchMock.status(500).data('error on server')

      expect(await saveDeveloper()).toEqual({
         status: 'fatal',
         data: 'error on server'
      })
   })
})

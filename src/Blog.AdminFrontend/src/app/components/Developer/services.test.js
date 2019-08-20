import fetchMock from 'fetchMock'
import { getDeveloper, saveDeveloper, anyError, validate } from './services'
import { emptyValidator, richtextEmptyValidator } from '../../utils'

jest.mock('../../utils')

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
      fetchMock.status(400).data({
         summary: ['summary field is required']
      })

      expect(await saveDeveloper(sample)).toEqual({
         status: 'error',
         data: { summary: ['summary field is required'] }
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

describe('anyError', () => {
   test.each([
      { summaryErrors: [{ type: 1, message: 'err1' }] },
      { skillsErrors: [{ type: 1, message: 'err1' }] },
      { experiencesErrors: [{ type: 1, message: 'err1' }] }
   ])('sets hasError if developer has any error in %p', arg => {
      const init = {
         hasError: false,
         summaryErrors: [],
         skillsErrors: [],
         experiencesErrors: [],
         experiences: []
      }
      Object.assign(init, arg)

      expect(anyError(init)).toBe(true)
   })

   test.each([
      { companyErrors: [{ type: 1, message: 'err1' }] },
      { positionErrors: [{ type: 1, message: 'err1' }] },
      { startDateErrors: [{ type: 1, message: 'err1' }] },
      { endDateErrors: [{ type: 1, message: 'err1' }] },
      { contentErrors: [{ type: 1, message: 'err1' }] }
   ])('sets hasError if experience has any error in %p', arg => {
      const init = {
         hasError: false,
         summaryErrors: [],
         skillsErrors: [],
         experiencesErrors: [],
         experiences: [
            {
               companyErrors: [],
               positionErrors: [],
               startDateErrors: [],
               endDateErrors: [],
               contentErrors: []
            }
         ]
      }
      Object.assign(init.experiences[0], arg)

      expect(anyError(init)).toBe(true)
   })

   it('does not set hasError for server errors', () => {
      const init = {
         hasError: true,
         summaryErrors: [{ type: 2, message: 'err2' }],
         skillsErrors: [{ type: 2, message: 'err2' }],
         experiencesErrors: [{ type: 2, message: 'err2' }],
         experiences: [
            {
               companyErrors: [{ type: 2, message: 'err2' }],
               positionErrors: [{ type: 2, message: 'err2' }],
               startDateErrors: [{ type: 2, message: 'err2' }],
               endDateErrors: [{ type: 2, message: 'err2' }],
               contentErrors: [{ type: 2, message: 'err2' }]
            }
         ],
         sideProjectErrors: [],
         sideProjects: []
      }
      expect(anyError(init)).toBe(false)
   })
})

describe('experience validations', () => {
   it('checks emptiness', () => {
      emptyValidator.mockImplementation(field => () => field + 'Error')
      richtextEmptyValidator.mockImplementation(field => () => field + 'Error')

      const state = {
         experiences: [
            {
               company: 'Parmis',
               position: 'C# Developer',
               startDate: '2010-01-01',
               endDate: '2012-01-01',
               content: 'DESC'
            }
         ],
         sideProjects: []
      }
      validate(state)

      expect(state.experiences).toEqual([
         {
            company: 'Parmis',
            companyErrors: 'companyError',
            position: 'C# Developer',
            positionErrors: 'positionError',
            startDate: '2010-01-01',
            startDateErrors: 'start dateError',
            endDate: '2012-01-01',
            endDateErrors: 'end dateError',
            content: 'DESC',
            contentErrors: 'contentError'
         }
      ])
   })

   it('prevents experience duplication', () => {
      emptyValidator.mockImplementation((field, errors) => () => errors)
      richtextEmptyValidator.mockImplementation((field, errors) => () => errors)

      const state = {
         experiences: [
            {
               company: 'Parmis',
               companyErrors: [],
               position: 'C# Developer'
            },
            {
               company: 'Parmis',
               companyErrors: [],
               position: 'C# Developer'
            }
         ],
         sideProjects: []
      }

      validate(state)

      expect(state.experiences[0].companyErrors).toEqual([])
      expect(state.experiences[1].companyErrors).toEqual([
         { type: 1, message: 'another experience as C# Developer at Parmis already exists' }
      ])
   })

   it('does not check experiences with empty company for duplication', () => {
      emptyValidator.mockImplementation((field, errors) => () => errors)
      richtextEmptyValidator.mockImplementation((field, errors) => () => errors)

      const state = {
         experiences: [
            {
               company: '',
               position: 'C# Developer'
            },
            {
               company: '',
               position: 'C# Developer'
            }
         ],
         sideProjects: []
      }

      validate(state)

      expect(state.experiences[0].companyErrors).toBe(undefined)
      expect(state.experiences[1].companyErrors).toBe(undefined)
   })

   it('does not check experiences with empty position for duplication', () => {
      emptyValidator.mockImplementation((field, errors) => () => errors)
      richtextEmptyValidator.mockImplementation((field, errors) => () => errors)

      const state = {
         experiences: [
            {
               company: 'Parmis',
               position: ' '
            },
            {
               company: 'Parmis',
               position: ' '
            }
         ],
         sideProjects: []
      }

      validate(state)

      expect(state.experiences[0].companyErrors).toBe(undefined)
      expect(state.experiences[1].companyErrors).toBe(undefined)
   })

   it('checks date overlaps', () => {
      emptyValidator.mockImplementation((field, errors) => () => errors)
      richtextEmptyValidator.mockImplementation((field, errors) => () => errors)

      const state = {
         experiences: [
            {
               company: 'Parmis',
               position: 'C# Developer',
               startDate: '2010-1-1',
               startDateErrors: [],
               endDate: '2011-1-1'
            },
            {
               company: 'Lodgify',
               position: 'C# Developer',
               startDate: '2011-1-2',
               startDateErrors: [],
               endDate: '2012-1-1'
            },
            {
               startDate: '2010-6-1',
               startDateErrors: [],
               endDate: '2012-1-1'
            }
         ],
         sideProjects: []
      }

      validate(state)

      expect(state.experiences[0].startDateErrors).toEqual([])
      expect(state.experiences[1].startDateErrors).toEqual([])
      expect(state.experiences[2].startDateErrors).toEqual([
         { type: 1, message: 'the date overlaps with C# Developer at Parmis' }
      ])
   })
})

describe('sideProject validations', () => {
   it('checks emptiness', () => {
      emptyValidator.mockImplementation(field => () => field + 'Error')
      richtextEmptyValidator.mockImplementation(field => () => field + 'Error')
      const state = {
         experiences: [],
         sideProjects: [{ id: 1, title: '', titleErrors: [], content: '', contentErrors: [] }]
      }
      validate(state)

      expect(state.sideProjects).toEqual([
         {
            id: 1,
            title: '',
            titleErrors: 'titleError',
            content: '',
            contentErrors: 'contentError'
         }
      ])
   })

   it('prevents side project duplication', () => {
      emptyValidator.mockImplementation((field, errors) => () => errors)
      richtextEmptyValidator.mockImplementation((field, errors) => () => errors)

      const state = {
         experiences: [],
         sideProjects: [
            { id: 1, title: 'richtext', titleErrors: [] },
            { id: 2, title: 'richtext', titleErrors: [] }
         ]
      }

      validate(state)

      expect(state.sideProjects[0]).toEqual({ id: 1, title: 'richtext', titleErrors: [] })
      expect(state.sideProjects[1]).toEqual({
         id: 2,
         title: 'richtext',
         titleErrors: [{ type: 1, message: 'a project with this title already exists' }]
      })
   })

   it('does not check side project with empty title for duplication', () => {
      emptyValidator.mockImplementation((field, errors) => () => errors)
      richtextEmptyValidator.mockImplementation((field, errors) => () => errors)

      const state = {
         experiences: [],
         sideProjects: [
            { id: 1, title: '', titleErrors: [] },
            { id: 2, title: ' ', titleErrors: [] }
         ]
      }

      validate(state)

      expect(state.sideProjects[0]).toEqual({ id: 1, title: '', titleErrors: [] })
      expect(state.sideProjects[1]).toEqual({ id: 2, title: ' ', titleErrors: [] })
   })
})

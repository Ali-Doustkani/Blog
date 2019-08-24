import React from 'react'
import PropTypes from 'prop-types'
import { useValidation } from './validation'
import { mount } from 'enzyme'

const Host = ({ state }) => {
   const [errors, validate] = useValidation()
   React.useEffect(() => {
      validate(state)
   }, [])
   return <div errors={errors} />
}
Host.propTypes = {
   state: PropTypes.object
}

const run = state =>
   mount(<Host state={state} />)
      .find('div')
      .prop('errors')

it('checks for empty summaries', () => {
   let result = run({ summary: '<p contenteditable></p>' })
   expect(result.summaryErrors).toEqual(['summary is required'])
   expect(result.hasError).toBe(true)

   result = run({ summary: '<p contenteditable>HELLO</p>' })
   expect(result.summaryErrors).toEqual([])
})

it('checks for empty skills', () => {
   let result = run({ skills: '' })
   expect(result.skillsErrors).toEqual(['skills is required'])
   expect(result.hasError).toBe(true)

   result = run({ skills: 'C#' })
   expect(result.skillsErrors).toEqual([])
})

describe('experiences', () => {
   it('checks for experience counts', () => {
      expect(run({ experiences: [] }).experiencesErrors).toContain(
         'You need at least one experience'
      )
      expect(
         run({
            experiences: [{ id: 1 }]
         })
      ).not.toContain('You need at least one experience')
   })

   it('checks emptiness', () => {
      const state = {
         experiences: [
            {
               company: '',
               position: '',
               startDate: '',
               endDate: '',
               content: '<p contenteditable></p>'
            }
         ]
      }
      const result = run(state)
      expect(result.hasError).toBe(true)
      expect(result.experiences).toEqual([
         {
            companyErrors: ['company is required'],
            positionErrors: ['position is required'],
            startDateErrors: ['start date is required'],
            endDateErrors: ['end date is required'],
            contentErrors: ['content is required']
         }
      ])
   })

   it('prevents experience duplication', () => {
      const state = {
         experiences: [
            {
               id: 1,
               company: 'Parmis',
               position: 'C# Developer',
               startDate: '2010-1-1',
               endDate: '2011-1-1',
               content: '<p>DESC</p>'
            },
            {
               id: 2,
               company: 'Parmis',
               position: 'C# Developer',
               startDate: '2012-1-1',
               endDate: '2013-1-1',
               content: '<p>DESC</p>'
            }
         ]
      }

      const errors = run(state)
      expect(errors.hasError).toBe(true)
      expect(errors.experiences[0].companyErrors).toEqual([])
      expect(errors.experiences[1].companyErrors).toEqual([
         'another experience as C# Developer at Parmis already exists'
      ])
   })

   it('does not check experiences with empty company for duplication', () => {
      const state = {
         experiences: [
            {
               id: 1,
               company: '',
               position: 'C# Developer'
            },
            {
               id: 2,
               company: '',
               position: 'C# Developer'
            }
         ]
      }

      const errors = run(state)
      expect(errors.hasError).toBe(true)
      expect(errors.experiences[0].companyErrors).toEqual(['company is required'])
      expect(errors.experiences[1].companyErrors).toEqual(['company is required'])
   })

   it('does not check experiences with empty position for duplication', () => {
      const state = {
         experiences: [
            {
               id: 1,
               company: 'Parmis',
               position: ' '
            },
            {
               id: 2,
               company: 'Parmis',
               position: ' '
            }
         ]
      }

      const errors = run(state)
      expect(errors.hasError).toBe(true)
      expect(errors.experiences[0].companyErrors).toEqual([])
      expect(errors.experiences[1].companyErrors).toEqual([])
   })

   it('checks date overlaps', () => {
      const state = {
         experiences: [
            {
               id: 1,
               company: 'Parmis',
               position: 'C# Developer',
               startDate: '2010-1-1',
               endDate: '2011-1-1'
            },
            {
               id: 2,
               company: 'Lodgify',
               position: 'C# Developer',
               startDate: '2011-1-2',
               endDate: '2012-1-1'
            },
            {
               id: 3,
               startDate: '2010-6-1',
               endDate: '2012-1-1'
            }
         ]
      }

      const errors = run(state)
      expect(errors.hasError).toBe(true)
      expect(errors.experiences[0].startDateErrors).toEqual([])
      expect(errors.experiences[1].startDateErrors).toEqual([])
      expect(errors.experiences[2].startDateErrors).toEqual([
         'the date overlaps with C# Developer at Parmis'
      ])
   })
})

describe('sideProject validations', () => {
   it('checks emptiness', () => {
      const state = {
         sideProjects: [{ id: 1, title: '', titleErrors: [], content: '', contentErrors: [] }]
      }
      const errors = run(state)
      expect(errors.hasError).toBe(true)
      expect(errors.sideProjects).toEqual([
         {
            titleErrors: ['title is required'],
            contentErrors: ['content is required']
         }
      ])
   })

   it('prevents side project duplication', () => {
      const state = {
         sideProjects: [
            { id: 1, title: 'richtext', content: 'DESC1' },
            { id: 2, title: 'richtext', content: 'DESC2' }
         ]
      }

      const result = run(state)

      expect(result.hasError).toBe(true)
      expect(result.sideProjects[0]).toEqual({ titleErrors: [], contentErrors: [] })
      expect(result.sideProjects[1]).toEqual({
         titleErrors: ['a project with this title already exists'],
         contentErrors: []
      })
   })

   it('does not check side project with empty title for duplication', () => {
      const state = {
         sideProjects: [
            { id: 1, title: '', content: 'DESC' },
            { id: 2, title: ' ', content: 'DESC' }
         ]
      }

      const result = run(state)

      expect(result.hasError).toBe(true)
      expect(result.sideProjects[0]).toEqual({
         titleErrors: ['title is required'],
         contentErrors: []
      })
      expect(result.sideProjects[1]).toEqual({
         titleErrors: ['title is required'],
         contentErrors: []
      })
   })
})

it('returns false for hasError when there is no error', () => {
   const state = {
      summary: '<p contenteditable>Hello</p>',
      skills: 'C#',
      experiences: [
         {
            id: 1,
            company: 'Parmis',
            position: 'C# Developer',
            startDate: '2010-1-1',
            endDate: '2011-1-1',
            content: '<p contenteditable>DESC</p>'
         }
      ]
   }
   expect(run(state).hasError).toBe(false)
})

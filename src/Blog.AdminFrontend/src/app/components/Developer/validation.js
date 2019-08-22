import React from 'react'
import * as validators from '../../utils/validations'
import { compose } from '../../utils/fn'

const initialize = () => ({
   hasError: false,
   summaryErrors: '',
   skillsErrors: '',
   experiencesErrors: [],
   experiences: [],
   sideProjects: []
})

const requiredMessage = result => (result.isEmpty ? [`${result.field} is required`] : [])

const isEmpty = compose(
   requiredMessage,
   input => ({ ...input, isEmpty: validators.isEmpty(input.value) })
)

const isRichtextEmpty = compose(
   requiredMessage,
   input => ({ ...input, isEmpty: validators.isRichtextEmpty(input.value) })
)

const useValidation = () => {
   const [errors, setErrors] = React.useState(initialize())

   const validate = state => {
      const result = initialize()
      result.summaryErrors = isRichtextEmpty({ value: state.summary, field: 'summary' })
      result.skillsErrors = isEmpty({ value: state.skills, field: 'skills' })

      if (state.experiences && state.experiences.length) {
         result.experiences = state.experiences.map(validateExperience)
         const dup = validators.getDuplicate({
            values: state.experiences,
            prop: ['company', 'position']
         })
         if (dup) {
            const index = state.experiences.findIndex(x => x.id === dup.id)
            const experienceResult = result.experiences[index]
            experienceResult.companyErrors.push(
               `another experience as ${dup.position} at ${dup.company} already exists`
            )
         }

         const [overlapper, overlapped] = validators.getOverlap(state.experiences)
         if (overlapper) {
            const index = state.experiences.findIndex(x => x.id == overlapper.id)
            const experienceResult = result.experiences[index]
            experienceResult.startDateErrors.push(
               `the date overlaps with ${overlapped.position} at ${overlapped.company}`
            )
         }
      } else {
         result.experiencesErrors.push('you need at least one experience')
      }

      if (state.sideProjects) {
         result.sideProjects = state.sideProjects.map(validateSideProject)

         const dup = validators.getDuplicate({
            values: state.sideProjects,
            prop: 'title'
         })
         if (dup) {
            const index = state.sideProjects.findIndex(x => x.id === dup.id)
            const projResult = result.sideProjects[index]
            projResult.titleErrors.push('a project with this title already exists')
         }
      }

      result.hasError = hasError(result)
      setErrors(result)
      return result
   }
   return [errors, validate]
}

const validateExperience = experience => ({
   companyErrors: isEmpty({ value: experience.company, field: 'company' }),
   positionErrors: isEmpty({ value: experience.position, field: 'position' }),
   startDateErrors: isEmpty({ value: experience.startDate, field: 'start date' }),
   endDateErrors: isEmpty({ value: experience.endDate, field: 'end date' }),
   contentErrors: isRichtextEmpty({ value: experience.content, field: 'content' })
})

const validateSideProject = sideProject => ({
   titleErrors: isEmpty({ value: sideProject.title, field: 'title' }),
   contentErrors: isRichtextEmpty({ value: sideProject.content, field: 'content' })
})

const hasError = errors =>
   Boolean(
      errors.summaryErrors.length ||
         errors.skillsErrors.length ||
         errors.experiencesErrors.length ||
         errors.experiences.some(
            x =>
               x.companyErrors.length ||
               x.positionErrors.length ||
               x.startDateErrors.length ||
               x.endDateErrors.length ||
               x.contentErrors.length
         ) ||
         errors.sideProjects.some(x => x.titleErrors.length || x.contentErrors.length)
   )

export { useValidation }

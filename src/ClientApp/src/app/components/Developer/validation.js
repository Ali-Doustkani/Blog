import React from 'react'
import * as validators from '../../utils/validations'
import { compose } from '../../utils/fn'

const initialize = () => ({
   hasError: false,
   summaryErrors: '',
   skillsErrors: '',
   experiencesErrors: [],
   experiences: [],
   sideProjects: [],
   educations: []
})

const requiredMessage = result => (result.isEmpty ? [`${result.field} is required`] : [])

const emptyArrayMessage = result =>
   result.isEmpty ? [`You need at least one ${result.field}`] : []

const isEmpty = compose(
   requiredMessage,
   input => ({ ...input, isEmpty: validators.isEmpty(input.value) })
)

const isRichtextEmpty = compose(
   requiredMessage,
   input => ({ ...input, isEmpty: validators.isRichtextEmpty(input.value) })
)

const isArrayEmpty = compose(
   emptyArrayMessage,
   input => ({ ...input, isEmpty: validators.isArrayEmpty(input.value) })
)

const validateExperience = experience => ({
   companyErrors: isEmpty({ value: experience.company, field: 'company' }),
   positionErrors: isEmpty({ value: experience.position, field: 'position' }),
   startDateErrors: isEmpty({ value: experience.startDate, field: 'start date' }),
   endDateErrors: isEmpty({ value: experience.endDate, field: 'end date' }),
   contentErrors: isRichtextEmpty({ value: experience.content, field: 'content' })
})

const checkExperienceDup = (state, result) => {
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
}

const checkExperienceOverlaps = (state, result) => {
   const [overlapper, overlapped] = validators.getOverlap(state.experiences)
   if (overlapper) {
      const index = state.experiences.findIndex(x => x.id == overlapper.id)
      const experienceResult = result.experiences[index]
      experienceResult.startDateErrors.push(
         `the date overlaps with ${overlapped.position} at ${overlapped.company}`
      )
   }
}

const validateSideProject = sideProject => ({
   titleErrors: isEmpty({ value: sideProject.title, field: 'title' }),
   contentErrors: isRichtextEmpty({ value: sideProject.content, field: 'content' })
})

const checkSideProjectDup = (state, result) => {
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

const validateEducation = education => ({
   degreeErrors: isEmpty({ value: education.degree, field: 'degree' }),
   universityErrors: isEmpty({ value: education.university, field: 'university' }),
   startDateErrors: isEmpty({ value: education.startDate, field: 'start date' }),
   endDateErrors: isEmpty({ value: education.endDate, field: 'end date' })
})

const checkEducationDup = (state, result) => {
   const dup = validators.getDuplicate({
      values: state.educations,
      prop: ['degree', 'university']
   })
   if (dup) {
      const index = state.educations.findIndex(x => x.id === dup.id)
      const projResult = result.educations[index]
      projResult.degreeErrors.push('an education with this degree & university already exists')
   }
}

const checkEducationOverlaps = (state, result) => {
   const [overlapper, overlapped] = validators.getOverlap(state.educations)
   if (overlapper) {
      const index = state.educations.findIndex(x => x.id == overlapper.id)
      const educationResult = result.educations[index]
      educationResult.startDateErrors.push(
         `the date overlaps with ${overlapped.degree} at ${overlapped.university}`
      )
   }
}

const useValidation = () => {
   const [errors, setErrors] = React.useState(initialize())

   const validate = state => {
      const result = initialize()
      result.summaryErrors = isRichtextEmpty({ value: state.summary, field: 'summary' })
      result.skillsErrors = isEmpty({ value: state.skills, field: 'skills' })
      result.experiencesErrors = isArrayEmpty({ value: state.experiences, field: 'experience' })

      if (state.experiences) {
         result.experiences = state.experiences.map(validateExperience)
         checkExperienceDup(state, result)
         checkExperienceOverlaps(state, result)
      }

      if (state.sideProjects) {
         result.sideProjects = state.sideProjects.map(validateSideProject)
         checkSideProjectDup(state, result)
      }

      if (state.educations) {
         result.educations = state.educations.map(validateEducation)
         checkEducationDup(state, result)
         checkEducationOverlaps(state, result)
      }

      result.hasError = hasError(result)
      setErrors(result)
      return result
   }
   return [errors, validate]
}

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
         errors.sideProjects.some(x => x.titleErrors.length || x.contentErrors.length) ||
         errors.educations.some(
            x =>
               x.degreeErrors.length ||
               x.universityErrors.length ||
               x.startDateErrors.length ||
               x.endDateErrors.length
         )
   )

export { useValidation }

import { emptyValidator, richtextEmptyValidator } from '../../utils'

const url_developer =
   process.env.NODE_ENV === 'production' ? '/api/developer' : 'http://localhost:3000/api/developer'

const ok = data => ({
   status: 'ok',
   data: data || null
})

const error = data => ({
   status: 'error',
   data
})

const fatal = msg => ({
   status: 'fatal',
   data: msg
})

const processResponse = async response => {
   switch (response.status) {
      case 200:
      case 201:
         return ok(await response.json())
      case 204:
         return ok()
      case 400:
         return error(await response.json())
      case 404:
         return fatal('server not found')
      default:
         return fatal(await response.text())
   }
}

const getDeveloper = async () => {
   try {
      return await processResponse(
         await fetch(url_developer, {
            headers: {
               Accept: 'application/json'
            }
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

const saveDeveloper = async state => {
   try {
      return await processResponse(
         await fetch(url_developer, {
            method: 'PUT',
            headers: {
               'Content-Type': 'application/json'
            },
            body: JSON.stringify(toDeveloper(state))
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

const toDeveloper = state => {
   const experiences = state.experiences.map(exp => ({
      id: exp.id,
      company: exp.company,
      position: exp.position,
      startDate: exp.startDate,
      endDate: exp.endDate,
      content: exp.content
   }))
   return {
      summary: state.summary,
      skills: state.skills,
      experiences
   }
}

const clientError = err => err.type === 1

const anyError = state =>
   Boolean(
      state.summaryErrors.some(clientError) ||
         state.skillsErrors.some(clientError) ||
         state.experiencesErrors.some(clientError) ||
         state.experiences.some(
            exp =>
               exp.companyErrors.some(clientError) ||
               exp.positionErrors.some(clientError) ||
               exp.startDateErrors.some(clientError) ||
               exp.endDateErrors.some(clientError) ||
               exp.contentErrors.some(clientError)
         )
   )

const validate = state => {
   state.summaryErrors = richtextEmptyValidator('summary', state.summaryErrors)(state.summary)
   state.skillsErrors = emptyValidator('skills', state.skillsErrors)(state.skills)
   state.experiences.forEach(checkExperienceEmptiness)
   checkDuplicateExperiences(state.experiences)
   checkDateOverlaps(state.experiences)
}

const checkExperienceEmptiness = experience => {
   experience.companyErrors = emptyValidator('company', experience.companyErrors)(
      experience.company
   )
   experience.positionErrors = emptyValidator('position', experience.positionErrors)(
      experience.position
   )
   experience.startDateErrors = emptyValidator('start date', experience.startDateErrors)(
      experience.startDate
   )
   experience.endDateErrors = emptyValidator('end date', experience.endDateErrors)(
      experience.endDate
   )
   experience.contentErrors = richtextEmptyValidator('content', experience.contentErrors)(
      experience.content
   )
   return experience
}

const checkDuplicateExperiences = experiences => {
   const reverse = experiences
      .filter(exp => isNotEmpty(exp.company) && isNotEmpty(exp.position))
      .reverse()
   for (const experience of reverse) {
      if (experiences.some(isDuplicate(experience))) {
         experience.companyErrors.push(experienceDuplicateError(experience))
      }
      return
   }
}

const isDuplicate = experience1 => experience2 =>
   experience1 !== experience2 &&
   experience2.company === experience1.company &&
   experience2.position === experience1.position

const experienceDuplicateError = experience => ({
   type: 1,
   message: `another experience as ${experience.position} at ${experience.company} already exists`
})

const checkDateOverlaps = experiences => {
   const reversed = experiences
      .filter(exp => isNotEmpty(exp.startDate) && isNotEmpty(exp.endDate))
      .reverse()

   for (const experience of reversed) {
      const found = experiences.find(overlaps(experience))
      if (found) {
         experience.startDateErrors.push(experienceDateOverlapError(found))
         return
      }
   }
}

const overlaps = experience1 => experience2 =>
   (new Date(experience1.startDate) > new Date(experience2.startDate) &&
      new Date(experience1.startDate) < new Date(experience2.endDate)) ||
   (new Date(experience1.endDate) > new Date(experience2.startDate) &&
      new Date(experience1.endDate) < new Date(experience2.endDate))

const experienceDateOverlapError = experience => ({
   type: 1,
   message: `the date overlaps with ${experience.position} at ${experience.company}`
})

const isNotEmpty = text => !/^\s*$/.test(text)

export { getDeveloper, saveDeveloper, anyError, validate }

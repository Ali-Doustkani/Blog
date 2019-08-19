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
   state.experiences.forEach(validateExperience)
}

const validateExperience = experience => {
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

export { getDeveloper, saveDeveloper, anyError, validate }

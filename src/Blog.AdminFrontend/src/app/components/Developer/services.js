const url_developer =
   process.env.NODE_ENV === 'production' ? '/api/developer' : 'http://localhost:3000/api/developer'

const ok = data => ({
   status: 'ok',
   data: data || null
})

const error = data => ({
   status: 'error',
   data: data.validationErrors
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

export { getDeveloper, saveDeveloper }

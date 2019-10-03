const url_developer = '/api/developer'
const url_posts = '/api/posts'

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
      case 401:
         return fatal('not authenticated')
      case 403:
         return fatal('forbidden')
      case 404:
         return fatal('server not found')
      default:
         return fatal(await response.text())
   }
}

const getDeveloper = async auth0 => {
   try {
      const token = await auth0.getAccessToken()
      return await processResponse(
         await fetch(url_developer, {
            headers: {
               Accept: 'application/json',
               Authorization: 'Bearer ' + token
            }
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

const saveDeveloper = async (state, auth0) => {
   try {
      const token = await auth0.getAccessToken()
      return await processResponse(
         await fetch(url_developer, {
            method: 'PUT',
            headers: {
               'Content-Type': 'application/json',
               Authorization: 'Bearer ' + token
            },
            body: JSON.stringify(state)
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

const getPostItems = async () => {
   try {
      return await processResponse(
         await fetch(url_posts, {
            headers: {
               Accept: 'application/json'
            }
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

export { getDeveloper, saveDeveloper, getPostItems }

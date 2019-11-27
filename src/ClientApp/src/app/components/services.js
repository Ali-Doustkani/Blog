const url_developer = '/api/developer'
const url_posts = '/api/posts'
const url_drafts = '/api/drafts'

function ok(data) {
   return {
      status: 'ok',
      data: data || null
   }
}

function error(data) {
   return {
      status: 'error',
      data
   }
}

function fatal(msg) {
   return {
      status: 'fatal',
      data: msg
   }
}

async function processResponse(response) {
   switch (response.status) {
      case 200:
      case 201:
         const contentType = response.headers.get('content-type')
         if (contentType && contentType.indexOf('application/json') !== -1) {
            return ok(await response.json())
         }
         return ok()
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

async function getDeveloper(auth0) {
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

async function saveDeveloper(state, auth0) {
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

async function getDraftItems() {
   try {
      return await processResponse(
         await fetch(url_drafts, {
            headers: {
               Accept: 'application/json'
            }
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

async function getDraft(id) {
   try {
      return await processResponse(await fetch(`${url_drafts}/${id}`))
   } catch (err) {
      return fatal(err.message)
   }
}

async function postDraft(post) {
   try {
      return await processResponse(
         await fetch(url_drafts, {
            method: 'POST',
            headers: {
               'Content-Type': 'application/json'
            },
            body: JSON.stringify(post)
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

async function patchDraft(post) {
   try {
      return await processResponse(
         await fetch(`${url_drafts}/${post.id}`, {
            method: 'PATCH',
            headers: {
               'Content-Type': 'application/json'
            },
            body: JSON.stringify({ title: post.title, content: post.content })
         })
      )
   } catch (err) {
      return fatal(err.message)
   }
}

export { getDeveloper, saveDeveloper, getDraftItems, postDraft, patchDraft, getDraft }

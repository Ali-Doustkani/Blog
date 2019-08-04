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
   try {
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
   } catch (err) {
      return fatal(err.message)
   }
}

const getDeveloper = async () =>
   await processResponse(
      await fetch(url_developer, {
         headers: {
            Accept: 'application/json'
         }
      })
   )

const saveDeveloper = async developer =>
   await processResponse(
      await fetch(url_developer, {
         method: 'PUT',
         headers: {
            'Content-Type': 'application/json'
         },
         body: JSON.stringify(developer)
      })
   )

export { getDeveloper, saveDeveloper }

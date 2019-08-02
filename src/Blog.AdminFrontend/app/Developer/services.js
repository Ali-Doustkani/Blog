const url_developer =
   process.env.NODE_ENV === 'production' ? '/api/developer' : 'http://localhost:3000/developer'

async function getDeveloper() {
   try {
      const response = await fetch(url_developer, {
         headers: {
            Accept: 'application/json'
         }
      })
      if (response.status === 204) {
         return {
            status: 'ok',
            developer: null
         }
      }
      const developer = await response.json()
      return {
         status: 'ok',
         developer
      }
   } catch (err) {
      return {
         status: 'error',
         message: err.message
      }
   }
}

async function saveDeveloper(developer) {
   const response = await fetch(url_developer, {
      method: 'PUT',
      headers: {
         'Content-Type': 'application/json'
      },
      body: JSON.stringify(developer)
   })
   return await response.json()
}

export { getDeveloper, saveDeveloper }

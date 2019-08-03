const url_developer =
   process.env.NODE_ENV === 'production' ? '/api/developer' : 'http://localhost:3000/api/developer'

async function getDeveloper() {
   try {
      const response = await fetch(url_developer, {
         headers: {
            Accept: 'application/json'
         }
      })
      if (response.status === 200) {
         const developer = await response.json()
         return {
            status: 'ok',
            developer
         }
      }
      if (response.status === 204) {
         return {
            status: 'ok',
            developer: null
         }
      }
      if (response.status === 404) {
         return {
            status: 'error',
            message: 'server not found'
         }
      }
      return {
         status: 'error',
         message: 'problem loading data'
      }
   } catch (err) {
      return {
         status: 'error',
         message: err.message
      }
   }
}

async function saveDeveloper(developer) {
   try {
      const response = await fetch(url_developer, {
         method: 'PUT',
         headers: {
            'Content-Type': 'application/json'
         },
         body: JSON.stringify(developer)
      })
      if (response.status === 200) {
         const data = await response.json()
         return {
            status: 'ok',
            data
         }
      }
   } catch (ex) {
      return {
         status: 'error',
         message: ex.message
      }
   }
}

export { getDeveloper, saveDeveloper }

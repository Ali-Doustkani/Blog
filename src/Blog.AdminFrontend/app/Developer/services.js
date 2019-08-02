const url_developer =
   process.env.NODE_ENV === 'production' ? '/api/developer' : 'http://localhost:3000/developer'

async function getDeveloper() {
   const response = await fetch(url_developer, {
      headers: {
         Accept: 'application/json'
      }
   })
   if (response.status === 204) {
      return null
   }
   try {
      return await response.json()
   } catch (err) {
      return null
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

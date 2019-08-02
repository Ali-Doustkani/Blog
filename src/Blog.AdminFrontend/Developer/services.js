async function getDeveloper() {
   const response = await fetch('/api/developer')
   if (response.status === 204) {
      return null
   }
   return await response.json()
}

async function saveDeveloper(developer) {
   const response = await fetch('/api/developer', {
      method: 'PUT',
      headers: {
         'Content-Type': 'application/json'
      },
      body: JSON.stringify(developer)
   })
   return await response.json()
}

export { getDeveloper, saveDeveloper }

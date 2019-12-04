import { useEffect, useState } from 'react'
import createAuth0Client from '@auth0/auth0-spa-js'

const config =
   process.env.NODE_ENV === 'production'
      ? {
           info: {
              domain: 'ali-doustkani.auth0.com',
              client_id: 'wZmKb30P6lnFD4Olh2ryazHqBwX8KU8L',
              redirect_uri: 'https://alidoustkani.com/admin',
              audience: 'https://alidoustkani.com/api'
           },
           logout: {
              returnTo: 'https://alidoustkani.com'
           }
        }
      : {
           info: {
              domain: 'ali-doustkani.auth0.com',
              client_id: 'wZmKb30P6lnFD4Olh2ryazHqBwX8KU8L',
              redirect_uri: 'http://localhost:5000/admin',
              audience: 'http://localhost:5000/api'
           },
           logout: {
              returnTo: 'http://localhost:5000'
           }
        }

const setHistory = appState => {
   window.history.replaceState(
      {},
      document.title,
      appState && appState.targetUrl ? appState.targetUrl : window.location.pathname
   )
}

function useAuth0() {
   const [client, setClient] = useState()
   const [loading, setLoading] = useState(true)

   useEffect(() => {
      const init = async () => {
         const client = await createAuth0Client(config.info)
         setClient(client)
         if (window.location.search.includes('code=')) {
            const { appState } = await client.handleRedirectCallback()
            setHistory(appState)
         }
         const authenticated = await client.isAuthenticated()
         if (!authenticated) {
            await client.loginWithRedirect()
         } else {
            setLoading(false)
         }
      }
      if (process.env.NODE_ENV === 'testing') {
         setLoading(false)
         setClient({
            logout: () => {},
            getTokenSilently: async () => Promise.resolve('TOK')
         })
      } else {
         init()
      }
   }, [])

   return {
      loading,
      logout: () => client.logout(config.logout),
      getAccessToken: async () => await client.getTokenSilently()
   }
}

export default useAuth0

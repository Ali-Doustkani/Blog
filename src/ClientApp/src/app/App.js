import React, { useEffect, useState } from 'react'
import Developer from './components/Developer/Developer'
import { InstantMessage, notify } from './components/Notification'
import { Loader } from 'Controls'
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import createAuth0Client from '@auth0/auth0-spa-js'
import config from './auth_config'

const DashboardPage = () => (
   <main>
      <h1>Dashboard</h1>
   </main>
)

const PostPage = () => (
   <main>
      <h1>Post</h1>
   </main>
)

const DeveloperPage = auth0 => () => (
   <main>
      <Developer notify={notify} auth0={auth0} />
      <InstantMessage />
   </main>
)

const setHistory = appState => {
   window.history.replaceState(
      {},
      document.title,
      appState && appState.targetUrl ? appState.targetUrl : window.location.pathname
   )
}

function App() {
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
      init()
   }, [])

   if (loading) {
      return <Loader text="Authenticating..." />
   }

   return (
      <Router basename="/newadmin">
         <>
            <header>
               <ul className="menu-bar">
                  <li>
                     <Link to="/">Dashboard</Link>
                  </li>
                  <li>
                     <div className="separator" />
                  </li>
                  <li>
                     <Link to="/post">New Post</Link>
                  </li>
                  <li>
                     <div className="separator" />
                  </li>
                  <li>
                     <Link to="/developer">Developer</Link>
                  </li>
                  <li className="end">
                     <a className="logout" onClick={() => client.logout(config.logout)}>
                        <i className="fas fa-power-off" />
                     </a>
                  </li>
               </ul>
            </header>
            <Route path="/" exact component={DashboardPage} />
            <Route path="/post" component={PostPage} />
            <Route path="/developer" component={DeveloperPage(client)} />
         </>
      </Router>
   )
}

export default App

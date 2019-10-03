import React from 'react'
import Developer from './components/Developer/Developer'
import PostList from './components/PostList/PostList'
import { InstantMessage, notify } from './components/Notification'
import { Loader } from 'Controls'
import { BrowserRouter as Router, Route, Link } from 'react-router-dom'
import useAuth0 from './useAuth0'

const PostPage = () => (
   <main>
      <PostList />
   </main>
)

const DeveloperPage = auth0 => () => (
   <main>
      <Developer notify={notify} auth0={auth0} />
      <InstantMessage />
   </main>
)

function App() {
   const auth0 = useAuth0()

   if (auth0.loading) {
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
                     <Link to="/developer">Developer</Link>
                  </li>
                  <li className="end">
                     <a className="logout" onClick={() => auth0.logout()}>
                        <i className="fas fa-power-off" />
                     </a>
                  </li>
               </ul>
            </header>
            <Route path="/" exact component={PostPage} />
            <Route path="/developer" component={DeveloperPage(auth0)} />
         </>
      </Router>
   )
}

export default App

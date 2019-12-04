import React, { useState, useEffect } from 'react'
import MainMenu from './components/MainMenu/MainMenu'
import Developer from './components/Developer/Developer'
import PostList from './components/PostList/PostList'
import Post from './components/Post/Post'
import { InstantMessage, notify } from './components/Notification'
import { Loader } from 'Controls'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import useAuth0 from './useAuth0'

const PostListPage = auth0 => () => (
   <main>
      <PostList auth0={auth0} />
   </main>
)

const PostPage = auth0 => () => (
   <main>
      <Post auth0={auth0} />
   </main>
)

const DeveloperPage = auth0 => () => (
   <main>
      <Developer notify={notify} auth0={auth0} />
      <InstantMessage />
   </main>
)

function App() {
   const [isMenuOpen, setMenuOpen] = useState(false)
   const auth0 =
      process.env.NODE_ENV === 'development'
         ? { loading: false, getAccessToken: () => {} }
         : useAuth0()
   const showMenu = () => setMenuOpen(!isMenuOpen)
   const closeMenu = () => setMenuOpen(false)

   useEffect(() => {
      if (isMenuOpen) {
         document.addEventListener('click', closeMenu)
      }
      return () => document.removeEventListener('click', closeMenu)
   }, [isMenuOpen])

   if (auth0.loading) {
      return <Loader text="Authenticating..." />
   }

   return (
      <Router basename="/admin">
         <>
            <MainMenu
               open={isMenuOpen}
               onOpenClick={() => showMenu()}
               onLogout={() => auth0.logout()}
            />
            <Switch>
               <Route path="/" exact component={PostListPage(auth0)} />
               <Route path="/developer" component={DeveloperPage(auth0)} />
               <Route path="/post/:id?" children={PostPage(auth0)} />
            </Switch>
         </>
      </Router>
   )
}

export default App

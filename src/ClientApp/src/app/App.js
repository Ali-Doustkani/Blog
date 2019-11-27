import React, { useState, useEffect } from 'react'
import MainMenu from './components/MainMenu/MainMenu'
import Developer from './components/Developer/Developer'
import PostList from './components/PostList/PostList'
import Post from './components/Post/Post'
import { InstantMessage, notify } from './components/Notification'
import { Loader } from 'Controls'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import useAuth0 from './useAuth0'

const PostListPage = () => (
   <main>
      <PostList />
   </main>
)

const PostPage = () => (
   <main>
      <Post />
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
         ? {
              loading: false
           }
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
      <Router basename="/newadmin">
         <>
            <MainMenu
               open={isMenuOpen}
               onOpenClick={() => showMenu()}
               onLogout={() => auth0.logout()}
            />
            <Switch>
               <Route path="/" exact component={PostListPage} />
               <Route path="/developer" component={DeveloperPage(auth0)} />
               <Route path="/post/:id?" children={<PostPage />} />
            </Switch>
         </>
      </Router>
   )
}

export default App

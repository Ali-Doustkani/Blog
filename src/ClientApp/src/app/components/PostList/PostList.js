import React, { useEffect, useReducer } from 'react'
import PostItem from './PostItem'
import { getDraftItems } from '../services'
import { Loader, Message, Button } from 'Controls'

const initial = {
   status: 'loading',
   errorMessage: '',
   posts: []
}

function reducer(state, action) {
   switch (action.type) {
      case 'LOAD_POSTS':
         if (action.result.status === 'ok') {
            return {
               posts: action.result.data,
               status: 'normal',
               errorMessage: ''
            }
         } else {
            return {
               posts: [],
               status: 'error',
               errorMessage: action.result.data
            }
         }
      default:
         throw new Error('Unsupported action type')
   }
}

function PostList() {
   const [state, dispatch] = useReducer(reducer, initial)
   let mounted = true

   useEffect(() => {
      fetchDraftItems()
      return () => (mounted = false)
   }, [])

   const fetchDraftItems = async () => {
      const result = await getDraftItems()
      if (mounted) {
         dispatch({ type: 'LOAD_POSTS', result })
      }
   }

   if (state.status === 'loading') {
      return <Loader text="Loading posts..." />
   } else if (state.status === 'error') {
      return (
         <Message message={state.errorMessage} onTryAgain={async () => await fetchDraftItems()} />
      )
   }

   return (
      <div data-testid="post-list" className="post-list">
         <h1>Posts</h1>
         <button
            onClick={() => {
               window.location.assign('/post')
            }}
         >
            New Post
         </button>
         <ol>
            {state.posts.map(x => {
               return (
                  <PostItem
                     key={x.id}
                     id={x.id}
                     title={x.title}
                     date={x.date}
                     published={x.published}
                     onEdit={id => window.location.assign(`/post/${id}`)}
                  />
               )
            })}
         </ol>
      </div>
   )
}

export default PostList

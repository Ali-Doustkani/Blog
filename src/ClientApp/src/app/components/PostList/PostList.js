import React, { useEffect, useReducer } from 'react'
import PostItem from './PostItem'
import { getPostItems } from '../services'
import { Loader, Message } from 'Controls'

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
      fetchPostItems()
      return () => (mounted = false)
   }, [])

   const fetchPostItems = async () => {
      const result = await getPostItems()
      if (mounted) {
         dispatch({ type: 'LOAD_POSTS', result })
      }
   }

   if (state.status === 'loading') {
      return <Loader text="Loading posts..." />
   } else if (state.status === 'error') {
      return (
         <Message message={state.errorMessage} onTryAgain={async () => await fetchPostItems()} />
      )
   }

   return (
      <div data-testid="post-list" className="post-list">
         <h1>Posts</h1>
         <ol>
            {state.posts.map(x => {
               const { id, ...rest } = x
               return <PostItem key={id} {...rest} />
            })}
         </ol>
      </div>
   )
}

export default PostList

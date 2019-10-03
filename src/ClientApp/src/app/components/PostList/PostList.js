import React, { useEffect, useState } from 'react'
import PostItem from './PostItem'
import { getPostItems } from '../services'
import { Loader } from 'Controls'

function PostList() {
   const [posts, setPosts] = useState([])
   const [loading, setLoading] = useState(true)

   useEffect(() => {
      let mounted = true
      const init = async () => {
         const result = await getPostItems()
         if (mounted) {
            setPosts(result.data)
            setLoading(false)
         }
      }
      init()
      return () => {
         mounted = false
      }
   }, [])

   if (loading) {
      return <Loader text="Loading posts..." />
   }

   return (
      <div data-cy="post-list" className="post-list">
         <h1>Posts</h1>
         <ol>
            {posts.map(x => {
               const { id, ...rest } = x
               return <PostItem key={id} {...rest} />
            })}
         </ol>
      </div>
   )
}

export default PostList

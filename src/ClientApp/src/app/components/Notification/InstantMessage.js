import React, { useEffect, useState } from 'react'
import Message from './Message'
import { subscribe, unsubscribe } from './subscription'

const InstantMessage = () => {
   const [appearance, setAppearance] = useState()
   const [message, setMessage] = useState()
   const [visible, setVisible] = useState(false)

   useEffect(() => {
      if (visible) {
         const id = setTimeout(() => {
            setVisible(false)
         }, 3000)
         return () => clearTimeout(id)
      }
   }, [visible])

   useEffect(() => {
      subscribe((message, appearance) => {
         setAppearance(appearance)
         setMessage(message)
         setVisible(true)
      })
      return () => unsubscribe()
   }, [])

   return visible ? (
      <Message message={message} appearance={appearance} onDismiss={() => setVisible(false)} />
   ) : null
}

export { InstantMessage }

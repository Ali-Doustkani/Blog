import React from 'react'
import ReactDOM from 'react-dom'
import Developer from './Developer'
import { InstantMessage, notify } from '../Notification'

ReactDOM.render(
   <>
      <Developer notify={notify} />
      <InstantMessage />
   </>,
   document.getElementById('root')
)

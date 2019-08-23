import React from 'react'
import ReactDOM from 'react-dom'
import Developer from './Developer'
import { InstantMessage, notify } from '../Notification'

ReactDOM.render(
   <>
      <Developer notify={notify} />
      <InstantMessage />
   </>,
   document.getElementsByTagName('main')[0]
)

ReactDOM.render(
   <p>© 2018 - {new Date().getFullYear()} Ali Doustkani</p>,
   document.getElementsByTagName('footer')[0]
)

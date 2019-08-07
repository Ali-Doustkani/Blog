import React from 'react'
import ReactDOM from 'react-dom'
import { ToastProvider } from 'react-toast-notifications'
import Developer from './Developer'
import Toast from '../../controls/Toast'

ReactDOM.render(
   <ToastProvider placement="bottom-center" autoDismissTimeout="3000" components={{ Toast }}>
      <Developer />
   </ToastProvider>,
   document.getElementById('root')
)

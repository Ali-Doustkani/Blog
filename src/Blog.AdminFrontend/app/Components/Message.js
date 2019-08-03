import React from 'react'

const Message = props => (
   <div className="page-message">
      <div data-testid="message-component">
         <div>{props.message}</div>
         <a onClick={props.onTryAgain} style={{ cursor: 'pointer' }}>
            Try again
         </a>
      </div>
   </div>
)

export { Message }

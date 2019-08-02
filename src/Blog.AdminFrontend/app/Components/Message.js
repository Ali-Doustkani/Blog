import React from 'react'

const Message = props => (
   <div style={{ textAlign: 'center', marginTop: 150 }}>
      <div
         style={{
            display: 'inline-block',
            background: 'rgba(107, 119, 141,.1)',
            borderRadius: 9,
            padding: '10px 30px'
         }}
         data-testid="message-component"
      >
         <div>{props.message}</div>
         <a onClick={props.onTryAgain} style={{ cursor: 'pointer' }}>
            Try again
         </a>
      </div>
   </div>
)

export { Message }

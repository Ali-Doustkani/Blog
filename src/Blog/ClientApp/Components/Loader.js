import React from 'react'

const Loader = props => (
   <div style={{ textAlign: 'center', marginTop: 150 }}>
      <div
         style={{
            display: 'inline-block',
            background: 'rgba(107, 119, 141,.1)',
            borderRadius: 9,
            padding: '10px 30px'
         }}
      >
         <img src="/images/loader.svg" style={{ width: 50 }} />
         <div>{props.text ? props.text : 'Loading...'}</div>
      </div>
   </div>
)

export { Loader }

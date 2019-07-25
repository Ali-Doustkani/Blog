import React from 'react'

const Button = props => (
   <button className="form-button" onClick={props.onClick}>
      {props.children}
   </button>
)

export { Button }

import React from 'react'

const Button = props => (
   <button data-testid={props['data-testid']} className="form-button" onClick={props.onClick}>
      {props.children}
   </button>
)

export { Button }

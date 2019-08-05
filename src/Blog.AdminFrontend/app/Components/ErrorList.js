import React from 'react'
import uuid from 'uuid/v1'

const ErrorList = props => (
   <ul data-testid="error-list">
      {props.errors.map(err => (
         <li key={uuid()} className="error">
            {err}
         </li>
      ))}
   </ul>
)

export { ErrorList }

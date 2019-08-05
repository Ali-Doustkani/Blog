import React from 'react'
import { ErrorList } from './ErrorList'

const Textarea = props => {
   let className = null,
      errorList = null
   if (props.errors && props.errors.length) {
      className = 'incorrect'
      errorList = <ErrorList errors={props.errors} />
   }

   return (
      <div className="text-group">
         <label>{props.label}</label>
         <textarea
            name={props.name}
            data-testid={props['data-testid']}
            defaultValue={props.defaultValue}
            className={className}
            onChange={props.onChange}
         />
         {errorList}
      </div>
   )
}

export { Textarea }

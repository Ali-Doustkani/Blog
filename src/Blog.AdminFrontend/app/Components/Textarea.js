import React from 'react'
import PropTypes from 'prop-types'
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
            disabled={props.disabled}
         />
         {errorList}
      </div>
   )
}

Textarea.propTypes = {
   errors: PropTypes.arrayOf(PropTypes.string),
   label: PropTypes.string,
   name: PropTypes.string,
   'data-testid': PropTypes.string,
   defaultValue: PropTypes.string,
   className: PropTypes.string,
   onChange: PropTypes.func,
   disabled: PropTypes.bool
}

export { Textarea }

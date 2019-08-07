import React from 'react'
import PropTypes from 'prop-types'
import { ErrorList } from './ErrorList'
import { emitChange, renderClassNames } from './utils'
import { DisabledContext } from '../DisabledContext'

const Textarea = props => {
   const disabled = React.useContext(DisabledContext)
   return (
      <div className="text-group">
         <label>{props.label}</label>
         <textarea
            name={props.name}
            defaultValue={props[props.name]}
            className={renderClassNames(props)}
            onChange={emitChange(props)}
            disabled={disabled}
         />
         <ErrorList errors={props[props.name + 'Errors']} />
      </div>
   )
}

Textarea.propTypes = {
   name: PropTypes.string.isRequired,
   label: PropTypes.string,
   className: PropTypes.string,
   onChange: PropTypes.func
}

export { Textarea }

import React from 'react'
import PropTypes from 'prop-types'
import { DisabledContext } from '../DisabledContext'

const renderClassNames = props => {
   const errors = props[props.name + 'Errors']
   const className = props.className
   if (errors && errors.length) {
      return className ? className + ' incorrect' : 'incorrect'
   }
   return className
}

const Input = props => {
   const disabled = React.useContext(DisabledContext)
   const ph = props.placeholder || props.name.charAt(0).toUpperCase() + props.name.slice(1)
   return (
      <input
         name={props.name}
         type={props.type}
         placeholder={ph}
         autoFocus={props.autoFocus}
         className={renderClassNames(props)}
         defaultValue={props[props.name]}
         onChange={e =>
            props.onChange({
               id: props.id,
               [props.name]: e.target.value
            })
         }
         disabled={disabled}
      />
   )
}

Input.propTypes = {
   id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
   name: PropTypes.string.isRequired,
   placeholder: PropTypes.string,
   className: PropTypes.string,
   autoFocus: PropTypes.bool,
   type: PropTypes.string,
   onChange: PropTypes.func
}

Input.defaultProps = {
   type: 'text'
}

export { Input }

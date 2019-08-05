import React from 'react'
import PropTypes from 'prop-types'

const Input = props => {
   const ph = props.placeholder || props.name.charAt(0).toUpperCase() + props.name.slice(1)
   const className =
      props.errors && props.errors.length ? props.className + ' incorrect' : props.className
   const type = props.type || 'text'
   return (
      <input
         name={props.name}
         type={type}
         data-testid={props.name + '-input'}
         placeholder={ph}
         autoFocus={props.autoFocus}
         className={className}
         defaultValue={props.value}
         onChange={props.onChange}
      />
   )
}

Input.propTypes = {
   name: PropTypes.string,
   placeholder: PropTypes.string,
   className: PropTypes.string,
   autoFocus: PropTypes.bool,
   value: PropTypes.string,
   type: PropTypes.string,
   onChange: PropTypes.func
}

export { Input }

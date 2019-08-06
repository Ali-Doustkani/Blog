import React from 'react'
import PropTypes from 'prop-types'
import { DisabledContext } from '../DisabledContext'

const Button = props => {
   const disabled = React.useContext(DisabledContext)

   return (
      <button
         data-testid={props['data-testid']}
         disabled={disabled}
         className="form-button"
         onClick={props.onClick}
      >
         {props.children}
      </button>
   )
}

Button.propTypes = {
   'data-testid': PropTypes.string,
   onClick: PropTypes.func,
   children: PropTypes.element
}

export { Button }

import React from 'react'
import PropTypes from 'prop-types'
import DisabledContext from 'DisabledContext'

const Button = props => {
   const disabled = React.useContext(DisabledContext)

   return (
      <button disabled={disabled} className="form-button" {...props}>
         {props.children}
      </button>
   )
}

Button.propTypes = {
   onClick: PropTypes.func.isRequired,
   children: PropTypes.any
}

export { Button }

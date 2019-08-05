import React from 'react'
import PropTypes from 'prop-types'

const Button = props => (
   <button
      data-testid={props['data-testid']}
      disabled={props.disabled}
      className="form-button"
      onClick={props.onClick}
   >
      {props.children}
   </button>
)

Button.propTypes = {
   'data-testid': PropTypes.string,
   onClick: PropTypes.func,
   disabled: PropTypes.bool
}

export { Button }

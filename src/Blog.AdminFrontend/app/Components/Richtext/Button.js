import React from 'react'
import PropTypes from 'prop-types'

const Button = props => (
   <button onClick={props.onClick} disabled={props.disabled}>
      {typeof props.content === 'string' ? (
         <i className={`fas fa-${props.content}`} />
      ) : (
         props.content
      )}
   </button>
)

Button.propTypes = {
   onClick: PropTypes.func.isRequired,
   content: PropTypes.oneOfType([PropTypes.string, PropTypes.object]),
   disabled: PropTypes.bool
}

export default Button

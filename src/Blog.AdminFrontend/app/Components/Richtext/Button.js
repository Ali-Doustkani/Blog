import React from 'react'
import PropTypes from 'prop-types'

const Button = props => (
   <button onClick={props.onClick}>
      {typeof props.content === 'string' ? (
         <i className={`fas fa-${props.content}`} />
      ) : (
         props.content
      )}
   </button>
)

Button.propTypes = {
   onClick: PropTypes.func.isRequired,
   content: PropTypes.oneOfType([PropTypes.string, PropTypes.object])
}

export default Button

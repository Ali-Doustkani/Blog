import React from 'react'
import PropTypes from 'prop-types'

const Message = props => {
   const icon =
      props.appearance === 'success' ? (
         <i className="fas fa-check" />
      ) : props.appearance === 'error' ? (
         <i className="fas fa-times" />
      ) : null
   return (
      <div className={'instant-message ' + props.appearance}>
         {icon}
         {props.message}
         <a onClick={props.onDismiss}>Dismiss</a>
      </div>
   )
}

Message.propTypes = {
   message: PropTypes.string.isRequired,
   appearance: PropTypes.string.isRequired,
   onDismiss: PropTypes.func
}

export default Message

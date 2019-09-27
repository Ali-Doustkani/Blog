import React from 'react'
import PropTypes from 'prop-types'

const Message = props => (
   <div className="page-message">
      <div data-cy="message-container">
         <div>{props.message}</div>
         <a onClick={props.onTryAgain} style={{ cursor: 'pointer' }}>
            Try again
         </a>
      </div>
   </div>
)

Message.propTypes = {
   message: PropTypes.string,
   onTryAgain: PropTypes.func
}

export { Message }

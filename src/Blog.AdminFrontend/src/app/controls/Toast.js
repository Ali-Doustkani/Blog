import React from 'react'
import PropTypes from 'prop-types'

const Toast = props => {
   const icon =
      props.appearance === 'success' ? (
         <i className="fas fa-check" />
      ) : props.appearance === 'error' ? (
         <i className="fas fa-times" />
      ) : null
   return (
      <div className={'toast ' + props.appearance}>
         {icon}
         {props.children}
         <a onClick={props.onDismiss}>Dismiss</a>
      </div>
   )
}

Toast.propTypes = {
   appearance: PropTypes.string,
   onDismiss: PropTypes.func,
   children: PropTypes.string
}

export default Toast

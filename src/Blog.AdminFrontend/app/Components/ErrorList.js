import React from 'react'
import PropTypes from 'prop-types'
import uuid from 'uuid/v1'

const ErrorList = props => {
   if (!props.errors || !props.errors.length) {
      return null
   }
   return (
      <ul data-testid="error-list">
         {props.errors.map(err => (
            <li key={uuid()} className="error">
               {err}
            </li>
         ))}
      </ul>
   )
}

ErrorList.propTypes = {
   errors: PropTypes.arrayOf(PropTypes.string)
}

export { ErrorList }

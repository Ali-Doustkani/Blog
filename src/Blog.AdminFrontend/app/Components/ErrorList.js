import React from 'react'
import PropTypes from 'prop-types'
import uuid from 'uuid/v1'

function flatten(errors) {
   let result = []
   for (let i = 0; i < errors.length; i++) {
      if (typeof errors[i] === 'string') {
         result.push(errors[i])
      }
      if (Array.isArray(errors[i])) {
         result = result.concat(errors[i])
      }
   }
   return result
}

const ErrorList = props => {
   if (!props.errors || !props.errors.length) {
      return null
   }

   const array = flatten(props.errors)
   if (!array.length) {
      return null
   }

   return (
      <ul data-testid="error-list">
         {array.map(err => (
            <li key={uuid()} className="error">
               {err}
            </li>
         ))}
      </ul>
   )
}

ErrorList.propTypes = {
   errors: PropTypes.arrayOf(PropTypes.any)
}

export { ErrorList }

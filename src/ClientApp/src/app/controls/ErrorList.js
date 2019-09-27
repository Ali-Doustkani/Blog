import React from 'react'
import PropTypes from 'prop-types'
import uuid from 'uuid/v1'

function flatten(errors) {
   if (!errors) {
      return []
   }
   if (typeof errors === 'string') {
      return [errors]
   }
   if (Array.isArray(errors)) {
      let result = []
      errors.forEach(item => {
         if (typeof item === 'string') {
            result.push(item)
         } else if (Array.isArray(item)) {
            result = result.concat(item)
         }
      })
      return result
   }
}

const ErrorList = props => {
   const errors = flatten(props.errors)
   return errors.length ? (
      <ul>
         {errors.map(err => (
            <li key={uuid()} className="error">
               {err}
            </li>
         ))}
      </ul>
   ) : null
}

ErrorList.propTypes = {
   errors: PropTypes.oneOfType([PropTypes.string, PropTypes.array])
}

export { ErrorList }

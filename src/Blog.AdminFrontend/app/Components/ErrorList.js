import React from 'react'
import PropTypes from 'prop-types'
import uuid from 'uuid/v1'

function flatten(props) {
   if (props.errors) {
      return props.errors
   }

   let result = []
   Object.getOwnPropertyNames(props).forEach(p => {
      if (p.endsWith('Errors') && Array.isArray(props[p])) {
         result = result.concat(props[p])
      }
   })
   return result
}

const ErrorList = props => {
   const array = flatten(props)
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

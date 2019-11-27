import React from 'react'
import PropTypes from 'prop-types'
import DisabledContext from 'DisabledContext'

const ToolbarButton = props => {
   const disabled = React.useContext(DisabledContext)
   const content =
      typeof props.content === 'string' ? (
         <i className={`fas fa-${props.content}`} />
      ) : (
         props.children
      )
   return (
      <button disabled={disabled} {...props} className="toolbarButton">
         {content}
      </button>
   )
}

ToolbarButton.propTypes = {
   onClick: PropTypes.func.isRequired,
   content: PropTypes.oneOfType([PropTypes.string, PropTypes.object]),
   children: PropTypes.element
}

export { ToolbarButton }

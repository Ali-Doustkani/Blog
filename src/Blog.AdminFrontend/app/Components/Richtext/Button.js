import React from 'react'

export default props => (
  <button onClick={props.onClick}>
    {typeof props.content === 'string' ? (
      <i className={`fas fa-${props.content}`} />
    ) : (
      props.content
    )}
  </button>
)

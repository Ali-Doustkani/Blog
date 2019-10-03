import React from 'react'
import PropTypes from 'prop-types'
import image from './loader.svg'

const Loader = props => (
   <div data-testid="loader-container" className="page-message">
      <div>
         <img src={image} style={{ width: 50 }} />
         <div>{props.text ? props.text : 'Loading...'}</div>
      </div>
   </div>
)

Loader.propTypes = {
   text: PropTypes.string
}

export { Loader }

import React from 'react'
import image from './loader.svg'

const Loader = props => (
   <div data-testid="loader-component" className="page-message">
      <div>
         <img src={image} style={{ width: 50 }} />
         <div>{props.text ? props.text : 'Loading...'}</div>
      </div>
   </div>
)

export { Loader }

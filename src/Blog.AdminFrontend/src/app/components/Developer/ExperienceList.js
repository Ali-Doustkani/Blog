import React from 'react'
import PropTypes from 'prop-types'
import Experience from './Experience'
import { Button } from 'Controls'

const ExperienceList = props => {
   return (
      <div className="container">
         {props.experiences.map(e => (
            <Experience key={e.id} {...e} {...props} />
         ))}
         <Button onClick={props.onAdd}>Add Work Experience</Button>
      </div>
   )
}

ExperienceList.propTypes = {
   experiences: PropTypes.arrayOf(PropTypes.object).isRequired,
   onDelete: PropTypes.func,
   onChange: PropTypes.func,
   onAdd: PropTypes.func
}

export default ExperienceList

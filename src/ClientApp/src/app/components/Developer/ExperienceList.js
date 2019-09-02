import React from 'react'
import PropTypes from 'prop-types'
import Experience from './Experience'
import { Button } from 'Controls'

const ExperienceList = props => {
   return (
      <div className="container">
         {props.experiences.map((e, index) => (
            <Experience
               key={e.id}
               {...e}
               {...props.errors[index]}
               onChange={props.onChange}
               onDelete={props.onDelete}
            />
         ))}
         <Button data-cy="add-experience-button" onClick={props.onAdd}>
            Add Work Experience
         </Button>
      </div>
   )
}

ExperienceList.propTypes = {
   experiences: PropTypes.arrayOf(PropTypes.object).isRequired,
   errors: PropTypes.arrayOf(PropTypes.object),
   onDelete: PropTypes.func,
   onChange: PropTypes.func,
   onAdd: PropTypes.func
}

export default ExperienceList

import React from 'react'
import PropTypes from 'prop-types'
import Education from './Education'
import { Button } from 'Controls'

const EducationList = props => (
   <div className="container">
      {props.educations.map((edu, index) => (
         <Education
            key={edu.id}
            {...edu}
            {...props.errors[index]}
            onDelete={props.onDelete}
            onChange={props.onChange}
         />
      ))}
      <Button onClick={props.onAdd}>Add Education</Button>
   </div>
)

EducationList.propTypes = {
   educations: PropTypes.arrayOf(PropTypes.object).isRequired,
   errors: PropTypes.arrayOf(PropTypes.object),
   onDelete: PropTypes.func,
   onAdd: PropTypes.func,
   onChange: PropTypes.func
}

export default EducationList

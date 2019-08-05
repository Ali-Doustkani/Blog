import React from 'react'
import PropTypes from 'prop-types'
import { Richtext, ErrorList, Input } from '../Components'
import Button from '../Components/Richtext/Button'

const Experience = props => {
   const inputChange = e =>
      props.onChange({
         id: props.id,
         [e.target.name]: e.target.value
      })

   return (
      <div data-testid="experience-component" className="work-experience-group">
         <div className="text-group toolbar">
            <label>Work Experience:</label>
            <Input
               name="company"
               autoFocus
               errors={props.companyErrors}
               value={props.company}
               onChange={inputChange}
            />
            <Input
               name="position"
               errors={props.positionErrors}
               value={props.position}
               onChange={inputChange}
            />
            <Input
               name="startDate"
               type="date"
               errors={props.startDateErrors}
               value={props.startDate}
               onChange={inputChange}
               className="hide-arrow"
            />
            <Input
               name="endDate"
               type="date"
               errors={props.endDateErrors}
               value={props.endDate}
               onChange={inputChange}
               className="hide-arrow"
            />
            <div className="separator" />
            <Button
               content="trash-alt"
               data-testid="deleteExperience-button"
               onClick={() => props.onDelete(props.id)}
            />
         </div>
         <Richtext
            name="content"
            data-testid="content-richtext"
            innerHtml={props.content}
            onChange={inputChange}
            errors={props.contentErrors}
         />
         <ErrorList
            errors={[
               props.companyErrors,
               props.positionErrors,
               props.startDateErrors,
               props.endDateErrors,
               props.contentErrors
            ]}
         />
      </div>
   )
}

Experience.propTypes = {
   id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
   company: PropTypes.string,
   companyErrors: PropTypes.arrayOf(PropTypes.string),
   position: PropTypes.string,
   positionErrors: PropTypes.arrayOf(PropTypes.string),
   startDate: PropTypes.string,
   startDateErrors: PropTypes.arrayOf(PropTypes.string),
   endDate: PropTypes.string,
   endDateErrors: PropTypes.arrayOf(PropTypes.string),
   content: PropTypes.string,
   contentErrors: PropTypes.arrayOf(PropTypes.string),
   onChange: PropTypes.func,
   onDelete: PropTypes.func
}

export default Experience

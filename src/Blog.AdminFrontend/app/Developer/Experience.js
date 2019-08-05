import React from 'react'
import PropTypes from 'prop-types'
import { Richtext, ErrorList, Button } from '../Components'

function concat() {
   let result = []
   ;[].forEach.call(arguments, arr => {
      if (arr && Array.isArray(arr)) {
         result = result.concat(arr)
      }
   })
   return result
}

const Experience = props => {
   const inputChange = e =>
      props.onChange({
         id: props.id,
         [e.target.name]: e.target.value
      })
   const errors = concat(
      props.companyErrors,
      props.positionErrors,
      props.startDateErrors,
      props.endDateErrors,
      props.contentErrors
   )

   return (
      <div data-testid="experience-component" className="work-experience-group">
         <div className="text-group toolbar">
            <label>Work Experience:</label>
            <input
               name="company"
               data-testid="company-input"
               placeholder="Company"
               autoFocus
               className={props.companyErrors && props.companyErrors.length ? 'incorrect' : null}
               defaultValue={props.company}
               onChange={inputChange}
            />
            <input
               name="position"
               data-testid="position-input"
               placeholder="Position"
               className={props.positionErrors && props.positionErrors.length ? 'incorrect' : null}
               defaultValue={props.position}
               onChange={inputChange}
            />
            <input
               name="startDate"
               data-testid="startDate-input"
               type="date"
               className={
                  props.startDateErrors && props.startDateErrors.length
                     ? 'hide-arrow incorrect'
                     : 'hide-arrow'
               }
               defaultValue={props.startDate}
               onChange={inputChange}
            />
            <input
               name="endDate"
               data-testid="endDate-input"
               type="date"
               className={
                  props.endDateErrors && props.endDateErrors.length
                     ? 'hide-arrow incorrect'
                     : 'hide-arrow'
               }
               defaultValue={props.endDate}
               onChange={inputChange}
            />
            <div className="separator" />
            <Button data-testid="deleteExperience-button" onClick={() => props.onDelete(props.id)}>
               <i className="fas fa-trash-alt" />
            </Button>
         </div>
         <Richtext
            name="content"
            data-testid="content-richtext"
            innerHtml={props.content}
            onChange={inputChange}
            errors={props.contentErrors}
         />
         <ErrorList errors={errors} />
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

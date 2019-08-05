import React from 'react'
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
               autoFocus
               placeholder="Company"
               className={props.companyErrors ? 'incorrect' : null}
               defaultValue={props.company}
               onChange={inputChange}
            />
            <input
               name="position"
               data-testid="position-input"
               placeholder="Position"
               className={props.positionErrors ? 'incorrect' : null}
               defaultValue={props.position}
               onChange={inputChange}
            />
            <input
               name="startDate"
               data-testid="startDate-input"
               type="date"
               className={props.startDateErrors ? 'hide-arrow incorrect' : 'hide-arrow'}
               defaultValue={props.startDate}
               onChange={inputChange}
            />
            <input
               name="endDate"
               data-testid="endDate-input"
               type="date"
               className={props.endDateErrors ? 'hide-arrow incorrect' : 'hide-arrow'}
               defaultValue={props.endDate}
               onChange={inputChange}
            />
            <div className="separator" />
            <Button
               data-testid="deleteExperience-button"
               onClick={() => props.deleteClicked(props.id)}
            >
               <i class="fas fa-trash-alt" />
            </Button>
         </div>
         <Richtext
            name="content"
            data-testid="content-richtext"
            innerHtml={props.content}
            onChange={inputChange}
            error={props.contentErrors}
         />
         <ErrorList errors={errors} />
      </div>
   )
}

export default Experience

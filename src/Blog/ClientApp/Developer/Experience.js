import React from 'react'
import { Richtext } from '../Components'

const Experience = props => {
   const inputChange = e =>
      props.onChange({
         id: props.id,
         [e.target.name]: e.target.value
      })

   return (
      <div data-testid="experience-component" className="work-experience-group">
         <div className="text-group single-row ">
            <label>Work Experience</label>
            <input
               name="company"
               data-testid="company-input"
               autoFocus
               placeholder="Company"
               className={props.companyError ? 'incorrect' : null}
               defaultValue={props.company}
               onChange={inputChange}
            />
            <input
               name="position"
               data-testid="position-input"
               placeholder="Position"
               className={props.positionError ? 'incorrect' : null}
               defaultValue={props.position}
               onChange={inputChange}
            />
            <input
               name="startDate"
               data-testid="startDate-input"
               type="date"
               className="hide-arrow"
               defaultValue={props.startDate}
               onChange={inputChange}
            />
            <input
               name="endDate"
               data-testid="endDate-input"
               type="date"
               className="hide-arrow"
               defaultValue={props.endDate}
               onChange={inputChange}
            />
         </div>
         <div className="text-group richtext-group">
            <Richtext
               name="content"
               data-testid="content-richtext"
               innerHtml={props.content}
               onChange={inputChange}
            />
         </div>
         <button
            data-testid="deleteExperience-button"
            onClick={() => props.deleteClicked(props.id)}
         >
            Delete
         </button>
      </div>
   )
}

export default Experience

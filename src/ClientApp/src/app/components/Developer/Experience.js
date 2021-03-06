import React from 'react'
import PropTypes from 'prop-types'
import { Richtext, ErrorList, Input, ToolbarButton } from 'Controls'

const Experience = props => (
   <div data-testid="experience-container" className="sub-form-group">
      <div className="text-group toolbar">
         <label>Work Experience:</label>
         <Input name="company" {...props} autoFocus />
         <Input name="position" {...props} />
         <Input name="startDate" {...props} type="date" className="hide-arrow" />
         <Input name="endDate" {...props} type="date" className="hide-arrow" />
         <div className="separator" />
         <ToolbarButton
            data-testid="delete-button"
            content="trash-alt"
            onClick={() => props.onDelete(props.id)}
         />
      </div>
      <Richtext name="content" showErrors={false} {...props} />
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

import React from 'react'
import PropTypes from 'prop-types'
import { Richtext, ErrorList, Input, ToolbarButton } from 'Controls'

const Experience = props => (
   <div data-testid="experience-component" className="work-experience-group">
      <div className="text-group toolbar">
         <label>Work Experience:</label>
         <Input name="company" {...props} autoFocus />
         <Input name="position" {...props} />
         <Input name="startDate" {...props} type="date" className="hide-arrow" />
         <Input name="endDate" {...props} type="date" className="hide-arrow" />
         <div className="separator" />
         <ToolbarButton content="trash-alt" onClick={() => props.onDelete(props.id)} />
      </div>
      <Richtext name="content" {...props} />
      <ErrorList {...props} />
   </div>
)

Experience.propTypes = {
   id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
   company: PropTypes.string,
   companyErrors: PropTypes.arrayOf(PropTypes.object),
   position: PropTypes.string,
   positionErrors: PropTypes.arrayOf(PropTypes.object),
   startDate: PropTypes.string,
   startDateErrors: PropTypes.arrayOf(PropTypes.object),
   endDate: PropTypes.string,
   endDateErrors: PropTypes.arrayOf(PropTypes.object),
   content: PropTypes.string,
   contentErrors: PropTypes.arrayOf(PropTypes.object),
   onChange: PropTypes.func,
   onDelete: PropTypes.func
}

export default Experience

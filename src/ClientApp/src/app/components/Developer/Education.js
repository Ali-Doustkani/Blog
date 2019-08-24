import React from 'react'
import PropTypes from 'prop-types'
import { Input, ToolbarButton, ErrorList } from 'Controls'

const Education = props => (
   <div className="sub-form-group">
      <div className="text-group toolbar">
         <label>Education:</label>
         <Input name="degree" autoFocus {...props} />
         <Input name="university" {...props} />
         <Input name="startDate" type="date" className="hide-arrow" {...props} />
         <Input name="endDate" type="date" className="hide-arrow" {...props} />
         <div className="separator" />
         <ToolbarButton content="trash-alt" onClick={() => props.onDelete(props.id)} />
         <ErrorList
            errors={[
               props.degreeErrors,
               props.universityErrors,
               props.startDateErrors,
               props.endDateErrors
            ]}
         />
      </div>
   </div>
)

Education.propTypes = {
   id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
   degree: PropTypes.string,
   degreeErrors: PropTypes.arrayOf(PropTypes.string),
   university: PropTypes.string,
   universityErrors: PropTypes.arrayOf(PropTypes.string),
   startDate: PropTypes.string,
   startDateErrors: PropTypes.arrayOf(PropTypes.string),
   endDate: PropTypes.string,
   endDateErrors: PropTypes.arrayOf(PropTypes.string),
   onDelete: PropTypes.func
}

export default Education
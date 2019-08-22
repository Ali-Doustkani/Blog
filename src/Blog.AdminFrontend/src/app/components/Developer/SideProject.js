import React from 'react'
import PropTypes from 'prop-types'
import { Input, Richtext, ToolbarButton, ErrorList } from 'Controls'

const SideProject = props => (
   <div className="sub-form-group">
      <div className="text-group toolbar">
         <label>Side Project:</label>
         <Input name="title" autoFocus {...props} />
         <div className="separator" />
         <ToolbarButton content="trash-alt" onClick={() => props.onDelete(props.id)} />
      </div>
      <Richtext name="content" showErrors={false} {...props} />
      <ErrorList errors={[props.titleErrors, props.contentErrors]} />
   </div>
)

SideProject.propTypes = {
   id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
   title: PropTypes.string,
   titleErrors: PropTypes.arrayOf(PropTypes.object),
   content: PropTypes.string,
   contentErrors: PropTypes.arrayOf(PropTypes.object),
   onDelete: PropTypes.func
}

export default SideProject

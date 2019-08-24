import React from 'react'
import PropTypes from 'prop-types'
import { Button } from 'Controls'
import SideProject from './SideProject'

const SideProjectList = props => (
   <div className="container">
      {props.sideProjects.map((e, index) => (
         <SideProject
            key={e.id}
            {...e}
            {...props.errors[index]}
            onChange={props.onChange}
            onDelete={props.onDelete}
         />
      ))}
      <Button onClick={props.onAdd}>Add Side Project</Button>
   </div>
)

SideProjectList.propTypes = {
   sideProjects: PropTypes.arrayOf(PropTypes.object).isRequired,
   errors: PropTypes.arrayOf(PropTypes.object),
   onAdd: PropTypes.func,
   onChange: PropTypes.func,
   onDelete: PropTypes.func
}

export default SideProjectList

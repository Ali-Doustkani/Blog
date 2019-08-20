import React from 'react'
import PropTypes from 'prop-types'
import { Button } from 'Controls'
import SideProject from './SideProject'

const SideProjectList = props => (
   <div className="container">
      {props.sideProjects.map(e => (
         <SideProject key={e.id} {...e} onChange={props.onChange} onDelete={props.onDelete} />
      ))}
      <Button onClick={props.onAdd}>Add Side Project</Button>
   </div>
)

SideProjectList.propTypes = {
   sideProjects: PropTypes.arrayOf(PropTypes.object).isRequired,
   onAdd: PropTypes.func,
   onChange: PropTypes.func,
   onDelete: PropTypes.func
}

export default SideProjectList

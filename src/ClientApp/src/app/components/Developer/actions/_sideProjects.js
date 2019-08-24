import uuid from 'uuid/v1'
import { update } from '../../../utils/fn'

const newSideProject = () => ({
   id: uuid(),
   title: '',
   content: ''
})

const addSideProject = state => ({
   ...state,
   sideProjects: [...state.sideProjects, newSideProject()]
})

const deleteSideProject = (state, action) => ({
   ...state,
   sideProjects: state.sideProjects.filter(x => x.id !== action.id)
})

const updateSideProject = (state, action) => ({
   ...state,
   sideProjects: state.sideProjects.map(update(action.change))
})

export { addSideProject, deleteSideProject, updateSideProject }

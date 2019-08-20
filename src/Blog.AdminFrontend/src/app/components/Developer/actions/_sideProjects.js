import uuid from 'uuid/v1'

const initSideProjectErrors = sideProject => ({
   ...sideProject,
   titleErrors: [],
   contentErrors: []
})

const newSideProject = () =>
   initSideProjectErrors({
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

const update = change => sideProject =>
   change.id === sideProject.id ? { ...sideProject, ...change } : sideProject

export { addSideProject, deleteSideProject, updateSideProject, initSideProjectErrors }

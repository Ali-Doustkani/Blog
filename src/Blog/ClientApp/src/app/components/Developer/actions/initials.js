const STATUS = {
   LOADING: 1,
   SAVING: 2,
   IDLE: 3,
   STOPPED: 4
}

const initialState = {
   status: STATUS.LOADING,
   errorMessage: '',
   summary: '',
   summaryErrors: [],
   skills: '',
   skillsErrors: [],
   experiences: [],
   experiencesErrors: [],
   sideProjects: [],
   sideProjectErrors: []
}

export { initialState, STATUS }

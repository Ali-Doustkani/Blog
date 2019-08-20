import React, { useEffect } from 'react'
import PropTypes from 'prop-types'
import { Loader, Message, Button, Richtext, Textarea, ErrorList, ask } from 'Controls'
import ExperienceList from './ExperienceList'
import SideProjectList from './SideProjectList'
import { getDeveloper, saveDeveloper, anyError, validate } from './services'
import DisabledContext from 'DisabledContext'
import { useActions, STATUS } from './actions'

const Developer = ({ notify }) => {
   const [state, actions] = useActions()

   useEffect(() => {
      fetchDeveloper()
   }, [])

   async function fetchDeveloper() {
      actions.toLoading()
      const result = await getDeveloper()
      actions.load(result)
   }

   async function save() {
      actions.removeServerErrors()
      validate(state)
      if (anyError(state)) {
         notify('Resolve the problems first', 'error')
         return
      }

      actions.toSaving()
      const result = await saveDeveloper(state)
      if (result.status === 'ok') {
         actions.updateIds(result.data)
         notify('The developer saved successfully!', 'success')
      } else if (result.status === 'error') {
         actions.showErrors(result.data)
         notify('Could not save the developer information. Checkout the errors.', 'error')
      }
   }

   if (state.status === STATUS.LOADING) {
      return <Loader text="Loading developer..." />
   } else if (state.status === STATUS.STOPPED) {
      return (
         <Message message={state.errorMessage} onTryAgain={async () => await fetchDeveloper()} />
      )
   }

   return (
      <DisabledContext.Provider value={state.status === STATUS.SAVING}>
         {state.status === STATUS.SAVING ? <Loader text="Saving developer..." /> : null}
         <div className="form about-form">
            <h1>Write about yourself</h1>
            <ErrorList errors={state.experiencesErrors} />
            <Richtext
               label="Summary"
               name="summary"
               autofocus
               {...state}
               onChange={actions.updateDeveloper}
            />
            <Textarea label="Skills" name="skills" {...state} onChange={actions.updateDeveloper} />
            <ExperienceList
               experiences={state.experiences}
               onAdd={actions.addExperience}
               onChange={actions.updateExperience}
               onDelete={ask(actions.deleteExperience)}
            />
            <SideProjectList
               sideProjects={state.sideProjects}
               onAdd={actions.addSideProject}
               onChange={actions.updateSideProject}
               onDelete={ask(actions.deleteSideProject)}
            />
            <Button onClick={save}>Save</Button>
         </div>
      </DisabledContext.Provider>
   )
}

Developer.propTypes = {
   notify: PropTypes.func.isRequired
}

export default Developer

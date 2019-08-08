import React, { useEffect } from 'react'
// import { useToasts } from 'react-toast-notifications'
import { Loader, Message, Button, Richtext, Textarea, ErrorList, ask } from 'Controls'
import ExperienceList from './ExperienceList'
import { getDeveloper, saveDeveloper } from './services'
import DisabledContext from 'DisabledContext'
import reducer from './reducer'
import { useActions } from './actions'

const Developer = () => {
   const [state, actions] = useActions(reducer)
   // const { addToast } = useToasts()

   useEffect(() => {
      fetchDeveloper()
   }, [])

   async function fetchDeveloper() {
      const result = await getDeveloper()
      actions.load(result)
   }

   async function save() {
      actions.toSaveMode()
      const result = await saveDeveloper(state)

      if (result.status === 'ok') {
         actions.updateIds(result.data)
         // addToast('The developer saved successfully!', {
         //    appearance: 'success',
         //    autoDismiss: true
         // })
      } else if (result.status === 'error') {
         actions.showErrors(result.data)
         // addToast('Could not save the developer information. Checkout the errors.', {
         //    appearance: 'error',
         //    autoDismiss: true
         // })
      }
   }

   if (state.isLoading && !state.disabled) {
      return <Loader text="Loading developer..." />
   }

   if (state.errorMessage) {
      return <Message message={state.errorMessage} onTryAgain={actions.restart} />
   }

   return (
      <DisabledContext.Provider value={state.disabled}>
         {state.isLoading ? <Loader text="Saving developer..." /> : null}
         <div className="form about-form">
            <h1>Write about yourself</h1>
            <ErrorList errors={state.experiencesErrors} />
            <Richtext
               label="Summary"
               name="summary"
               autofocus
               {...state}
               onChange={e => actions.updateDeveloper({ summary: e.summary })}
            />
            <Textarea
               label="Skills"
               name="skills"
               {...state}
               onChange={e => actions.updateDeveloper({ skills: e.skills })}
            />
            <ExperienceList
               experiences={state.experiences}
               onAdd={actions.addExperience}
               onChange={actions.updateExperience}
               onDelete={ask(actions.deleteExperience)}
            />
            <Button onClick={save}>Save</Button>
         </div>
      </DisabledContext.Provider>
   )
}

export default Developer

import React, { useReducer, useEffect } from 'react'
import { useToasts } from 'react-toast-notifications'
import { Loader, Message, Button, Richtext, Textarea, ask } from '../Components'
import ExperienceList from './ExperienceList'
import { getDeveloper, saveDeveloper } from './services'
import reducer from './reducer'
import { DisabledContext } from '../DisabledContext'

const initialState = {
   isLoading: true,
   disabled: false,
   errorMessage: '',
   summary: '',
   summaryErrors: [],
   skills: '',
   skillsErrors: [],
   experiences: []
}

const Developer = () => {
   const [state, dispatch] = useReducer(reducer, initialState)
   const { addToast } = useToasts()

   useEffect(() => {
      if (state.isLoading) {
         fetchDeveloper()
      }
   }, [state.isLoading])

   async function fetchDeveloper() {
      const result = await getDeveloper()
      dispatch({ type: 'LOAD', result })
   }

   async function save() {
      dispatch({ type: 'GOTO_SAVE_MODE' })
      const result = await saveDeveloper(state)
      if (result.status === 'ok') {
         dispatch({ type: 'UPDATE_IDS', data: result.data })
         addToast('The developer saved successfully!', {
            appearance: 'success',
            autoDismiss: true
         })
      } else if (result.status === 'error') {
         dispatch({ type: 'SHOW_ERRORS', data: result.data })
         addToast('Could not save the developer information. Checkout the errors.', {
            appearance: 'error',
            autoDismiss: true
         })
      }
   }

   if (state.isLoading && !state.disabled) {
      return <Loader text="Loading developer..." />
   }

   if (state.errorMessage) {
      return (
         <Message message={state.errorMessage} onTryAgain={() => dispatch({ type: 'RESTART' })} />
      )
   }

   return (
      <DisabledContext.Provider value={state.disabled}>
         {state.isLoading ? <Loader text="Saving developer..." /> : null}
         <div className="form about-form">
            <h1>Write about yourself</h1>
            <Richtext
               label="Summary"
               name="summary"
               autofocus
               {...state}
               onChange={e =>
                  dispatch({ type: 'UPDATE_DEVELOPER', change: { summary: e.summary } })
               }
            />
            <Textarea
               label="Skills"
               name="skills"
               {...state}
               onChange={e => dispatch({ type: 'UPDATE_DEVELOPER', change: { skills: e.skills } })}
            />
            <ExperienceList
               experiences={state.experiences}
               onAdd={() => dispatch({ type: 'NEW_EXPERIENCE' })}
               onChange={change => dispatch({ type: 'UPDATE_EXPERIENCE', change })}
               onDelete={ask(id => dispatch({ type: 'DELETE_EXPERIENCE', id }))}
            />
            <Button onClick={save}>Save</Button>
         </div>
      </DisabledContext.Provider>
   )
}

export default Developer

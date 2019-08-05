import React, { useReducer, useEffect } from 'react'
import { useToasts } from 'react-toast-notifications'
import { Loader, Message, Button, Richtext, Textarea, ask } from '../Components'
import Experience from './Experience'
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
      const { isLoading, ...developer } = state
      dispatch({ type: 'GOTO_SAVE_MODE' })
      const result = await saveDeveloper(developer)
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
      return <Loader text="loading developer..." />
   }

   if (state.errorMessage) {
      return (
         <Message message={state.errorMessage} onTryAgain={() => dispatch({ type: 'RESTART' })} />
      )
   }

   return (
      <DisabledContext.Provider value={state.disabled}>
         {state.isLoading ? <Loader /> : null}
         <div className="form about-form">
            <h1>Write about yourself</h1>
            <Richtext
               label="Summary"
               autofocus
               innerHtml={state.summary}
               errors={state.summaryErrors}
               onChange={e =>
                  dispatch({ type: 'UPDATE_DEVELOPER', change: { summary: e.target.value } })
               }
            />
            <Textarea
               label="Skills"
               name="skills"
               data-testid="skills-input"
               defaultValue={state.skills}
               errors={state.skillsErrors}
               onChange={e =>
                  dispatch({ type: 'UPDATE_DEVELOPER', change: { skills: e.target.value } })
               }
            />
            <div className="container">
               {state.experiences.map(e => (
                  <Experience
                     key={e.id}
                     {...e}
                     onDelete={ask(id => dispatch({ type: 'DELETE_EXPERIENCE', id }))}
                     onChange={change => dispatch({ type: 'UPDATE_EXPERIENCE', change })}
                  />
               ))}
               <Button
                  data-testid="addExperience-button"
                  onClick={() => dispatch({ type: 'NEW_EXPERIENCE' })}
               >
                  Add Work Experience
               </Button>
            </div>
            <Button data-testid="save-button" onClick={save}>
               Save
            </Button>
         </div>
      </DisabledContext.Provider>
   )
}

export default Developer

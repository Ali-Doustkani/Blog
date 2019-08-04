import React, { useReducer, useEffect } from 'react'
import { useToasts } from 'react-toast-notifications'
import { Loader, Message, Button, Richtext } from '../Components'
import Experience from './Experience'
import { getDeveloper, saveDeveloper } from './services'
import reducer from './reducer'

const initialState = {
   isLoading: true
}

function Developer() {
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
      developer.experiences = developer.experiences.map(e => ({
         id: e.id,
         company: e.company,
         position: e.position,
         startDate: e.startDate,
         endDate: e.endDate,
         content: e.content
      }))
      const result = await saveDeveloper(developer)
      if (result.status === 'ok') {
         dispatch({ type: 'UPDATE_IDS', result })
         addToast('The developer saved successfully!', {
            appearance: 'success',
            autoDismiss: true
         })
      } else if (result.status === 'error') {
         addToast('Could not save the developer information. Checkout the errors.', {
            appearance: 'error',
            autoDismiss: true
         })
      }
   }

   if (state.isLoading) {
      return <Loader text="loading developer..." />
   }

   if (state.errorMessage) {
      return (
         <Message message={state.errorMessage} onTryAgain={() => dispatch({ type: 'RESTART' })} />
      )
   }

   return (
      <div className="form about-form">
         <h1>Write about yourself</h1>
         <div className="text-group richtext-group">
            <label>Summary</label>
            <Richtext
               innerHtml={state.summary}
               error={state.summaryError}
               onChange={e =>
                  dispatch({ type: 'UPDATE_DEVELOPER', change: { summary: e.target.value } })
               }
            />
         </div>
         <div className="text-group">
            <label>Skills</label>
            <textarea
               name="skills"
               data-testid="skills-input"
               defaultValue={state.skills}
               className={state.skillsError ? 'incorrect' : null}
               onChange={e =>
                  dispatch({ type: 'UPDATE_DEVELOPER', change: { skills: e.target.value } })
               }
            />
         </div>
         <div className="container">
            {state.experiences.map(e => (
               <Experience
                  key={e.id}
                  {...e}
                  deleteClicked={id => dispatch({ type: 'DELETE_EXPERIENCE', id })}
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
   )
}

export default Developer

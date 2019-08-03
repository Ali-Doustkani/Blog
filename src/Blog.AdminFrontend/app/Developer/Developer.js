import React, { useReducer, useEffect } from 'react'
import { Loader, Message, Button, Richtext } from '../Components'
import Experience from './Experience'
import { getDeveloper, saveDeveloper } from './services'
import reducer from './reducer'

const initialState = {
   isLoading: true
}

function Developer() {
   const [state, dispatch] = useReducer(reducer, initialState)

   useEffect(() => {
      if (state.isLoading) {
         fetchDeveloper()
      }
   }, [state.isLoading])

   async function fetchDeveloper() {
      const data = await getDeveloper()
      dispatch({ type: 'LOAD', data })
   }

   async function save() {
      const result = await saveDeveloper(reducer(state, { type: 'EXTRACT_DATA' }))
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
            <Richtext innerHtml={state.summary} />
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

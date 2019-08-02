import React from 'react'
import { Loader, Button, Richtext } from '../Components'
import Experience from './Experience'
import { getDeveloper, saveDeveloper } from './services'
import reducer from './reducer'

class Developer extends React.Component {
   constructor(props) {
      super(props)
      this.addExperience = this.addExperience.bind(this)
      this.deleteExperience = this.deleteExperience.bind(this)
      this.updateExperience = this.updateExperience.bind(this)
      this.save = this.save.bind(this)
      this.state = {
         isLoading: true
      }
   }
   async componentDidMount() {
      const developer = await getDeveloper()
      this.setState(reducer(this.state, { type: 'LOAD', developer }))
   }
   addExperience() {
      this.setState(reducer(this.state, { type: 'NEW_EXPERIENCE' }))
   }
   deleteExperience(id) {
      this.setState(reducer(this.state, { type: 'DELETE_EXPERIENCE', id }))
   }
   updateExperience(e) {
      this.setState(reducer(this.state, { type: 'UPDATE_EXPERIENCE', experience: e }))
   }
   async save() {
      const result = await saveDeveloper(reducer(this.state, { type: 'EXTRACT_DATA' }))
   }
   render() {
      if (this.state.isLoading) {
         return <Loader text="loading developer..." />
      }
      return (
         <div className="form about-form">
            <h1>Write about yourself</h1>
            <div className="text-group richtext-group">
               <label>Summary</label>
               <Richtext innerHtml={this.state.summary} />
            </div>
            <div className="container">
               {this.state.experiences.map(e => (
                  <Experience
                     key={e.id}
                     {...e}
                     deleteClicked={this.deleteExperience}
                     onChange={this.updateExperience}
                  />
               ))}
               <Button data-testid="addExperience-button" onClick={this.addExperience}>
                  Add Work Experience
               </Button>
            </div>
            <Button data-testid="save-button" onClick={this.save}>
               Save
            </Button>
         </div>
      )
   }
}

export default Developer

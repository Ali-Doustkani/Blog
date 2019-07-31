import React from 'react'
import uuid from 'uuid/v1'
import { Loader, Button, Richtext } from '../Components'
import Experience from './Experience'
import { getDeveloper, saveDeveloper } from './services'

const assignFrom = source => target =>
   target.id === source.id ? Object.assign(target, source) : target

class Developer extends React.Component {
   constructor(props) {
      super(props)
      this.addExperience = this.addExperience.bind(this)
      this.deleteExperience = this.deleteExperience.bind(this)
      this.updateExperience = this.updateExperience.bind(this)
      this.save = this.save.bind(this)
      this.state = {
         isLoading: true,
         summary: '',
         experiences: []
      }
   }
   async componentDidMount() {
      const developer = await getDeveloper()
      this.setState({
         isLoading: false,
         ...developer
      })
   }
   addExperience() {
      this.state.experiences.push({
         id: uuid(),
         company: '',
         position: '',
         startDate: new Date(),
         endDate: null
      })
      this.setState({ experiences: this.state.experiences })
   }
   deleteExperience(id) {
      this.setState(prevState => ({
         experiences: prevState.experiences.filter(x => x.id !== id)
      }))
   }
   updateExperience(e) {
      this.setState(prevState => ({
         experiences: prevState.experiences.map(assignFrom(e))
      }))
   }
   async save() {
      const result = await saveDeveloper({
         summary: this.state.summary,
         experiences: this.state.experiences
      })
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

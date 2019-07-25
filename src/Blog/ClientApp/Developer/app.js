import React from 'react'
import ReactDOM from 'react-dom'
import { Loader, Button, Richtext } from '../Components'
import Experience from './Experience'

function observer() {
   const observers = []
   const subscribe = callback => observers.push(callback)
   const unsubscribe = callback => observers.splice(observers.indexOf(callback), 1)
   const callEach = fn => observers.forEach(ob => fn(ob))
   return { subscribe, unsubscribe, callEach }
}

class Developer extends React.Component {
   constructor(props) {
      super(props)
      this.deleteExperience = this.deleteExperience.bind(this)
      this.addExperience = this.addExperience.bind(this)
      this.summary = React.createRef()
      this.observer = observer()
      this.save = this.save.bind(this)
      this.state = {
         isLoading: true,
         summary: '',
         experiences: []
      }
   }
   componentDidMount() {
      fetch('/api/developer')
         .then(res => res.json())
         .then(data =>
            this.setState({
               isLoading: false,
               summary: data.summary,
               experiences: data.experiences
            })
         )
   }
   addExperience() {
      const id = Math.floor(Math.random() * 10)
      this.state.experiences.push({ id, company: '', position: '', from: new Date(), to: null })
      this.setState({ experiences: this.state.experiences })
   }
   deleteExperience(id) {
      var toDelete = this.state.experiences.indexOf(this.state.experiences.find(x => x.id === id))
      this.state.experiences.splice(toDelete, 1)
      this.setState({ experiences: this.state.experiences })
   }
   save() {
      this.observer.callEach(fn => {
         console.log(fn())
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
               <Richtext innerHtml={this.state.summary} ref={this.summary} />
            </div>
            <div className="container">
               {this.state.experiences.map(e => (
                  <Experience
                     key={e.id}
                     {...e}
                     deleteClicked={this.deleteExperience}
                     observer={this.observer}
                  />
               ))}
               <Button onClick={this.addExperience}>Add Work Experience</Button>
            </div>
            <Button onClick={this.save}>Save</Button>
         </div>
      )
   }
}

ReactDOM.render(<Developer />, document.getElementById('root'))

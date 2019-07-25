import React from 'react'
import { Richtext } from '../Components'

export default class extends React.Component {
   constructor(props) {
      super(props)
      this.state = {
         contentError: ''
      }
      this.collectData = this.collectData.bind(this)
      props.observer.subscribe(this.collectData)
      this.company = React.createRef()
      this.position = React.createRef()
      this.startDate = React.createRef()
      this.endDate = React.createRef()
      this.content = React.createRef()
   }
   componentWillUnmount() {
      this.props.observer.unsubscribe(this.collectData)
   }
   collectData() {
      if (!this.content.current.innerText) {
         this.setState({ contentError: 'inset data' })
      }
      return {
         company: this.company.current.value,
         position: this.position.current.value,
         startDate: this.startDate.current.value,
         endDate: this.endDate.current.value,
         content: this.content.current.innerHTML
      }
   }
   render() {
      return (
         <div className="work-experience-group">
            <div className="text-group single-row ">
               <label>Work Experience</label>
               <input
                  autoFocus
                  placeholder="Company"
                  defaultValue={this.props.company}
                  ref={this.company}
               />
               <input
                  placeholder="Position"
                  defaultValue={this.props.position}
                  ref={this.position}
               />
               <input
                  type="date"
                  className="hide-arrow"
                  defaultValue={this.props.startDate}
                  ref={this.startDate}
               />
               <input
                  type="date"
                  className="hide-arrow"
                  defaultValue={this.props.endDate}
                  ref={this.endDate}
               />
            </div>
            <div className="text-group richtext-group">
               <Richtext
                  innerHtml={this.props.content}
                  ref={this.content}
                  error={this.state.contentError}
               />
            </div>
            <button onClick={() => this.props.deleteClicked(this.props.id)}>Delete</button>
         </div>
      )
   }
}

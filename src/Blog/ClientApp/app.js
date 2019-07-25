import React from 'react'
import ReactDOM from 'react-dom'
import Richtext from './Components/Richtext'

class Developer extends React.Component {
  constructor(props) {
    super(props)
  }
  render() {
    return (
      <div className="form about-form">
        <h1>Write about yourself</h1>
        <div className="error" />
        <div class="text-group richtext-group">
          <label>Summary</label>
          <Richtext />
          <span className="error" />
        </div>
        <button className="form-button">Save</button>
      </div>
    )
  }
}

ReactDOM.render(<Developer />, document.getElementById('root'))

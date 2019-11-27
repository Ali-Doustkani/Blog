import React from 'react'
import ReactDOM from 'react-dom'
import App from './app/App'
import setFixtures from './__mocks__/fixtures/set-fixtures'

setFixtures()

ReactDOM.render(<App />, document.getElementById('root'))

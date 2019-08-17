const express = require('express')
const cors = require('cors')
const bodyParser = require('body-parser')
const app = express()
const developer = require('./developer')

app.use(cors())
app.use(bodyParser.json())

app.get('/api/developer', (req, res) =>
   setTimeout(() => {
      res.send(developer)
   }, 3000)
)

app.put('/api/developer', (req, res) => {
   setTimeout(() => {
      res.status(400).send({
         title: 'Validation',
         errors: [
            { error: 'required', path: ['summary'] },
            { error: 'required', path: ['skills'] },
            { error: 'required', path: ['experiences'] },
            { error: 'required', path: ['experiences', 0, 'company'] },
            { error: 'required', path: ['experiences', 0, 'position'] }
         ]
      })
   }, 6000)
})

app.listen(3000, () => {
   console.log('PUT ERROR => http://localhost:3000')
})

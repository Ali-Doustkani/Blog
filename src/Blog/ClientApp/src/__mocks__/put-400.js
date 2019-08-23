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
         summary: ['the summary field is required'],
         skills: ['the skills field is required'],
         experiences: ['the experiences is required'],
         'experiences[0].company': ['the company field is required'],
         'experiences[0].position': ['the position is required']
      })
   }, 6000)
})

app.listen(3000, () => {
   console.log('PUT ERROR => http://localhost:3000')
})

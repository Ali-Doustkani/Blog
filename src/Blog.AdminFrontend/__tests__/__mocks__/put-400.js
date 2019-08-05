const express = require('express')
const cors = require('cors')
const bodyParser = require('body-parser')
const app = express()
const developer = require('./developer')

app.use(cors())
app.use(bodyParser.json())

app.get('/api/developer', (req, res) => res.send(developer))

app.put('/api/developer', (req, res) => {
   res.status(400).send({
      summary: ['summary is required', 'summary is bad'],
      skills: ['skills is required', 'skills is bad'],
      experiences: [
         {
            company: ['company is required', 'company already exists']
         },
         {
            position: ['position is not valid']
         }
      ]
   })
})

app.listen(3000, () => {
   console.log('PUT ERROR => http://localhost:3000')
})

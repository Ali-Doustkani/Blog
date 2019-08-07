const express = require('express')
const cors = require('cors')
const bodyParser = require('body-parser')
const app = express()
const developer = require('./developer')

app.use(cors())
app.use(bodyParser.json())

app.get('/api/developer', (req, res) => res.send(developer))

app.put('/api/developer', (req, res) => {
   const ids = req.body.experiences.map((exp, i) => i + 100)
   res.send({
      experiences: ids
   })
})

app.listen(3000, () => {
   console.log('PUT OK => http://localhost:3000')
})

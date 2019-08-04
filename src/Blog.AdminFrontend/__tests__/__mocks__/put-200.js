const express = require('express')
const cors = require('cors')
const bodyParser = require('body-parser')
const app = express()

app.use(cors())
app.use(bodyParser.json())

app.get('/api/developer', (req, res) => {
   res.send(developer)
})

app.put('/api/developer', (req, res) => {
   const ids = req.body.experiences.map((exp, i) => i + 100)
   res.send({
      experiences: ids
   })
})

app.listen(3000, () => {
   console.log('MOCK API SERVER STARTED => http://localhost:3000')
})

const developer = {
   summary: '<p contenteditable="true">Summary</p><p contenteditable="true">Second</p>',
   skills: 'C#\nJS\n123',
   experiences: [
      {
         id: 1,
         company: 'Lodgify1',
         position: 'C# Developer',
         startDate: '2019-01-01',
         endDate: '2020-01-01',
         content: '<p contenteditable>worked as a back-end developer'
      }
   ]
}

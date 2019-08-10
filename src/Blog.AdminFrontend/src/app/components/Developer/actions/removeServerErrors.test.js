import removeServerErrors from './removeServerErrors'

it('removes all errors with server type', () => {
   const init = {
      summaryErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
      skillsErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
      experiencesErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
      experiences: [
         {
            companyErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
            positionErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
            startDateErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
            endDateErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }],
            contentErrors: [{ type: 1, message: 'err1' }, { type: 2, message: 'err2' }]
         }
      ]
   }

   const newState = removeServerErrors(init)

   const errors = [{ type: 1, message: 'err1' }]
   expect(newState.summaryErrors).toEqual(errors)
   expect(newState.skillsErrors).toEqual(errors)
   expect(newState.experiencesErrors).toEqual(errors)
   expect(newState.experiences[0].companyErrors).toEqual(errors)
   expect(newState.experiences[0].positionErrors).toEqual(errors)
   expect(newState.experiences[0].startDateErrors).toEqual(errors)
   expect(newState.experiences[0].endDateErrors).toEqual(errors)
   expect(newState.experiences[0].contentErrors).toEqual(errors)
})

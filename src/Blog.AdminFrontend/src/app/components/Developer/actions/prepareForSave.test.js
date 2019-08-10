import prepareForSave from './prepareForSave'

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

   const newState = prepareForSave(init)

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

it('does not set hasError for server errors', () => {
   const init = {
      hasError: true,
      summaryErrors: [{ type: 2, message: 'err2' }],
      skillsErrors: [{ type: 2, message: 'err2' }],
      experiencesErrors: [{ type: 2, message: 'err2' }],
      experiences: [
         {
            companyErrors: [{ type: 2, message: 'err2' }],
            positionErrors: [{ type: 2, message: 'err2' }],
            startDateErrors: [{ type: 2, message: 'err2' }],
            endDateErrors: [{ type: 2, message: 'err2' }],
            contentErrors: [{ type: 2, message: 'err2' }]
         }
      ]
   }
   const newState = prepareForSave(init)

   expect(newState.hasError).toBe(false)
})

test.each([
   { summaryErrors: [{ type: 1, message: 'err1' }] },
   { skillsErrors: [{ type: 1, message: 'err1' }] },
   { experiencesErrors: [{ type: 1, message: 'err1' }] }
])('sets hasError if developer has any error in %p', arg => {
   const init = {
      hasError: false,
      summaryErrors: [],
      skillsErrors: [],
      experiencesErrors: [],
      experiences: []
   }
   Object.assign(init, arg)

   const newState = prepareForSave(init)

   expect(newState.hasError).toBe(true)
})

test.each([
   { companyErrors: [{ type: 1, message: 'err1' }] },
   { positionErrors: [{ type: 1, message: 'err1' }] },
   { startDateErrors: [{ type: 1, message: 'err1' }] },
   { endDateErrors: [{ type: 1, message: 'err1' }] },
   { contentErrors: [{ type: 1, message: 'err1' }] }
])('sets hasError if experience has any error in %p', arg => {
   const init = {
      hasError: false,
      summaryErrors: [],
      skillsErrors: [],
      experiencesErrors: [],
      experiences: [
         {
            companyErrors: [],
            positionErrors: [],
            startDateErrors: [],
            endDateErrors: [],
            contentErrors: []
         }
      ]
   }
   Object.assign(init.experiences[0], arg)

   const newState = prepareForSave(init)

   expect(newState.hasError).toBe(true)
})

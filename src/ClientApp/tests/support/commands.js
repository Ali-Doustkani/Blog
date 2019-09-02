Cypress.Commands.add('testid', { prevSubject: false }, selector => {
   return cy.get(`*[data-cy='${selector}']`)
})

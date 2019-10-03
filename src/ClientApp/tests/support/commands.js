Cypress.Commands.add('testid', { prevSubject: false }, selector => {
   return cy.get(`*[data-testid='${selector}']`)
})

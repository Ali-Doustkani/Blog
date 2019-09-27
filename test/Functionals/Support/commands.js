function any(selector) {
  return Boolean(Cypress.$(`*[data-cy=${selector}]`).length);
}

Cypress.Commands.add("testid", { prevSubject: false }, selector => {
  return cy.get(`*[data-cy='${selector}']`);
});

Cypress.Commands.add("login", { prevSubject: true }, () => {
  cy.testid("username-input").type("test");
  cy.testid("password-input").type("123");
  return cy.testid("login-button").click();
});

Cypress.Commands.add("logout", { prevSubject: false }, selector => {
  cy.testid("logout-button").click();
});

Cypress.Commands.add("clickAll", { prevSubject: false }, selector => {
  if (any(selector)) {
    cy.testid(selector).then(x => {
      for (let i = 0; i < x.length; i++) {
        cy.testid(selector)
          .eq(0)
          .click();
      }
    });
  }
});

Cypress.Commands.add("clearRichtext", { prevSubject: true }, prev => {
  prev[0].innerHTML = '<p contenteditable="true"></p>';
  return cy.wrap(prev[0].firstChild);
});

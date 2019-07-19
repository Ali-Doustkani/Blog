function any(selector) {
  return Boolean(Cypress.$(`*[data-cy=${selector}]`).length);
}

Cypress.Commands.add("el", { prevSubject: false }, selector => {
  return cy.get(`*[data-cy='${selector}']`);
});

Cypress.Commands.add("login", { prevSubject: true }, () => {
  cy.el("username-input").type("test");
  cy.el("password-input").type("123");
  return cy.el("login-button").click();
});

Cypress.Commands.add("clickAll", { prevSubject: false }, selector => {
  if (any(selector)) {
    cy.el(selector).then(x => {
      for (let i = 0; i < x.length; i++) {
        cy.el(selector)
          .eq(0)
          .click();
      }
    });
  }
});

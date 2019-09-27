describe("admin activities", () => {
  it("shows admin panel when logged in", () => {
    cy.visit("/admin").login();
    cy.testid("logout-button").should("be.visible");

    cy.testid("logout-button").click();
    cy.testid("logout-button").should("not.be.visible");
  });

  it("shows edit button when logged in", () => {
    cy.visit("/admin").login();
    cy.visit("/admin/Post");
    cy.testid("title-input").type("test");
    cy.testid("tags-input").type("test");
    cy.testid("summary-input").type("test");
    cy.testid("content-input")
      .find("p")
      .type("test");
    cy.testid("publish-checkbox").check();
    cy.testid("save-button").click();
    cy.testid("edit-button").should("be.visible");

    cy.testid("logout-button").click();
    cy.visit("/en/test");
    cy.testid("edit-button").should("not.be.visible");
  });
});

describe("admin activities", () => {
  it("shows admin panel when logged in", () => {
    cy.visit("/admin").login();
    cy.el("logout-button").should("be.visible");

    cy.el("logout-button").click();
    cy.el("logout-button").should("not.be.visible");
  });

  it("shows edit button when logged in", () => {
    cy.visit("/admin").login();
    cy.visit("/admin/Post");
    cy.el("title-input").type("test");
    cy.el("tags-input").type("test");
    cy.el("summary-input").type("test");
    cy.el("content-input")
      .find("p")
      .type("test");
    cy.el("publish-checkbox").check();
    cy.el("save-button").click();
    cy.el("edit-button").should("be.visible");

    cy.el("logout-button").click();
    cy.visit("/en/test");
    cy.el("edit-button").should("not.be.visible");
  });
});

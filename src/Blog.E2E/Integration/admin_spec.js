describe("admin activities", () => {
  it("show admin panel when logged in", () => {
    cy.visit("/admin").login();
    cy.el("logout-button").should("be.visible");

    cy.el("logout-button").click();
    cy.el("logout-button").should("not.be.visible");
  });
});

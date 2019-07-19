beforeEach(() => {
  cy.visit("/admin")
    .login()
    .then(() => cy.clickAll("delete-button"));
});

it("add a new farsi post", () => {
  cy.visit("/admin/Post");
  cy.el("farsiLanguage-radio").click();
  cy.el("publishDate-input").type("2019-07-18");
  cy.el("title-input").type("آموزش Javascript");
  cy.el("englishUrl-input").type("learn-js");
  cy.el("tags-input").type("js, cypress");
  cy.el("summary-input").type("خلاصه");
  cy.el("content-input")
    .find("p")
    .type("متن");
  cy.el("save-button").click();
  cy.el("published-value").should("have.class", "fa-times");
  cy.el("title-value").should("have.text", "آموزش Javascript");
  cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
    .its("status")
    .should("eq", 404);
  cy.el("edit-button").click();
  cy.el("preview-button").then($a => {
    cy.visit($a[0].href);
    assertArticle();
  });
  cy.visit("/admin");
  cy.el("edit-button").click();
  cy.el("publish-checkbox").click();
  cy.el("save-button").click();
  assertArticle();
  cy.visit("/admin");
  cy.el("published-value").should("have.class", "fa-check");
  cy.el("edit-button").click();
  cy.el("publish-checkbox")
    .should("be.checked")
    .uncheck();
  cy.el("save-button").click();
  cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
    .its("status")
    .should("eq", 404);
  cy.el("published-value").should("have.class", "fa-times");
});

function assertArticle() {
  cy.el("article-container").should("have.class", "farsi");
  cy.el("title-value").should("have.text", "آموزش Javascript");
  cy.el("date-value").should("have.text", "پنج شنبه، 27 تیر 1398");
  cy.el("article-container")
    .find("p")
    .should("have.text", "متن");
}

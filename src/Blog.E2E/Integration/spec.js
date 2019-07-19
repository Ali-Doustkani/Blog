beforeEach(() => {
  cy.visit("/admin");
  cy.get("input[name='username']").type("test");
  cy.get("input[name='password']").type("123");
  cy.get("form")
    .submit()
    .then(() => {
      if (Cypress.$("button:contains('Delete')").length) {
        cy.get("button:contains('Delete')").then(x => {
          for (let i = 0; i < x.length; i++) {
            cy.contains("Delete").click();
          }
        });
      }
    });
});

it("add a new farsi post", () => {
  cy.visit("/admin/Post");
  cy.contains("Farsi").click();
  cy.get("#PublishDate").type("2019-07-18");
  cy.get("#Title").type("آموزش Javascript");
  cy.get("#EnglishUrl").type("learn-js");
  cy.get("#Tags").type("js, cypress");
  cy.get("#Summary").type("خلاصه");
  cy.get("#editor>p").type("متن");
  cy.contains("Save").click();
  cy.get("tbody tr:first-of-type td")
    .eq(1)
    .find("i")
    .should("have.class", "fa-times");
  cy.get("tbody tr:first-of-type td")
    .eq(2)
    .should("have.text", "آموزش Javascript");
  cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
    .its("status")
    .should("eq", 404);
  cy.contains("Edit").click();
  cy.contains("Preview").then($a => {
    cy.visit($a[0].href);
    assertArticle();
  });
  cy.visit("/admin");
  cy.contains("Edit").click();
  cy.contains("Publish the Post").click();
  cy.contains("Save").click();
  assertArticle();
  cy.visit("/admin");
  cy.get("tbody tr:first-of-type td")
    .eq(1)
    .find("i")
    .should("have.class", "fa-check");
  cy.contains("Edit").click();
  cy.get("#Publish")
    .should("be.checked")
    .uncheck();
  cy.contains("Save").click();
  cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
    .its("status")
    .should("eq", 404);
  cy.get("tbody tr:first-of-type td")
    .eq(1)
    .find("i")
    .should("have.class", "fa-times");
});

function assertArticle() {
  cy.get("article")
    .should("have.css", "direction")
    .and("eq", "rtl");
  cy.get("article header h1").should("have.text", "آموزش Javascript");
  cy.get("article header h2").should("have.text", "پنج شنبه، 27 تیر 1398");
  cy.get("article p").should("have.text", "متن");
}

describe("adding posts", () => {
   before(() => {
      cy.visit("/admin")
         .login()
         .then(() => cy.clickAll("delete-button"));
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
      cy.logout();
   });

   it("show items in admin page", () => {
      cy.visit("/admin").login();
      cy.el("post-row").should("have.length", 1);
      cy.el("published-value").should("have.class", "fa-times");
      cy.el("title-value").should("have.text", "آموزش Javascript");
   });

   it("show items in home page", () => {
      cy.visit("/admin").login();
      cy.el("edit-button").click();
      cy.el("publish-checkbox").check();
      cy.el("save-button").click();

      cy.visit("/fa");
      cy.el("post-item").should("have.length", 1);
      cy.visit("/en");
      cy.el("post-item").should("have.length", 0);

      cy.visit("/admin")
      cy.el("edit-button").click();
      cy.el("englishLanguage-radio").check();
      cy.el("save-button").click();
      cy.visit("/en");
      cy.el("post-item").should("have.length", 1);
      cy.visit("/fa");
      cy.el("post-item").should("have.length", 0);
   });

   it("publish & unpublish a draft", () => {
      cy.visit("/admin").login();
      cy.el("edit-button").click();
      cy.el("farsiLanguage-radio").check();
      cy.el("save-button").click();

      cy.visit("/admin");
      cy.el("edit-button").click();
      cy.el("preview-button").then($a => {
         cy.visit($a[0].href);
         cy.el("article-container").should("have.class", "farsi");
         cy.el("title-value").should("have.text", "آموزش Javascript");
         cy.el("date-value").should("have.text", "پنج شنبه، 27 تیر 1398");
         cy.el("article-container")
            .find("p")
            .should("have.text", "متن");
      });
      cy.visit("/admin");
      cy.el("edit-button").click();
      cy.el("englishLanguage-radio").check();
      cy.el("publish-checkbox").check();
      cy.el("save-button").click();
      cy.el("article-container").should("not.have.class", "farsi");
      cy.el("title-value").should("have.text", "آموزش Javascript");
      cy.el("date-value").should("have.text", "Thursday, July 18, 2019");
      cy.el("article-container")
         .find("p")
         .should("have.text", "متن");

      cy.visit("/admin");
      cy.el("published-value").should("have.class", "fa-check");
      cy.el("edit-button").click();
      cy.el("publish-checkbox")
         .should("be.checked")
         .uncheck();
      cy.el("save-button").click();
      cy.el("published-value").should("have.class", "fa-times");
   });

   it("publish with different languages", () => {
      cy.visit("/admin").login();
  
      cy.request({ url: "/en/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 404);
      cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 404);
  
      cy.el("edit-button").click();
      cy.el("farsiLanguage-radio").check();
      cy.el("publish-checkbox").check();
      cy.el("save-button").click();
      cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 200);
      cy.request({ url: "/en/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 404);
  
      cy.visit("/admin");
      cy.el("edit-button").click();
      cy.el("englishLanguage-radio").check();
      cy.el("save-button").click();
      cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 404);
      cy.request({ url: "/en/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 200);
  
      cy.visit("/admin");
      cy.el("edit-button").click();
      cy.el("publish-checkbox").uncheck();
      cy.el("save-button").click();
      cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 404);
      cy.request({ url: "/en/learn-js", failOnStatusCode: false })
        .its("status")
        .should("eq", 404);
    });
});

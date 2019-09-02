describe("adding posts", () => {
  const days = [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday"
  ];
  const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December"
  ];
  function getDate() {
    const date = new Date();
    return `${days[date.getDay()]}, ${
      months[date.getMonth()]
    } ${date.getDate()}, ${date.getFullYear()}`;
  }

  before(() => {
    cy.visit("/admin")
      .login()
      .then(() => cy.clickAll("delete-button"));
    cy.visit("/admin/Post");
    cy.el("farsiLanguage-radio").click();
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

  it("shows items in admin page", () => {
    cy.visit("/admin").login();
    cy.el("post-row").should("have.length", 1);
    cy.el("published-value").should("have.class", "fa-times");
    cy.el("title-value").should("have.text", "آموزش Javascript");
  });

  it("shows items in home page", () => {
    cy.visit("/admin").login();
    cy.el("edit-button").click();
    cy.el("publish-checkbox").check();
    cy.el("save-button").click();

    cy.visit("/fa");
    cy.el("post-item").should("have.length", 1);
    cy.visit("/en");
    cy.el("post-item").should("have.length", 0);

    cy.visit("/admin");
    cy.el("edit-button").click();
    cy.el("englishLanguage-radio").check();
    cy.el("save-button").click();
    cy.visit("/en");
    cy.el("post-item").should("have.length", 1);
    cy.visit("/fa");
    cy.el("post-item").should("have.length", 0);
  });

  it("publishes & unpublishes a draft", () => {
    cy.visit("/admin").login();
    cy.el("edit-button").click();
    cy.el("farsiLanguage-radio").check();
    cy.el("save-button").click();

    cy.visit("/admin");
    cy.el("edit-button").click();
    cy.el("englishLanguage-radio").check();
    cy.el("publish-checkbox").check();
    cy.el("save-button").click();
    cy.el("article-container").should("not.have.class", "farsi");
    cy.el("title-value").should("have.text", "آموزش Javascript");
    cy.el("date-value").should("have.text", getDate());
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

  it("publishes with different languages", () => {
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

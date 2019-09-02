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
    cy.testid("farsiLanguage-radio").click();
    cy.testid("title-input").type("آموزش Javascript");
    cy.testid("englishUrl-input").type("learn-js");
    cy.testid("tags-input").type("js, cypress");
    cy.testid("summary-input").type("خلاصه");
    cy.testid("content-input")
      .find("p")
      .type("متن");
    cy.testid("save-button").click();
    cy.logout();
  });

  it("shows items in admin page", () => {
    cy.visit("/admin").login();
    cy.testid("post-row").should("have.length", 1);
    cy.testid("published-value").should("have.class", "fa-times");
    cy.testid("title-value").should("have.text", "آموزش Javascript");
  });

  it("shows items in home page", () => {
    cy.visit("/admin").login();
    cy.testid("edit-button").click();
    cy.testid("publish-checkbox").check();
    cy.testid("save-button").click();

    cy.visit("/fa");
    cy.testid("post-item").should("have.length", 1);
    cy.visit("/en");
    cy.testid("post-item").should("have.length", 0);

    cy.visit("/admin");
    cy.testid("edit-button").click();
    cy.testid("englishLanguage-radio").check();
    cy.testid("save-button").click();
    cy.visit("/en");
    cy.testid("post-item").should("have.length", 1);
    cy.visit("/fa");
    cy.testid("post-item").should("have.length", 0);
  });

  it("publishes & unpublishes a draft", () => {
    cy.visit("/admin").login();
    cy.testid("edit-button").click();
    cy.testid("farsiLanguage-radio").check();
    cy.testid("save-button").click();

    cy.visit("/admin");
    cy.testid("edit-button").click();
    cy.testid("englishLanguage-radio").check();
    cy.testid("publish-checkbox").check();
    cy.testid("save-button").click();
    cy.testid("article-container").should("not.have.class", "farsi");
    cy.testid("title-value").should("have.text", "آموزش Javascript");
    cy.testid("date-value").should("have.text", getDate());
    cy.testid("article-container")
      .find("p")
      .should("have.text", "متن");

    cy.visit("/admin");
    cy.testid("published-value").should("have.class", "fa-check");
    cy.testid("edit-button").click();
    cy.testid("publish-checkbox")
      .should("be.checked")
      .uncheck();
    cy.testid("save-button").click();
    cy.testid("published-value").should("have.class", "fa-times");
  });

  it("publishes with different languages", () => {
    cy.visit("/admin").login();

    cy.request({ url: "/en/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 404);
    cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 404);

    cy.testid("edit-button").click();
    cy.testid("farsiLanguage-radio").check();
    cy.testid("publish-checkbox").check();
    cy.testid("save-button").click();
    cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 200);
    cy.request({ url: "/en/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 404);

    cy.visit("/admin");
    cy.testid("edit-button").click();
    cy.testid("englishLanguage-radio").check();
    cy.testid("save-button").click();
    cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 404);
    cy.request({ url: "/en/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 200);

    cy.visit("/admin");
    cy.testid("edit-button").click();
    cy.testid("publish-checkbox").uncheck();
    cy.testid("save-button").click();
    cy.request({ url: "/fa/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 404);
    cy.request({ url: "/en/learn-js", failOnStatusCode: false })
      .its("status")
      .should("eq", 404);
  });
});

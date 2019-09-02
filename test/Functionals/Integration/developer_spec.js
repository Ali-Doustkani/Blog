it("updates the developer info", () => {
  cy.server();
  cy.route("PUT", "/api/developer").as("putDeveloper");
  cy.route("GET", "/api/developer").as("getDeveloper");

  cy.visit("/admin/developer");
  cy.wait("@getDeveloper");

  cy.testid("summary-richtext")
    .clearRichtext()
    .type("Hi. Im Ali.");

  cy.testid("skills-input")
    .clear()
    .type("C#, Javascript, React");

  cy.clickAll("delete-button");

  // add 2 experiences

  cy.testid("add-experience-button")
    .click()
    .click();

  cy.testid("experience-container")
    .eq(0)
    .within(() => {
      cy.testid("company-input").type("Parmis");
      cy.testid("position-input").type("C# Developer");
      cy.testid("startDate-input").type("2010-01-01");
      cy.testid("endDate-input").type("2011-01-01");
      cy.testid("content-richtext")
        .get("p")
        .type("coded back-end features");
    });

  cy.testid("experience-container")
    .eq(1)
    .within(() => {
      cy.testid("company-input").type("Lodgify");
      cy.testid("position-input").type("Fullstack Developer");
      cy.testid("startDate-input").type("2012-01-01");
      cy.testid("endDate-input").type("2013-01-01");
      cy.testid("content-richtext")
        .get("p")
        .type("coded both front-end and back-end");
    });

  // add 2 side projects

  cy.testid("add-project-button")
    .click()
    .click();

  cy.testid("project-container")
    .eq(0)
    .within(() => {
      cy.testid("title-input").type("Richtext");
      cy.testid("content-richtext")
        .get("p")
        .type("a HTML web richtext editor");
    });

  cy.testid("project-container")
    .eq(1)
    .within(() => {
      cy.testid("title-input").type("CactusGuru");
      cy.testid("content-richtext")
        .get("p")
        .type("Cacti Collection Manager");
    });

  // add 2 educations

  cy.testid("add-education-button")
    .click()
    .click();

  cy.testid("education-container")
    .eq(0)
    .within(() => {
      cy.testid("degree-input").type("BS of software engineering");
      cy.testid("university-input").type("C&S");
      cy.testid("startDate-input").type("2012-01-01");
      cy.testid("endDate-input").type("2013-01-01");
    });

  cy.testid("education-container")
    .eq(1)
    .within(() => {
      cy.testid("degree-input").type("MS of software engineering");
      cy.testid("university-input").type("C&S");
      cy.testid("startDate-input").type("2013-01-01");
      cy.testid("endDate-input").type("2014-01-01");
    });

  // save

  cy.testid("save-button").click();
  cy.wait("@putDeveloper");

  // check about page

  cy.visit("/about");

  cy.testid("summary-container").within(() => {
    cy.get("p").should("have.text", "Hi. Im Ali.");
  });

  cy.testid("experience-container").within(() => {
    cy.get("section>h2")
      .eq(0)
      .should("have.text", "C# Developer, Parmis January/2010 - January/2011");
    cy.get("p")
      .eq(0)
      .should("have.text", "coded back-end features");
    cy.get("section>h2")
      .eq(1)
      .should(
        "have.text",
        "Fullstack Developer, Lodgify January/2012 - January/2013"
      );
    cy.get("p")
      .eq(1)
      .should("have.text", "coded both front-end and back-end");
  });

  cy.testid("project-container").within(() => {
    cy.get("section>h2")
      .eq(0)
      .should("have.text", "Richtext");
    cy.get("p")
      .eq(0)
      .should("have.text", "a HTML web richtext editor");
    cy.get("section>h2")
      .eq(1)
      .should("have.text", "CactusGuru");
    cy.get("p")
      .eq(1)
      .should("have.text", "Cacti Collection Manager");
  });

  cy.testid("education-container").within(() => {
    cy.get("section h2")
      .eq(0)
      .should("have.text", "BS of software engineering 2012 - 2013");
    cy.get("p")
      .eq(0)
      .should("have.text", "C&S");
    cy.get("section h2")
      .eq(1)
      .should("have.text", "MS of software engineering 2013 - 2014");
    cy.get("p")
      .eq(1)
      .should("have.text", "C&S");
  });

  cy.testid("skills-container").within(() => {
    cy.get("p").should("have.text", "C#, Javascript, React");
  });
});

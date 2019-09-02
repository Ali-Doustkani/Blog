it("updates the developer info", () => {
  cy.server();
  cy.route("PUT", "/api/developer").as("putDeveloper");
  cy.route("GET", "/api/developer").as("getDeveloper");

  cy.visit("/admin/developer");
  cy.wait("@getDeveloper");

  cy.el("summary-richtext")
    .clearRichtext()
    .type("Hi. Im Ali.");

  cy.el("skills-input")
    .clear()
    .type("C#, Javascript, React");

  cy.clickAll("delete-button");

  // add 2 experiences

  cy.el("add-experience-button")
    .click()
    .click();

  cy.el("experience-container")
    .eq(0)
    .within(() => {
      cy.el("company-input").type("Parmis");
      cy.el("position-input").type("C# Developer");
      cy.el("startDate-input").type("2010-01-01");
      cy.el("endDate-input").type("2011-01-01");
      cy.el("content-richtext")
        .get("p")
        .type("coded back-end features");
    });

  cy.el("experience-container")
    .eq(1)
    .within(() => {
      cy.el("company-input").type("Lodgify");
      cy.el("position-input").type("Fullstack Developer");
      cy.el("startDate-input").type("2012-01-01");
      cy.el("endDate-input").type("2013-01-01");
      cy.el("content-richtext")
        .get("p")
        .type("coded both front-end and back-end");
    });

  // add 2 side projects

  cy.el("add-project-button")
    .click()
    .click();

  cy.el("project-container")
    .eq(0)
    .within(() => {
      cy.el("title-input").type("Richtext");
      cy.el("content-richtext")
        .get("p")
        .type("a HTML web richtext editor");
    });

  cy.el("project-container")
    .eq(1)
    .within(() => {
      cy.el("title-input").type("CactusGuru");
      cy.el("content-richtext")
        .get("p")
        .type("Cacti Collection Manager");
    });

  // add 2 educations

  cy.el("add-education-button")
    .click()
    .click();

  cy.el("education-container")
    .eq(0)
    .within(() => {
      cy.el("degree-input").type("BS of software engineering");
      cy.el("university-input").type("C&S");
      cy.el("startDate-input").type("2012-01-01");
      cy.el("endDate-input").type("2013-01-01");
    });

  cy.el("education-container")
    .eq(1)
    .within(() => {
      cy.el("degree-input").type("MS of software engineering");
      cy.el("university-input").type("C&S");
      cy.el("startDate-input").type("2013-01-01");
      cy.el("endDate-input").type("2014-01-01");
    });

  // save

  cy.el("save-button").click();
  cy.wait("@putDeveloper");

  // check about page

  cy.visit("/about");

  cy.el("summary-container").within(() => {
    cy.get("p").should("have.text", "Hi. Im Ali.");
  });

  cy.el("experience-container").within(() => {
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

  cy.el("project-container").within(() => {
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

  cy.el("education-container").within(() => {
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

  cy.el("skills-container").within(() => {
    cy.get("p").should("have.text", "C#, Javascript, React");
  });
});

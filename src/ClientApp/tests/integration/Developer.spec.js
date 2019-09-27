describe('fetching', () => {
   beforeEach(() => {
      cy.server()
      cy.route('GET', '/api/developer', 'fixture:developer.json')
      cy.visit('/')
   })

   it('fetches general info', () => {
      cy.testid('summary-richtext').should('have.text', 'Hi. Im Ali.')
      cy.testid('skills-input').should('have.value', 'C#\nJavascript\nReact')
   })

   it('fetches experiences', () => {
      cy.testid('experience-container').should('have.length', 2)

      cy.testid('experience-container')
         .eq(0)
         .within(() => {
            cy.testid('company-input').should('have.value', 'Lodgify')
            cy.testid('position-input').should('have.value', 'C# Developer')
            cy.testid('startDate-input').should('have.value', '2019-01-01')
            cy.testid('endDate-input').should('have.value', '2020-01-01')
            cy.testid('content-richtext').should('have.text', 'worked as a back-end developer')
         })

      cy.testid('experience-container')
         .eq(1)
         .within(() => {
            cy.testid('company-input').should('have.value', 'Parmis')
            cy.testid('position-input').should('have.value', 'Fullstack Developer')
            cy.testid('startDate-input').should('have.value', '2020-01-01')
            cy.testid('endDate-input').should('have.value', '2021-01-01')
            cy.testid('content-richtext').should(
               'have.text',
               'worked as both front-end & back-end developer'
            )
         })
   })

   it('fetches side projects', () => {
      cy.testid('project-container').should('have.length', 2)

      cy.testid('project-container')
         .eq(0)
         .within(() => {
            cy.testid('title-input').should('have.value', 'Richtext Editor')
            cy.testid('content-richtext').should('have.text', 'A HTML richtext editor')
         })

      cy.testid('project-container')
         .eq(1)
         .within(() => {
            cy.testid('title-input').should('have.value', 'Cactus Guru')
            cy.testid('content-richtext').should('have.text', 'Cacti Collection Manager')
         })
   })

   it('fetches educations', () => {
      cy.testid('education-container').should('have.length', 2)

      cy.testid('education-container')
         .eq(0)
         .within(() => {
            cy.testid('degree-input').should('have.value', 'B.S.')
            cy.testid('university-input').should('have.value', 'S&C')
            cy.testid('startDate-input').should('have.value', '2010-01-01')
            cy.testid('endDate-input').should('have.value', '2011-01-01')
         })

      cy.testid('education-container')
         .eq(1)
         .within(() => {
            cy.testid('degree-input').should('have.value', 'M.S.')
            cy.testid('university-input').should('have.value', 'S&C')
            cy.testid('startDate-input').should('have.value', '2011-01-01')
            cy.testid('endDate-input').should('have.value', '2012-01-01')
         })
   })
})

describe('deleting items', () => {
   beforeEach(() => {
      cy.server()
      cy.route('GET', '/api/developer', 'fixture:developer.json')
      cy.visit('/')
   })

   it('deletes an experience', () => {
      cy.testid('experience-container')
         .eq(0)
         .within(() => {
            cy.testid('delete-button').click()
         })

      cy.testid('experience-container').should('have.length', 1)
      cy.testid('experience-container').within(() => {
         cy.testid('company-input').should('have.value', 'Parmis')
      })
   })

   it('deletes a side project', () => {
      cy.testid('project-container')
         .eq(1)
         .within(() => {
            cy.testid('delete-button').click()
         })

      cy.testid('project-container').should('have.length', 1)
      cy.testid('project-container').within(() => {
         cy.testid('title-input').should('have.value', 'Richtext Editor')
      })
   })

   it('deletes an education', () => {
      cy.testid('education-container')
         .eq(0)
         .within(() => {
            cy.testid('delete-button').click()
         })

      cy.testid('education-container').should('have.length', 1)
      cy.testid('education-container').within(() => {
         cy.testid('degree-input').should('have.value', 'M.S.')
      })
   })
})

describe('loading', () => {
   it('shows loader', () => {
      cy.server()
      cy.route({
         method: 'GET',
         url: '/api/developer',
         response: 'fixture:developer.json',
         delay: 300
      }).as('getDeveloper')
      cy.visit('/')

      cy.testid('loader-container').should('be.visible')
      cy.wait('@getDeveloper')
      cy.testid('loader-container').should('not.be.visible')
   })

   it('shows retry when loading failed', () => {
      cy.server()
      cy.route('GET', '/api/developer', '').as('getDeveloper')
      cy.visit('/')

      cy.wait('@getDeveloper')
      cy.testid('message-container').should($c => {
         expect($c).to.be.visible
         expect($c).to.contain('Unexpected end of JSON input')
         expect($c).to.contain('Try again')
      })
   })
})

describe('saving', () => {
   it('sends correct state to end-point', () => {
      cy.server()
      cy.route('GET', '/api/developer', 'fixture:developer.json').as('getDeveloper')
      cy.route({
         method: 'PUT',
         url: '/api/developer',
         response: {
            experiences: [1, 2],
            sideProjects: [1, 2],
            educations: [1, 2]
         },
         onRequest: msg => {
            const experience1 = msg.request.body.experiences[0]
            expect(experience1).to.deep.equal({
               id: 1,
               company: 'Lodgify',
               position: 'C# Developer',
               startDate: '2019-01-01',
               endDate: '2020-01-01',
               content: '<p contenteditable="true">worked as a back-end developer</p>'
            })

            const experience2 = msg.request.body.experiences[1]
            expect(experience2.id).not.to.be.a('number')
            expect(experience2.company).to.equal('Bellin')
            expect(experience2.position).to.equal('Fullstack Developer')
            expect(experience2.startDate).to.equal('2022-01-01')
            expect(experience2.endDate).to.equal('2023-01-01')
            expect(experience2.content).to.equal('<p contenteditable="true">MERN Stack</p>')

            const proj1 = msg.request.body.sideProjects[0]
            expect(proj1).to.deep.equal({
               id: 2,
               title: 'Cactus Guru',
               content: '<p contenteditable="true">Cacti Collection Manager</p>'
            })

            const proj2 = msg.request.body.sideProjects[1]
            expect(proj2.id).not.to.be.a('number')
            expect(proj2.title).to.equal('Blog')
            expect(proj2.content).to.equal('<p contenteditable="true">This exact blog</p>')

            const edu = msg.request.body.educations[0]
            expect(edu.id).not.to.be.a('number')
            expect(edu.degree).to.equal('B.S.')
            expect(edu.university).to.equal('Science & Culture')
            expect(edu.startDate).to.equal('2010-01-01')
            expect(edu.endDate).to.equal('2014-01-01')
         }
      })

      cy.visit('/')

      // change experiences

      cy.testid('experience-container')
         .eq(1)
         .within(() => {
            cy.testid('delete-button').click()
         })

      cy.testid('add-experience-button').click()

      cy.testid('experience-container')
         .eq(1)
         .within(() => {
            cy.testid('company-input').type('Bellin')
            cy.testid('position-input').type('Fullstack Developer')
            cy.testid('startDate-input').type('2022-01-01')
            cy.testid('endDate-input').type('2023-01-01')
            cy.testid('content-richtext')
               .find('p')
               .type('MERN Stack')
         })

      // change side projects

      cy.testid('project-container')
         .eq(0)
         .within(() => {
            cy.testid('delete-button').click()
         })

      cy.testid('add-project-button').click()

      cy.testid('project-container')
         .eq(1)
         .within(() => {
            cy.testid('title-input').type('Blog')
            cy.testid('content-richtext')
               .find('p')
               .type('This exact blog')
         })

      // change educations

      cy.testid('education-container')
         .eq(0)
         .as('firstEducation')
      cy.get('@firstEducation').within(() => cy.testid('delete-button').click())
      cy.get('@firstEducation').within(() => cy.testid('delete-button').click())

      cy.testid('add-education-button').click()

      cy.get('@firstEducation').within(() => {
         cy.testid('degree-input').type('B.S.')
         cy.testid('university-input').type('Science & Culture')
         cy.testid('startDate-input').type('2010-01-01')
         cy.testid('endDate-input').type('2014-01-01')
      })

      // save changes

      cy.testid('save-button').click()
   })

   it('shows the error if saving fails', () => {
      cy.server()
      cy.route('GET', '/api/developer', 'fixture:developer.json')
      cy.route('PUT', '/api/developer', '').as('putDeveloper')

      cy.visit('/')
      cy.testid('save-button').click()
      cy.wait('@putDeveloper')
      cy.testid('toaster-container').should($c => {
         expect($c).to.be.visible
         expect($c).to.have.class('error')
      })
   })

   it('shows loader indicator when saving', () => {
      cy.server()
      cy.route('GET', '/api/developer', 'fixture:developer.json')
      cy.route({
         method: 'PUT',
         url: '/api/developer',
         delay: 300,
         response: { experiences: [], sideProjects: [], educations: [] }
      }).as('putDeveloper')

      cy.visit('/')
      cy.testid('save-button').click()
      cy.testid('loader-container').should('be.visible')
      cy.wait('@putDeveloper')
      cy.testid('loader-container').should('not.be.visible')
      cy.testid('toaster-container').should($c => {
         expect($c).to.be.visible
         expect($c).to.have.class('success')
      })
   })
})

describe('succeed network requests', () => {
   beforeEach(() => {
      cy.server()
      cy.route('GET', '/api/drafts', 'fixture:posts.json').as('getDrafts')
      cy.visit('/')
   })

   it('display post items', () => {
      cy.wait('@getDrafts')

      cy.testid('post-list').within(() => {
         cy.get('ol')
            .children()
            .should('have.length', 3)

         cy.get('li').should($li => {
            expect($li.eq(0)).to.contain('قراردادهای خوانش')
            expect($li.eq(0)).to.contain('1398-10-25')

            expect($li.eq(1)).to.contain('با Regular Expression رفیق شویم!')
            expect($li.eq(1)).to.contain('1398-6-2')

            expect($li.eq(2)).to.contain('OAuth2.0: Introduction')
            expect($li.eq(2)).to.contain('1397-12-15')
            expect($li.eq(2)).to.contain('Not Published')
         })
      })
   })

   it('shows loader', () => {
      cy.testid('loader-container').should('be.visible')
      cy.wait('@getDrafts')
      cy.testid('loader-container').should('not.be.visible')
   })
})

describe('failed network requests', () => {
   it('shows error message on error', () => {
      cy.server()
      cy.route({
         method: 'GET',
         url: '/api/drafts',
         status: 500,
         response: ''
      })
      cy.visit('/')
      cy.testid('message-container').should('be.visible')
   })

   it('loads again on TryAgain', () => {
      cy.server()
      cy.route({ method: 'GET', url: '/api/drafts', response: '', status: 500 }).as('getDrafts')
      cy.visit('/')

      cy.wait('@getDrafts')
      cy.testid('post-list').should('not.be.visible')

      cy.route('GET', '/api/drafts', 'fixture:posts.json')
      cy.contains('Try again').click()

      cy.testid('post-list').should('be.visible')
   })
})

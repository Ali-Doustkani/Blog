describe('saving', () => {
   beforeEach(() => {
      cy.server()
      cy.visit('/post')
   })

   it('show message when saved automatically', () => {
      cy.route('POST', '/api/drafts').as('postDraft')

      cy.contains('English').click()
      cy.focused().type('Post Title')
      cy.get('article>p').type('Post Content')

      cy.wait('@postDraft')
      cy.get('.saveMessage').should('be.visible')
      cy.wait(2100)
      cy.get('.saveMessage').should('not.be.visible')
   })

   it('saves the draft for the first time', () => {
      cy.route({
         method: 'POST',
         url: '/api/drafts',
         onRequest: msg => {
            const post = msg.request.body
            expect(post.lang).to.equal('english')
            expect(post.title).to.equal('Post Title')
            // expect(post.content).to.equal('<p contenteditable="true">Post Content</p>')
         }
      }).as('postDraft')

      cy.contains('English').click()
      cy.focused().type('Post Title')
      cy.get('article>p').type('Post Content')
      cy.wait('@postDraft')
   })
})

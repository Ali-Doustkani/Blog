beforeEach(() => {
   cy.server()
   cy.route('GET', '/api/posts', 'fixture:posts.json').as('getPosts')
   cy.visit('/')
})

it('display post items', () => {
   cy.wait('@getPosts')

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
   cy.wait('@getPosts')
   cy.testid('loader-container').should('not.be.visible')
})

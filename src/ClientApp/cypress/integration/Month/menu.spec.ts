describe('Menu', () => {
    beforeEach(() => {
        cy.apiLogin({});
        cy.goToMonth({ year: 2022, month: 12 })
        cy.viewport('iphone-8');
    })
    
    it('should display a menu button on mobile', () => {
        cy.get('.navbar-burger').should('exist');
    })
    
    it('should show the menu on clicking the burger', () => {
        cy.get('.navbar-burger').click();
        cy.get('.navbar-item').should('contain', 'Previous Month')
        cy.get('.navbar-item').should('contain', 'Next Month')
        cy.get('.navbar-item').should('contain', 'Today')
    })
    
    it('should disappear the menu after selecting an option', () => {
        cy.get('.navbar-burger').click();
        cy.get('.navbar-item').contains('Previous Month').click()
        cy.get('.navbar-item').contains('Today').should('not.be.visible')
        cy.getDay('Nov 1, 2022').should('exist')
    })
})

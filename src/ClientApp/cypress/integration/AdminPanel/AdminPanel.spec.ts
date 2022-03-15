
describe('Admin Panel', () => {
    beforeEach(() => {
        cy.apiLogin({});
        cy.goToMonth({year: 2022, month: 12})
    })
    
    it('should open the admin panel', () => {
        cy.get('.navbar-burger').click();
        cy.get('.navbar-item').contains('Settings').click();
        cy.get('.card-header-title').should('contain', 'Admin Settings');
    });
    
    it('should disable sign ups', () =>{
        cy.get('.navbar-burger').click();
        cy.get('.navbar-item').contains('Settings').click();
        
        cy.get('input[type="checkbox"').click();
        cy.get('.divButton').contains('Save').click();
        cy.wait(100);
        
        cy.get('.navbar-item').contains('Sign Out').click();
        cy.get('.button').should('not.contain', 'Sign Up')

        cy.apiLogin({});
        cy.goToMonth({year: 2022, month: 12})

        cy.get('.navbar-burger').click();
        cy.get('.navbar-item').contains('Settings').click();

        cy.get('input[type="checkbox"').click();
        cy.get('.divButton').contains('Save').click();
    })
})
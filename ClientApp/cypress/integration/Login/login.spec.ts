/// <reference types="Cypress" />

describe('Login', () => {
  it('should display the login page', () => {
    cy.visit('/')
    cy.get('.card-header-title')
      .should('contain.text', 'Welcome')
  });

  it('should Like Login or something', () => {
    cy.get('[formControlName=username]').type('test1');
    cy.get('[formControlName=password]').type('WordToYourMom');
    cy.contains('button', 'Login').click();

    cy.location('pathname').should('contain', 'month')
      .then(() => {
        const bearerToken = window.localStorage.getItem('bearerToken');
        const refreshToken = cy.getCookie('refreshToken');

        expect(refreshToken).to.be.not.null;
        expect(bearerToken).to.be.a('string');

        const token = JSON.parse(bearerToken!);
        expect(token).to.be.a('object');
        expect(token).to.have.keys(['token', 'expiresAt'])
      })
  });

  it('should logout', () => {
    cy.get('.navbar-burger').click();
    cy.get('.navbar-item').contains('Sign Out').click()
      .then(() => {
        const bearerToken = window.localStorage.getItem('bearerToken');
        expect(bearerToken).to.be.null;
      });

  });

  it('should display an error for invalid username/password', () => {
    cy.get('[formControlName=username]').type('test1');
    cy.contains('button', 'Login').click();

    cy.get('#notificationBar').should('contain.text', 'Incorrect Username or Password');
    cy.get('#notificationBar').find('button').click();

    cy.get('#notificationBar').should('not.exist');
  })
})


describe('Quick Search', () => {
  beforeEach(() => {
    cy.apiLogin({});
    cy.goToMonth({year: 2022, month: 12})
  })
  const date = 'Dec 1, 2022';

  it('should display the quick search box', () => {
    cy.get('[name=quickSearch]').should('exist');
  })

  it('should search transactions and months', () => {
    const name1 = (Math.random() + 1).toString(36).substring(7);
    cy.openTransactionForm(date, "Income")
    cy.transactionFill(name1, 1);
    cy.get('a').should('contain', name1);
    cy.get('[name=quickSearch]').should('exist')
    cy.get('[name=quickSearch]').type(name1);
    cy.get('.dropdown-item').should('contain', name1)

    cy.focused().clear();
    cy.focused().type('1')
    cy.get('.dropdown-item').should('contain', 'Dec 1')

    cy.get('body').type('{esc}')

    cy.deleteTransaction(name1)
  })
})

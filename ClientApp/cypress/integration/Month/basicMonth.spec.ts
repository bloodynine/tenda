const date = 'Dec 1, 2022';

describe('Month', () => {
  beforeEach(() => {
    cy.apiLogin({});
    cy.goToMonth({year: 2022, month: 12})
  })

  it('should display', () => {
    cy.get('.container').children().should('have.length', 31)
    cy.getDay(date).find('#bills')
    cy.getDay(date).find('#incomes')
    cy.getDay(date).find('#oneOffs')
  })

  it('should load a new month', () => {
    cy.goToMonth({year: 2022, month: 2})
    cy.get('.container').children().should('have.length', 28)
  })

  it('should add income', () => {
    cy.dayTotal(date).then( (beforeAmount) => {
      cy.getDay(date).find('.divButton').contains('Add Income').click();
      cy.transactionFill('test1', 7);
      cy.dayTotal(date).should('eq', beforeAmount + 7)
    })
    cy.getDay(date)
      .find('#incomes')
      .children()
      .contains('test1');

    cy.deleteTransaction('test1');
  })

  it('should add a bill', () => {
    cy.dayTotal(date).then((beforeAmount) => {
        cy.getDay(date).find('.divButton')
          .contains('Add Bill')
          .click()

        cy.transactionFill('Test2', 12);
        cy.dayTotal(date).should('eq', beforeAmount - 12);
    })
    cy.getDay(date).find('#bills')
      .children().contains('Test2');

    cy.deleteTransaction('Test2');
  })

  it('should add a oneOff', () => {
    cy.dayTotal(date).then((beforeAmount) => {
      cy.getDay(date).find('.divButton')
        .contains('Add Transaction')
        .click()

      cy.transactionFill('Test3', 34);
      cy.dayTotal(date).should('eq', beforeAmount - 34);
    })
    cy.getDay(date).find('#oneOffs')
      .children().contains('Test3');

    cy.deleteTransaction('Test3');
  })
})

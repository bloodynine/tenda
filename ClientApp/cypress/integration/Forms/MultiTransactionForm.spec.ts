describe('Multi Transaction Form', () => {
  beforeEach(() => {
    cy.apiLogin({});
    cy.goToMonth({year:2022, month:12});
  });

  const date = 'Dec 1, 2022';
  function saveIsDisabled() {
    cy.get('.divButton').contains('Save').invoke('attr', 'class').then(x => x.split(' ')).should('contain', 'disabledDivButton')
  }

  function saveNotDisabled() {
    cy.get('.divButton').contains('Save').invoke('attr', 'class').then(x => x.split(' ')).should('not.contain', 'disabledDivButton')
  }

  it('should be able to submit a valid form with multiple entries', ()=> {
    const name1 = (Math.random() + 1).toString(36).substring(7);
    const name2 = (Math.random() + 1).toString(36).substring(7);
    cy.openTransactionForm(date, 'Transaction');
    cy.get('label').contains('Name').siblings('.control').find('.input').type(name1);
    cy.get('label').contains('Amount').siblings('.control').find('.input').type('1');

    saveNotDisabled();
    cy.get('button').contains('Add Another').click();
    saveIsDisabled();

    cy.get('label').eq(3).siblings('.control').find('.input').type(name2);
    cy.get('label').eq(4).siblings('.control').find('.input').type('1');
    saveNotDisabled();

    cy.get('.divButton').contains('Save').click();
    cy.getDay(date).find('#oneOffs').children().should('contain', name1)
      cy.getDay(date).find('#oneOffs').children().should('contain', name2)

    cy.deleteTransaction(name1);
    cy.deleteTransaction(name2);
  })

  it('should be invalid with no name', ()=> {
    const name1 = (Math.random() + 1).toString(36).substring(7);
    const name2 = (Math.random() + 1).toString(36).substring(7);
    cy.openTransactionForm(date, 'Transaction');
    cy.get('label').contains('Name').siblings('.control').find('.input').type(name1);

    cy.focused().clear();

    cy.get('.help.is-danger').should('contain', 'Name field is required')
    saveIsDisabled();

    cy.get('label').contains('Amount').siblings('.control').find('.input').type('1');
    cy.get('label').contains('Name').siblings('.control').find('.input').type(name1);
    saveNotDisabled();

    cy.get('button').contains('Add Another').click();
    saveIsDisabled();

    cy.get('label').eq(3).siblings('.control').find('.input').type(name2);
    cy.focused().clear()
    cy.get('.help.is-danger').should('contain', 'Name field is required')
    saveIsDisabled();
  })
})

describe('Single Transaction Form', () => {
  beforeEach(() => {
    cy.apiLogin({});
    cy.goToMonth({year: 2022, month: 12})
  });

  function saveIsDisabled() {
    cy.get('.divButton').contains('Save').invoke('attr', 'class').then(x => x.split(' ')).should('contain', 'disabledDivButton')
  }
  const date = 'Dec 1, 2022';

  function saveNotDisabled() {
    cy.get('.divButton').contains('Save').invoke('attr', 'class').then(x => x.split(' ')).should('not.contain', 'disabledDivButton')
  }

  it('should be able to submit a valid form', () => {
    cy.openTransactionForm(date, 'Income');
    cy.get('label').contains('Name').siblings('.control').find('.input').type('Name1');
    cy.get('label').contains('Amount').siblings('.control').find('.input').type('1');

    saveNotDisabled();

    cy.get('.divButton').contains('Save').click();

    cy.getDay(date).find('#incomes').children().contains('Name1');
    cy.deleteTransaction('Name1')
  });

  it('should be invalid when touched without a name', () => {
    cy.openTransactionForm(date, 'Income');
    cy.get('label').contains('Name').siblings('.control').find('.input').type('Name1');
    cy.focused().clear();

    saveIsDisabled();
    cy.get('.help.is-danger').contains('Name field is required')
  });

  it('should be invalid when touched without an amount', () => {
    cy.openTransactionForm(date, 'Income');
    cy.get('label').contains('Amount').siblings('.control').find('.input').type('1');
    cy.focused().clear();

    saveIsDisabled();
    cy.get('.help.is-danger').contains('Amount field is required')
  });

  it('should be invalid when amount is not a number', () => {
    cy.openTransactionForm(date, 'Income');
    const testAmounts = ['d', '1.111', '1d2']
    testAmounts.forEach(x => {
      cy.get('label').contains('Amount').siblings('.control').find('.input').type(x);

      saveIsDisabled();
      cy.get('.help.is-danger').contains('Amount must be a number');
      cy.focused().clear();
    })
  });

  it('should be invalid when interval is not null and not a number', () => {
    cy.openTransactionForm(date, 'Income');
    const testAmounts = ['d', '1.111', '1d2']
    testAmounts.forEach(x => {
      cy.get('label').contains('Interval').siblings('.control').find('.input').type(x);

      saveIsDisabled();
      cy.get('.help.is-danger').contains('Interval must be a number when not empty');
      cy.focused().clear();
    })
  });
})

describe('Single Transaction Form Tag Tests', () => {
  const date = 'Dec 1, 2022';
  beforeEach(() => {
    cy.apiLogin({});
    cy.goToMonth({year: 2022, month: 12})
  });

  it('should update the list of tags when saved', () => {
    cy.openTransactionForm(date, 'Income');
    cy.getTextField('Name').type('Name1');
    cy.getTextField('Amount').type('1');
    cy.getTextField('Tags').type('Tag1{enter}')
    cy.getTextField('Tags').type('Tag2{enter}')
    cy.getTextField('Tags').type('Three{enter}')
    cy.contains('.divButton', 'Save').click();

    cy.goToMonth({year: 2022, month: 12}) // Re-naving to the page to fix detached dom issue with cypress
    cy.openTransactionForm(date, 'income')

    cy.getTextField('Tags').type('T');
    cy.get('.dropdown-item').should('contain', 'Tag1')
      .and('contain', 'Tag2')
      .and('contain', 'Three')

    cy.focused().clear();
    cy.getTextField('Tags').type('th');
    cy.get('.dropdown-item').should('contain', 'Three');

    cy.get('.modal-close').click();
    cy.deleteTransaction('Name1');
  })
})


/// <reference types="cypress" />
import './commands'
declare global {
  namespace Cypress {
    interface Chainable {
      /**
       * Custom command to select DOM element by data-cy attribute.
       * @example cy.dataCy('greeting')
       */
      apiLogin(user: any): void;
      goToMonth(month: any): void;
      getDay(dateStr: any): Chainable<JQuery>;
      kwTest(someThing: string): void;
      dataCy(value: string): Chainable<JQuery>
      transactionFill(transactionName: string, amount: number): void;
      deleteTransaction(transactionName: string): void;
      dayTotal(dateString: string): Chainable<number>;
      openTransactionForm(dateString: string, formType: string): Chainable<JQuery>;
      getTextField(labelName: string): Chainable<JQuery>;
    }
  }
}

/// <reference types="cypress" />
import Chainable = Cypress.Chainable;

Cypress.Commands.add('kwTest', (someThing: string) => {
  console.log(someThing)
})
Cypress.Commands.add('apiLogin', (user) => {
  cy.request('POST', 'https://localhost:7139/api/users/login', {username: Cypress.env('e2eUser'), password:Cypress.env('e2ePass')}).then( (response) => {
      const tokens = response.body;
      const bearerToken = {token: tokens.bearerToken, expiresAt: tokens.bearerExpiresAt};
      localStorage.setItem('bearerToken', JSON.stringify(bearerToken) );
      cy.setCookie('refreshToken', tokens.refreshToken, {sameSite: "strict"})
    }
  )
});

Cypress.Commands.add('goToMonth', (dateObj) => {
  cy.visit(`/year/${dateObj.year}/month/${dateObj.month}`);
})

Cypress.Commands.add('getDay', (dateStr) =>
{
  return cy.get('div').contains(dateStr).parents('.card');
})

Cypress.Commands.add('dataCy', (value) => {
  return cy.get(`[data-cy=${value}]`)
})

Cypress.Commands.add('transactionFill', (transactionName: string, amount: number) => {
  cy.get('label').contains('Name').siblings('.control').find('.input').type(transactionName);
  cy.get('label').contains('Amount').siblings('.control').find('.input').type(`${amount}`);
  cy.contains('.divButton', 'Save').click();
})

Cypress.Commands.add('deleteTransaction', (transactionName: string) => {
  cy.get('a').contains(transactionName).click();
  cy.contains('.deleteButton', 'Delete').click()
})

Cypress.Commands.add('dayTotal', (dateString: string) => {
  cy.getDay(dateString).find('#dayTotal')
    .invoke('text')
    .then(text => +text
      .replace('$', '')
      .replace(',', '').trim()
    )
})

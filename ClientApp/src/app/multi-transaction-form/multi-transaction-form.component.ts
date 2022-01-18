import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { Transaction } from "../Shared/Interfaces/Transaction";
import { StateService } from "../state.service";
import { TransactionService } from "../transaction.service";

@Component({
  selector: 'app-multi-transaction-form',
  templateUrl: './multi-transaction-form.component.html',
  styleUrls: ['./multi-transaction-form.component.css']
})
export class MultiTransactionFormComponent implements OnInit {
  transactionDate: Date = new Date();

  transactions: Array<Transaction> = [];
  constructor(
    private stateService: StateService,
    private transactionService: TransactionService,
  ) { }

  ngOnInit(): void {
    this.stateService.multiTransactionDate.subscribe(x => {
      this.transactionDate = x;
      this.transactions = [];
      this.transactions.push({name: "", date: x} as Transaction);
    });
  }

  addAnother() {
    this.transactions.push({name: "", date: this.transactionDate} as Transaction);

  }

  save() {
    this.transactions.forEach(x => {
      if (x.amount > 0){
        x.amount = -1 * x.amount;
      }
    })

    this.transactionService.CreateBulkTransactions(this.transactions).then(x => this.stateService.UpdateMonth(x));
    this.stateService.ExitAllModals();
  }

  cancel() {
    this.stateService.ExitAllModals();
  }

  removeTransaction(index: number) {
    this.transactions.splice(index, 1);
  }
}

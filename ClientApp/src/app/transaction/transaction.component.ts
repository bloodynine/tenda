import {Component, Input, OnInit} from '@angular/core';
import {Transaction, TransactionType} from "../Shared/Interfaces/Transaction";
import {TransactionService} from "../transaction.service";

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit {
  @Input() transaction: Transaction = {} as Transaction;
  classType: Array<string> = [];
  constructor(private transactionService: TransactionService) { }

  ngOnInit(): void {
    if(this.transaction){
      this.classType.push(TransactionType[this.transaction.type]);
      if(this.transaction.isResolved){
        this.classType.push('faded')
      }
    }
  }

  toggleResolved(){
    this.transaction.isResolved = !this.transaction.isResolved;
    this.transactionService.UpdateTransaction(this.transaction);
  }

  editTransaction(){
    this.transactionService.malleableTransaction.next(this.transaction);
  }

}

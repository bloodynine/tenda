import { Component, Input, OnInit } from '@angular/core';
import { Day } from "../Shared/Interfaces/Day";
import { TransactionService } from "../transaction.service";
import { Transaction, TransactionType } from "../Shared/Interfaces/Transaction";
import { StateService } from "../state.service";

@Component({
  selector: 'app-day',
  templateUrl: './day.component.html',
  styleUrls: ['./day.component.css']
})
export class DayComponent implements OnInit {
  @Input() day : Day | undefined;
  constructor(
    private transactionService: TransactionService,
    private stateService: StateService) { }

  ngOnInit(): void {
  }

  createTransaction(type: TransactionType): void {
    if(type == TransactionType.OneOff){
      this.stateService.EditMultiTransaction(this.day?.date);
    }else{
      this.stateService.EditTransaction({date: this.day?.date, type: type} as Transaction);
    }
  }
}

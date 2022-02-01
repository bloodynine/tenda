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
  localDate: string = '';
  constructor(
    private transactionService: TransactionService,
    private stateService: StateService) { }

  ngOnInit(): void {
    const options = {month: 'short', day: "numeric"} as Intl.DateTimeFormatOptions;
    this.localDate = new Date(this.day?.date ?? '').toLocaleDateString('en-US', options) ?? '';
  }

  createTransaction(type: TransactionType): void {
    if(type == TransactionType.OneOff){
      this.stateService.EditMultiTransaction(this.day?.date);
    }else{
      this.stateService.EditTransaction({date: this.day?.date, type: type} as Transaction);
    }
  }
}

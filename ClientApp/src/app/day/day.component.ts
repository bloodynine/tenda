import {Component, Input, OnInit} from '@angular/core';
import {Day} from "../Shared/Interfaces/Day";
import {TransactionService} from "../transaction.service";
import {Transaction, TransactionType} from "../Shared/Interfaces/Transaction";

@Component({
  selector: 'app-day',
  templateUrl: './day.component.html',
  styleUrls: ['./day.component.css']
})
export class DayComponent implements OnInit {
  @Input() day : Day | undefined;
  constructor(private transactionService: TransactionService) { }

  ngOnInit(): void {
  }

  createTransaction(type: TransactionType): void {
    this.transactionService.malleableTransaction.next({date: this.day?.date, type: type} as Transaction)
  }
}

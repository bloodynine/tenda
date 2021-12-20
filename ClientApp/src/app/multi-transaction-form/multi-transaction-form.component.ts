import { Component, Input, OnInit } from '@angular/core';
import { Transaction } from "../Shared/Interfaces/Transaction";
import { FormControl, FormGroup } from "@angular/forms";

@Component({
  selector: 'app-multi-transaction-form',
  templateUrl: './multi-transaction-form.component.html',
  styleUrls: ['./multi-transaction-form.component.css']
})
export class MultiTransactionFormComponent implements OnInit {
  @Input() transactionDate: Date = new Date();

  transactions: Array<Transaction> = [{} as Transaction];
  constructor() { }

  ngOnInit(): void {
  }

}

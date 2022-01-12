import {Component, ElementRef, Input, OnInit} from '@angular/core';
import {Transaction, TransactionType, TransactionTypeLabelMapping} from "../Shared/Interfaces/Transaction";
import {FormControl, FormGroup} from "@angular/forms";
import * as bulmaCalendar from "bulma-calendar";
import {TransactionService} from "../transaction.service";
import {RepeatType, RepeatTypeLabel} from "../Shared/Interfaces/RepeatSettings";
import {StateService} from "../state.service";
import {RepeatService} from "../repeat.service";

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css']
})
export class TransactionFormComponent implements OnInit {
  transaction: Transaction = {} as Transaction;
  form: FormGroup = new FormGroup({});

  public types:TransactionType[] = [TransactionType.Bill, TransactionType.Income, TransactionType.OneOff];
  public labelMapping = TransactionTypeLabelMapping;

  constructor(private elRef: ElementRef,
              private transactionService: TransactionService,
              private stateService: StateService,
              private repeatService: RepeatService) { }

  get isNewTransaction(): boolean {
    return !this.transaction.id;
  }
  get showRepeat(): boolean {
    return this.isNewTransaction && this.form.get('type')?.value!= TransactionType.OneOff;
  }

  get showModifyRepeatButton(): boolean{
    return !this.isNewTransaction && this.transaction.isRepeating;
  }

  ngOnInit(): void {
    this.stateService.editingTransaction.subscribe(x => {
      this.transaction = x;
      this.form = new FormGroup({
        transactionName: new FormControl(x.name),
        amount: new FormControl(x.amount),
        date: new FormControl(x.date),
        repeatFrequency: new FormControl(''),
        interval: new FormControl('')
      })
    })
  }

  cancel() {
    this.stateService.ExitAllModals();
  }

  delete() {
    this.transactionService.DeleteTransaction(this.transaction)
      .then(x => this.stateService.UpdateMonth(x));
    this.stateService.ExitAllModals();
  }

  save() {
    this.patchInValues();
    if(this.isNewTransaction){
      this.transactionService.CreateNewTransaction(this.transaction)
        .then(x => this.stateService.UpdateMonth(x));
    }else{
      this.transactionService.UpdateTransaction(this.transaction).then(x => this.stateService.UpdateMonth(x)
      );
    }
    this.stateService.ExitAllModals();
  }

  public editRepeatSettings(): void {
    this.repeatService.GetRepeatContract(this.transaction.associatedRepeatId).then(x =>{
      // We Set the start date of the repeat contract to the current transaction date.
      // This provides the expected context to the user doing the editing
      x.startDate = this.transaction.date;
      this.stateService.EditRepeatSetting(x)
      });
  }

  private patchInValues(): void {
    this.transaction.name = this.form.get('transactionName')?.value;
    this.transaction.amount = this.form.get('amount')?.value;
    if(this.form.get('repeatFrequency') && this.form.get('repeatFrequency')?.value != ''){
      this.transaction.repeatSettings = {
        type: Number.parseInt(this.form.get('repeatFrequency')?.value),
        interval: this.form.get('interval')?.value,
      startDate: new Date(this.form.get('date')?.value)}
    }

    if(this.transaction.amount > 0 && this.transaction.type != TransactionType.Income){
      this.transaction.amount = this.transaction.amount * -1;
    }
  }
}

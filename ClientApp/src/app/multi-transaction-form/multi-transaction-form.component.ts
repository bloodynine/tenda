import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { Transaction } from "../Shared/Interfaces/Transaction";
import { StateService } from "../state.service";
import { TransactionService } from "../transaction.service";
import { Form, FormArray, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: 'app-multi-transaction-form',
  templateUrl: './multi-transaction-form.component.html',
  styleUrls: ['./multi-transaction-form.component.css']
})
export class MultiTransactionFormComponent implements OnInit {
  transactionDate: Date = new Date();
  form: FormGroup = new FormGroup({});
  transactionTags: Array<Array<string>> = []
  numRegex = /^-?\d*[.,]?\d{0,2}$/;

  get formArray(): FormArray{
    return this.form.get('list') as FormArray
  }

  constructor(
    private stateService: StateService,
    private transactionService: TransactionService,
    private formBuilder: FormBuilder
  ) { }

  doesFieldHaveError(index: number, fieldName: string, error?: string): boolean {
    const array = this.form.get('list') as FormArray;
    const group =array.controls[index] as FormGroup;
    const pristine = group.get(fieldName)?.pristine;
    if(!error){
      return !pristine && (group.get(fieldName)?.invalid ?? false);
    }
    return !pristine && (group.get(fieldName)?.hasError(error) ?? false);
  }

  ngOnInit(): void {
    this.transactionTags.push([]);
    this.stateService.multiTransactionDate.subscribe(x => {
      this.form = this.formBuilder.group({
        list: this.formBuilder.array([
          this.formBuilder.group({
            transactionName: new FormControl('', [Validators.required]),
            amount: new FormControl('', [Validators.required, Validators.pattern(this.numRegex)]),
          })
        ])
      })
      this.transactionDate = x;
    });
  }

  addAnother() {
    this.formArray.push(
      this.formBuilder.group({
      transactionName: new FormControl('', [Validators.required]),
      amount: new FormControl('', [Validators.required, Validators.pattern(this.numRegex)]),
      tags: new FormControl('')
      })
    )
    this.transactionTags.push([]);
  }

  save() {
    const request = this.buildRequest();
    this.transactionService.CreateBulkTransactions(request).then(x => this.stateService.UpdateMonth(x));
    this.stateService.ExitAllModals();
  }

  cancel() {
    this.stateService.ExitAllModals();
  }

  removeTransaction(index: number) {
    this.formArray.removeAt(index);
    this.transactionTags.splice(index, 1);
  }
  private buildRequest(): Transaction[] {
    let transactions: Transaction[] = [];
    this.formArray.controls.forEach((control, i) => {
      let amount = control.get('amount')?.value;
      if(amount > 0 ) {
        amount = amount * -1;
      }
     transactions.push(
       {
         name: control.get('transactionName')?.value,
         amount: amount,
         date: this.transactionDate,
         tags: this.transactionTags[i]
       } as Transaction
     )
    })
    return transactions;
  }
}

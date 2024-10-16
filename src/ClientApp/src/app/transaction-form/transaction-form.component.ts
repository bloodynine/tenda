import { Component, ElementRef, OnInit } from '@angular/core';
import { Transaction, TransactionType, TransactionTypeLabelMapping } from "../Shared/Interfaces/Transaction";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { TransactionService } from "../Shared/Services/transaction.service";
import { StateService } from "../Shared/Services/state.service";
import { RepeatService } from "../Shared/Services/repeat.service";

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css']
})
export class TransactionFormComponent implements OnInit {
  transaction: Transaction = {} as Transaction;
  form: FormGroup = new FormGroup({});
  endDate: Date | undefined;

  public types: TransactionType[] = [TransactionType.Bill, TransactionType.Income, TransactionType.OneOff];
  public labelMapping = TransactionTypeLabelMapping;

  constructor(private elRef: ElementRef,
              private transactionService: TransactionService,
              private stateService: StateService,
              private repeatService: RepeatService,
              private fb: FormBuilder) {
  }

  get isNewTransaction(): boolean {
    return !this.transaction.id;
  }

  get showRepeat(): boolean {
    return this.isNewTransaction && this.form.get('type')?.value != TransactionType.OneOff;
  }

  get showModifyRepeatButton(): boolean {
    return !this.isNewTransaction && this.transaction.isRepeating;
  }

  isFormInvalid(): boolean {
    return this.form.status == 'INVALID'
  }

  doesFieldHaveError(fieldName: string, error?: string): boolean {
    const pristine = this.form.get(fieldName)?.pristine;
    if (!error) {
      return !pristine && (this.form.get(fieldName)?.invalid ?? false);
    }
    return !pristine && (this.form.get(fieldName)?.hasError(error) ?? false);
  }

  ngOnInit(): void {
    const numRegex = /^-?\d*[.,]?\d{0,2}$/;
    this.stateService.editingTransaction.subscribe(x => {
      this.transaction = x;
      this.form = this.fb.group({
        transactionName: new FormControl(x.name, [Validators.required]),
        amount: new FormControl(x.amount, [Validators.required, Validators.pattern(numRegex)]),
        date: new FormControl(new Date(x.date), [Validators.required]),
        repeatSettings: this.fb.group({
          repeatFrequency: new FormControl(''),
          endDate: new FormControl(''),
          interval: new FormControl('', Validators.pattern(numRegex))
        })
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
    if (this.isFormInvalid()) {
      return;
    }
    this.patchInValues();
    if (this.isNewTransaction) {
      this.transactionService.CreateNewTransaction(this.transaction)
        .then(x => this.stateService.UpdateMonth(x));
    } else {
      this.transactionService.UpdateTransaction(this.transaction).then(x => this.stateService.UpdateMonth(x)
      );
    }
    this.stateService.ExitAllModals();
  }

  public editRepeatSettings(): void {
    this.repeatService.GetRepeatContract(this.transaction.associatedRepeatId).then(x => {
      // We Set the start date of the repeat contract to the current transaction date.
      // This provides the expected context to the user doing the editing
      x.startDate = this.transaction.date;
      this.stateService.EditRepeatSetting(x)
    });
  }

  private patchInValues(): void {
    const repeatSettings = this.form.get('repeatSettings') as FormGroup
    this.transaction.name = this.form.get('transactionName')?.value;
    this.transaction.amount = this.form.get('amount')?.value;
    this.transaction.date = new Date(this.form.get('date')?.value);
    if (repeatSettings.get('repeatFrequency')?.value) {
      this.transaction.repeatSettings = {
        type: Number.parseInt(repeatSettings.get('repeatFrequency')?.value),
        interval: repeatSettings.get('interval')?.value,
        startDate: new Date(this.form.get('date')?.value),
        endDate: new Date(repeatSettings.get('endDate')?.value),
      }
    }

    if (this.transaction.amount > 0 && this.transaction.type != TransactionType.Income) {
      this.transaction.amount = this.transaction.amount * -1;
    }
  }
}

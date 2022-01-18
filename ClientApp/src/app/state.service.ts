import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject, Subject } from "rxjs";
import { RepeatContract } from "./Shared/Interfaces/RepeatContract";
import { Month } from "./Shared/Interfaces/Month";
import { CurrentState, ModelWindow } from "./Shared/Interfaces/CurrentState";
import { Transaction } from "./Shared/Interfaces/Transaction";

@Injectable({
  providedIn: 'root'
})
export class StateService {
  public currentState: BehaviorSubject<CurrentState> = new BehaviorSubject<CurrentState>({} as CurrentState);
  public editingRepeatSettings: Subject<RepeatContract> = new ReplaySubject();
  public month: BehaviorSubject<Month> = new BehaviorSubject<Month>({} as Month);
  public editingTransaction: Subject<Transaction> = new ReplaySubject();
  public multiTransactionDate: Subject<Date> = new ReplaySubject();

  constructor() { }

  public EditRepeatSetting(setting: RepeatContract): void{
    const state = this.currentState.getValue();
    state.modalWindow = ModelWindow.RepeatSettings;
    this.currentState.next(state);
    this.editingRepeatSettings.next(setting);
  }

  public EditTransaction(transaction: Transaction): void {
    const state = this.currentState.getValue();
    state.modalWindow = ModelWindow.Transaction;
    this.currentState.next(state);
    this.editingTransaction.next(transaction);
  }

  public EditMultiTransaction(date: Date | undefined): void {
    const state = this.currentState.getValue();
    state.modalWindow = ModelWindow.MultiTransaction;
    this.currentState.next(state);
    this.multiTransactionDate.next(date);
  }

  public NullRepeatSetting(): void {
    this.editingRepeatSettings.next(undefined);
  }

  public UpdateMonth(month: Month): void {
    this.month.next(month);
  }

  public ExitAllModals(): void {
    const state = this.currentState.getValue();
    state.modalWindow = ModelWindow.None;
    this.currentState.next(state);
  }

  public UpdateViewDate(date: Date): void {
    console.log(date)
    const state = this.currentState.getValue();
    state.currentViewDate = date;
    this.currentState.next(state);
  }
}

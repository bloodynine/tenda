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
    const newState = {modalWindow: ModelWindow.RepeatSettings } as CurrentState;
    this.currentState.next(newState);
    this.editingRepeatSettings.next(setting);
  }

  public EditTransaction(transaction: Transaction): void {
    const newState = {modalWindow: ModelWindow.Transaction } as CurrentState;
    this.currentState.next(newState);
    this.editingTransaction.next(transaction);
  }

  public EditMultiTransaction(date: Date | undefined): void {
    const newState = {modalWindow: ModelWindow.MultiTransaction } as CurrentState;
    this.currentState.next(newState);
    this.multiTransactionDate.next(date);
  }

  public NullRepeatSetting(): void {
    this.editingRepeatSettings.next(undefined);
  }

  public UpdateMonth(month: Month): void {
    this.month.next(month);
  }

  public ExitAllModals(): void {
    this.currentState.next({} as CurrentState);
  }
}

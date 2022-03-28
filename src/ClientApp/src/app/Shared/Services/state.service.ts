import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject, Subject } from "rxjs";
import { RepeatContract } from "../Interfaces/RepeatContract";
import { Month } from "../Interfaces/Month";
import { CurrentState, ModelWindow } from "../Interfaces/CurrentState";
import { Transaction } from "../Interfaces/Transaction";

@Injectable({
  providedIn: 'root'
})
export class StateService {
  public currentState: BehaviorSubject<CurrentState> = new BehaviorSubject<CurrentState>({ modalWindow: ModelWindow.None } as CurrentState);
  public editingRepeatSettings: Subject<RepeatContract> = new ReplaySubject();
  public month: BehaviorSubject<Month> = new BehaviorSubject<Month>({} as Month);
  public editingTransaction: Subject<Transaction> = new ReplaySubject();
  public multiTransactionDate: Subject<Date> = new ReplaySubject();
  public selectedDate: BehaviorSubject<Date> = new BehaviorSubject<Date>(new Date());

  constructor() {
  }

  public EditRepeatSetting(setting: RepeatContract): void {
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
  
  public EditAdminSettings(): void {
    const state = this.currentState.getValue();
    state.modalWindow = ModelWindow.Admin;
    this.currentState.next(state);
  }

  public AddErrorMsg(message: string): void {
    const state = this.currentState.getValue();
    state.notificationMsg = message;
    state.notificationClass = "is-danger";
    this.currentState.next(state);
  }

  public ClearNotifications(): void {
    const state = this.currentState.getValue();
    state.notificationMsg = null;
    state.notificationClass = "is-primary";
    this.currentState.next(state);
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
    const state = this.currentState.getValue();
    this.selectedDate.next(date);
    state.currentViewDate = date;
    this.currentState.next(state);
  }
}

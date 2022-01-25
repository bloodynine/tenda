export interface CurrentState {
  modalWindow: ModelWindow,
  currentViewDate: Date,
  notificationMsg: string | null,
  notificationClass: string,
  displayQuickSearch: boolean;
}

export enum ModelWindow {
  None,
  Transaction,
  RepeatSettings,
  MultiTransaction
}

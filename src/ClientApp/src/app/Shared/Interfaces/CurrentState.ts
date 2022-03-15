export interface CurrentState {
  modalWindow: ModelWindow,
  currentViewDate: Date,
  notificationMsg: string | null,
  notificationClass: string,
}

export enum ModelWindow {
  None,
  Transaction,
  RepeatSettings,
  MultiTransaction,
  Admin,
}

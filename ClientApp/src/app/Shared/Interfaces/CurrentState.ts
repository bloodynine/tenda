export interface CurrentState {
  modalWindow: ModelWindow,
  currentViewDate: Date,
}

export enum ModelWindow {
  None,
  Transaction,
  RepeatSettings,
  MultiTransaction
}

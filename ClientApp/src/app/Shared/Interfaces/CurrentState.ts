export interface CurrentState {
  modalWindow: ModelWindow
}

export enum ModelWindow {
  None,
  Transaction,
  RepeatSettings,
  MultiTransaction
}

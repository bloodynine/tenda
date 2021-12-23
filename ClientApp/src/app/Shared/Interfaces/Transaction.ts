import {RepeatSettings} from "./RepeatSettings";

export interface Transaction {
  id: string;
  created: Date;
  updated: Date;
  name: string;
  amount: number;
  isResolved: boolean;
  type: TransactionType;
  date: Date;
  isRepeating: boolean;
  repeatSettings: RepeatSettings | null;
  associatedRepeatId: string;
  tags: string[];
}

export enum TransactionType {
  OneOff = 1,
  Bill = 2,
  Income = 3
}

export const TransactionTypeLabelMapping: Record<TransactionType, string> = {
  [TransactionType.OneOff]: "Transaction",
  [TransactionType.Bill]: "Bill",
  [TransactionType.Income]: "Income"
}

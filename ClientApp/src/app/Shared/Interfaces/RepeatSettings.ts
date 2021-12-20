import {TransactionType} from "./Transaction";

export interface RepeatSettings {
  startDate: Date;
  interval: number;
  type: RepeatType
}

export enum RepeatType {
  None,
  ByMonth,
  ByWeek,
  ByDay
}
export const RepeatTypeLabel: Record<RepeatType, string> = {
  [RepeatType.None]: "",
  [RepeatType.ByMonth]: "Monthly",
  [RepeatType.ByDay]: "Daily",
  [RepeatType.ByWeek]: "Weekly",
}

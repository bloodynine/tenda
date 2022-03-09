import {TransactionType} from "./Transaction";

export interface RepeatSettings {
  startDate: Date;
  interval: number;
  type: RepeatType | null;
  endDate: Date | null;
}

export enum RepeatType {
  // None,
  ByMonth = 1,
  ByWeek = 2,
  ByDay =3
}
export const RepeatTypeLabel: Record<RepeatType, string> = {
  // [RepeatType.None]: "",
  [RepeatType.ByMonth]: "Monthly",
  [RepeatType.ByDay]: "Daily",
  [RepeatType.ByWeek]: "Weekly",
}

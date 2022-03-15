import {TransactionType} from "./Transaction";
import {RepeatType} from "./RepeatSettings";

export interface RepeatContract {
  id: string;
  name: string;
  amount: number;
  interval: number;
  type: TransactionType;
  repeatType: RepeatType;
  startDate: Date;
  endDate: Date | null;
  tags: string[];
}

import {Transaction} from "./Transaction";

export interface Day {
  date: Date;
  oneOffs: Array<Transaction>;
  bills: Array<Transaction>;
  incomes: Array<Transaction>;
  runningTotal: number;
}

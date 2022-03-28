import { LabelNumber } from "./ByTagReport";

export interface ByMonthReport {
    averageIncome: number;
    averageExpenses: number;
    incomeExpensesByMonth: GroupedItem[]
}

export interface GroupedItem {
    name: string;
    series: LabelNumber[];
}
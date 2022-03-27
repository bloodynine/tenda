import { Component, OnInit } from '@angular/core';
import { ByTagReport } from "../Models/ByTagReport";
import { ReportService } from "../report.service";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { TransactionType } from "../../Shared/Interfaces/Transaction";

@Component({
  selector: 'app-by-tag-report',
  templateUrl: './by-tag-report.component.html',
  styleUrls: ['./by-tag-report.component.css']
})
export class ByTagReportComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  report: ByTagReport | undefined;
  constructor(
      private reportService: ReportService,
      private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    let endDate = new Date()
    endDate.setMonth(endDate.getMonth() + 1)
    this.form = this.fb.group({
      startDate: new FormControl(new Date()),
      endDate: new FormControl(endDate),
      transactions: new FormControl(true),
      bills: new FormControl(true),
      incomes: new FormControl(true),
    })
    
    this.reportService.getByTagReportValues(new Date(), endDate, [TransactionType.Income, TransactionType.Bill, TransactionType.OneOff]).then(x => this.report = x)
    this.onChanges();
  }
  
  onChanges(): void {
    this.form.valueChanges.subscribe(_ => this.fetch());
  }
  
  fetch(): void {
    const startDate = new Date(this.form.get('startDate')?.value)
    const endDate = new Date(this.form.get('endDate')?.value)
    this.reportService.getByTagReportValues(startDate, endDate, this.getTypeFilterArray()).then(x => this.report = x)
  }
  
  getTypeFilterArray(): TransactionType[] {
    let res = [];
    if(this.form.get('transactions')?.value){
      res.push(TransactionType.OneOff)
    }
    if(this.form.get('bills')?.value){
      res.push(TransactionType.Bill)
    }
    if(this.form.get('incomes')?.value){
      res.push(TransactionType.Income)
    }
    return res;
  }

}

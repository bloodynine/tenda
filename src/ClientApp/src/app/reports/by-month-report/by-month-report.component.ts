import { Component, OnInit } from '@angular/core';
import { ReportService } from "../report.service";
import { ByMonthReport } from "../Models/ByMonthReport";
import { ChartTypes } from "../Models/ChartTypes";
import { Form, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { ReportSettings } from "../Models/ReportSettings";
import { first } from "rxjs/operators";

@Component({
  selector: 'app-by-month-report',
  templateUrl: './by-month-report.component.html',
  styleUrls: ['./by-month-report.component.css']
})
export class ByMonthReportComponent implements OnInit {
  report: ByMonthReport = {} as ByMonthReport;
  groupedBarChart: ChartTypes = ChartTypes.GroupedBarChart;
  form: FormGroup = new FormGroup({});
  reportSettings: ReportSettings | undefined;
  currentYear: number = new Date().getFullYear();
  
  constructor(
      private reportService: ReportService,
      private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      year: new FormControl(this.currentYear)
    })
    this.reportService.reportSettings.pipe(first()).subscribe(x => this.reportSettings = x);
    this.fetch();
    this.onChanges();
  }
  
  onChanges(): void {
    this.form.valueChanges.subscribe(_ => this.fetch())
  }

  fetch() : void {
    const selectedYear = this.form.get('year')?.value
    this.reportService.getByMonthReportValues(selectedYear).then(x => this.report = x)
  }
}

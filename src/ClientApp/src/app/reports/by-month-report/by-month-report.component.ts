import { Component, OnInit } from '@angular/core';
import { ReportService } from "../report.service";
import { ByMonthReport } from "../Models/ByMonthReport";

@Component({
  selector: 'app-by-month-report',
  templateUrl: './by-month-report.component.html',
  styleUrls: ['./by-month-report.component.css']
})
export class ByMonthReportComponent implements OnInit {
  report: ByMonthReport = {} as ByMonthReport;
  constructor(
      private reportService: ReportService
  ) { }

  ngOnInit(): void {
    this.reportService.getByMonthReportValues(2022).then(x => this.report = x);
    
  }

}

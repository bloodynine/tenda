import { Component, OnInit } from '@angular/core';
import { ByTagReport } from "../Models/ByTagReport";
import { ReportService } from "../report.service";

@Component({
  selector: 'app-by-tag-report',
  templateUrl: './by-tag-report.component.html',
  styleUrls: ['./by-tag-report.component.css']
})
export class ByTagReportComponent implements OnInit {
  saleData = [
    { name: "Mobiles", value: 105000 },
    { name: "Laptop", value: 55000 },
    { name: "AC", value: 15000 },
    { name: "Headset", value: 150000 },
    { name: "Fridge", value: 20000 }
  ];
  report: ByTagReport | undefined;
  constructor(
      private reportService: ReportService
  ) { }

  ngOnInit(): void {
    this.reportService.getByTagReportValues().then(x => this.report = x)
  }

}

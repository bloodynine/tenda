import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from "@angular/router";
import { ReportWrapperComponent } from "./report-wrapper/report-wrapper.component";
import { ByTagReportComponent } from "./by-tag-report/by-tag-report.component";
import { ByMonthReportComponent } from "./by-month-report/by-month-report.component";


const routes: Routes = [
  {
    path: '', component: ReportWrapperComponent, data: {reportName: 'root'}
  },
  {
    path: 'tags', component: ReportWrapperComponent, data: {reportName: 'tags'},
    children: [
      {path: '', component: ByTagReportComponent}
    ]
  },
  {
    path: 'month', component: ReportWrapperComponent, data: {reportName: 'month'},
    children: [
      {path: '', component: ByMonthReportComponent}
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
      RouterModule
  ]
})
export class ReportRoutingModule { }

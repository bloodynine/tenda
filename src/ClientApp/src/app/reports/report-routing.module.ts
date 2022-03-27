import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from "@angular/router";
import { ReportWrapperComponent } from "./report-wrapper/report-wrapper.component";
import { ByTagReportComponent } from "./by-tag-report/by-tag-report.component";
import { ProjectionReportComponent } from "./projection-report/projection-report.component";


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
    path: 'projection', component: ReportWrapperComponent, data: {reportName: 'projection'},
    children: [
      {path: '', component: ProjectionReportComponent}
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

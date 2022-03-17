import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportWrapperComponent } from './report-wrapper/report-wrapper.component';
import { ReportRoutingModule } from './report-routing.module';
import { ByTagReportComponent } from './by-tag-report/by-tag-report.component';
import { NgxChartsModule } from "@swimlane/ngx-charts";



@NgModule({
  declarations: [
    ReportWrapperComponent,
    ByTagReportComponent
  ],
  imports: [
    CommonModule,
    ReportRoutingModule,
    NgxChartsModule
  ],
})
export class ReportsModule { }

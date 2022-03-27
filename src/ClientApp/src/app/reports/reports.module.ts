import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportWrapperComponent } from './report-wrapper/report-wrapper.component';
import { ReportRoutingModule } from './report-routing.module';
import { ByTagReportComponent } from './by-tag-report/by-tag-report.component';
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { SharedModule } from "../Shared/shared.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { ProjectionReportComponent } from './projection-report/projection-report.component';



@NgModule({
  declarations: [
    ReportWrapperComponent,
    ByTagReportComponent,
    ProjectionReportComponent
  ],
    imports: [
        CommonModule,
        ReportRoutingModule,
        NgxChartsModule,
        SharedModule,
        FormsModule,
        ReactiveFormsModule
    ],
})
export class ReportsModule { }

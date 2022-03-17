import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from "@angular/router";
import { ReportWrapperComponent } from "./report-wrapper/report-wrapper.component";


const routes: Routes = [
  {path: '', component: ReportWrapperComponent}
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

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from "./login/login.component";
import { MonthComponent } from "./month/month.component";

const routes: Routes = [
  { path: '', component: LoginComponent, data: {disableMenu: true}},
  { path: 'login', component: LoginComponent, data: {disableMenu: true} },
  { path: 'month', component: MonthComponent },
  { path: 'year/:year/month/:month', component: MonthComponent },
  { path: 'year/:year/month/:month/day/:day', component: MonthComponent },
  { path: 'reports', loadChildren: () => import('./reports/reports.module').then(m => m.ReportsModule), data: {disableTransactionMenu: true} }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}

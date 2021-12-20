import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {TestComponent} from "./test/test.component";
import {LoginComponent} from "./login/login.component";
import {MonthComponent} from "./month/month.component";

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'login', component: LoginComponent},
  {path: 'month', component: MonthComponent},
  {path: 'test', component: TestComponent},
  {path: 'year/:year/month/:month', component: MonthComponent},
  {path: 'year/:year/month/:month/day/:day', component: MonthComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}

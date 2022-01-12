import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from "@angular/common/http";
import {CookieService} from "ngx-cookie-service";
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MonthComponent } from './month/month.component';
import {TokenInterceptor} from "./token.interceptor";
import { DayComponent } from './day/day.component';
import { TransactionComponent } from './transaction/transaction.component';
import {IconsModule} from "./icons/icons.module";
import { TransactionFormComponent } from './transaction-form/transaction-form.component';
import { RepeatFormComponent } from './repeat-form/repeat-form.component';
import { DateSelectorComponent } from './date-selector/date-selector.component';
import { MultiTransactionFormComponent } from './multi-transaction-form/multi-transaction-form.component';
import { TagInputComponent } from './Shared/tag-input/tag-input.component';
import { FocusDirective } from "./Shared/Directives/focus.directive";
import { RepeatTypeInputComponent } from './Shared/repeat-type-input/repeat-type-input.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MonthComponent,
    DayComponent,
    TransactionComponent,
    TransactionFormComponent,
    RepeatFormComponent,
    DateSelectorComponent,
    MultiTransactionFormComponent,
    FocusDirective,
    TagInputComponent,
    RepeatTypeInputComponent
  ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        ReactiveFormsModule,
        IconsModule,
        FormsModule
    ],
  providers: [ CookieService, {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }

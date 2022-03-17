import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { DateSelectorComponent } from './Shared/date-selector/date-selector.component';
import { DayComponent } from './day/day.component';
import { FocusDirective } from "./Shared/Directives/focus.directive";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { Injector, NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { MonthComponent } from './month/month.component';
import { MultiTransactionFormComponent } from './multi-transaction-form/multi-transaction-form.component';
import { NotificationsComponent } from './Shared/notifications/notifications.component';
import { RepeatFormComponent } from './repeat-form/repeat-form.component';
import { RepeatSettingsComponent } from './Shared/repeat-settings/repeat-settings.component';
import { RepeatTypeInputComponent } from './Shared/repeat-type-input/repeat-type-input.component';
import { TagInputComponent } from './Shared/tag-input/tag-input.component';
import { TransactionComponent } from './transaction/transaction.component';
import { TransactionFormComponent } from './transaction-form/transaction-form.component';
import { TransactionSearchComponent } from './transaction-search/transaction-search.component';
import { CookieService } from "ngx-cookie-service";
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule} from "@angular/common/http";
import { IconsModule } from "./Shared/icons/icons.module";
import { TokenInterceptor } from "./token.interceptor";
import { AppConfigModule } from "./app-config/app-config.module";
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

export let InjectorInstance: Injector;

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
    RepeatTypeInputComponent,
    NotificationsComponent,
    TransactionSearchComponent,
    RepeatSettingsComponent,
    AdminPanelComponent,
  ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        ReactiveFormsModule,
        IconsModule,
        FormsModule,
        AppConfigModule,
        BrowserAnimationsModule
    ],
  providers: [ CookieService, {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(private injector: Injector) {
    InjectorInstance = this.injector;
  }
}

import { APP_INITIALIZER, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StartupService } from "../Shared/Services/startup.service";
import { HttpClientModule } from "@angular/common/http";


export function init_app(startService: StartupService) {
  return () => startService.getSettings();
}

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    StartupService,
    {provide: APP_INITIALIZER, useFactory: init_app, deps: [StartupService], multi: true}
  ]
})
export class AppConfigModule { }

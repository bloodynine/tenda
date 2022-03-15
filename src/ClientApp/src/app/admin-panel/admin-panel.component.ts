import { Component, OnInit } from '@angular/core';
import { StartupService } from "../Shared/Services/startup.service";
import { first } from "rxjs/operators";
import { ServerSettings } from "../Shared/Interfaces/ServerSettings";
import { StateService } from "../Shared/Services/state.service";

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  settings: ServerSettings = {} as ServerSettings;

  constructor(private startupService: StartupService,
              private state: StateService) { }

  ngOnInit(): void {
    this.startupService.settings.pipe(first()).subscribe(x => this.settings = x)
    
  }

  toggleSignUps(): void {
    this.settings.allowSignUps = !this.settings.allowSignUps;
  }
  
  cancel() {
    this.state.ExitAllModals();
  }
  
  save() {
    this.startupService.saveSettings(this.settings);
    this.state.ExitAllModals();
  }
}

import { Component, OnInit } from '@angular/core';
import { StateService } from "../../state.service";

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  showNotification: boolean = false;
  notificationClass: string = "is-primary"
  message: string = "";
  constructor(
    private stateService: StateService
  ) { }

  ngOnInit(): void {
    this.stateService.currentState.subscribe(x => {
      if(x.notificationMsg){
        this.showNotification = true;
        this.notificationClass = x.notificationClass;
        this.message = x.notificationMsg;
      } else{
        this.showNotification = false;
      }
    })
  }

  hideNotification(): void {
    this.stateService.ClearNotifications();
  }

}

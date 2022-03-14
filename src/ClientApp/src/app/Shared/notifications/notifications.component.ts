import { Component, OnInit } from '@angular/core';
import { StateService } from "../Services/state.service";
import { BehaviorSubject } from "rxjs";
import { delay, take } from "rxjs/operators";

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  showNotification: boolean = false;
  notificationClass: string = "is-primary"
  message: string = "";
  waitMe: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(
    private stateService: StateService
  ) {
  }

  ngOnInit(): void {
    this.stateService.currentState.subscribe(x => {
      if (x.notificationMsg) {
        this.showNotification = true;
        this.notificationClass = x.notificationClass;
        this.message = x.notificationMsg;
        this.waitMe.pipe(delay(8000), take(1)).subscribe(x => this.stateService.ClearNotifications())
      } else {
        this.showNotification = false;
      }
    })
  }

  hideNotification(): void {
    this.stateService.ClearNotifications();
  }

}

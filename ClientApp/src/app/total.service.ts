import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {HubConnection} from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class TotalService {
  private connection: HubConnection;
  public total: BehaviorSubject<Seed> = new BehaviorSubject<Seed>({amount: 0} as Seed)
  constructor() { }

  public Init(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.connection.con
    })
  }
}

import { Injectable } from '@angular/core';
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ServerSettings } from "../Interfaces/ServerSettings";
import { ReplaySubject, Subject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StartupService {

  baseUrl: string = environment.apiUrl;
  public settings: Subject<ServerSettings> = new ReplaySubject();

  constructor(
    private http: HttpClient
  ) { }

  public getSettings(): void{
    this.http.get<ServerSettings>(`${this.baseUrl}/settings/server`).toPromise().then(x=> {
      this.settings.next(x)
    });
  }
}

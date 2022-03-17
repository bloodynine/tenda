import { Injectable } from '@angular/core';
import { Month } from "../Interfaces/Month";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { HandleHttpError } from "./handle-error.service";
import { StateService } from "./state.service";

@Injectable({
  providedIn: 'root'
})
export class MonthService {
  baseUrl: string = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private state: StateService
  ) { }

  public GetMonth(year: number, month: number): Observable<Month>{
    return this.http.get<Month>(`${this.baseUrl}/month/${month}/year/${year}`).pipe(HandleHttpError());
  }
}

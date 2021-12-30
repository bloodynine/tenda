import { Injectable } from '@angular/core';
import { Month } from "./Shared/Interfaces/Month";
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class MonthService {
  baseUrl: string = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) { }

  public GetMonth(year: number, month: number): Observable<Month>{
    console.log('get month')
    return this.http.get<Month>(`${this.baseUrl}/month/${month}/year/${year}`);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { ByTagReport } from "./Models/ByTagReport";
import { HandleHttpError } from "../Shared/Services/handle-error.service";
import { TransactionType } from "../Shared/Interfaces/Transaction";

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  baseUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }
  
  public getByTagReportValues(startDate: Date, endDate: Date, typeFilter: Array<TransactionType>): Promise<ByTagReport>{
    
    return this.http.post<ByTagReport>(`${this.baseUrl}/reports/GetByTagReport`, {
      startDate: startDate,
      endDate: endDate,
      types: typeFilter
    }).pipe(HandleHttpError()).toPromise();
  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { ByTagReport } from "./Models/ByTagReport";
import { HandleHttpError } from "../Shared/Services/handle-error.service";
import { TransactionType } from "../Shared/Interfaces/Transaction";
import { ByMonthReport } from "./Models/ByMonthReport";
import { BehaviorSubject, ReplaySubject } from "rxjs";
import { ReportSettings } from "./Models/ReportSettings";

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  baseUrl: string = environment.apiUrl;
  reportSettings: ReplaySubject<ReportSettings> = new ReplaySubject<ReportSettings>();
  constructor(private http: HttpClient) {
    this.http.get<ReportSettings>(`${this.baseUrl}/reports/settings`).pipe(HandleHttpError()).toPromise().then(x => this.reportSettings.next(x));
  }
  
  public getByTagReportValues(startDate: Date, endDate: Date, typeFilter: Array<TransactionType>): Promise<ByTagReport>{
    
    return this.http.post<ByTagReport>(`${this.baseUrl}/reports/GetByTagReport`, {
      startDate: startDate,
      endDate: endDate,
      types: typeFilter
    }).pipe(HandleHttpError()).toPromise();
  }
  
  public getByMonthReportValues(year: number): Promise<ByMonthReport>{
    const params = new HttpParams().set('year', year);
    return this.http.get<ByMonthReport>(`${this.baseUrl}/reports/GetByMonthReport`, {params: params})
    .pipe(HandleHttpError()).toPromise();
  }
}

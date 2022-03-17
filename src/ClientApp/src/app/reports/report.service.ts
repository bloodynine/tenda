import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { ByTagReport } from "./Models/ByTagReport";

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  baseUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) { }
  
  public getByTagReportValues(): Promise<ByTagReport>{
    return this.http.get<ByTagReport>(`${this.baseUrl}/reports/GetByTagReport`).toPromise();
  }
}

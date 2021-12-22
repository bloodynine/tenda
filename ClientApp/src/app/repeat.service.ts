import { Injectable } from '@angular/core';
import {environment} from "../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {RepeatContract} from "./Shared/Interfaces/RepeatContract";
import {Month} from "./Shared/Interfaces/Month";

@Injectable({
  providedIn: 'root'
})
export class RepeatService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public GetRepeatContract(id: string): Promise<RepeatContract> {
    return this.http.get<RepeatContract>(`${this.baseUrl}/repeats/${id}`).toPromise();
  }

  public UpdateRepeatContract(id: string, contract: RepeatContract): Promise<Month> {
    const params = new HttpParams().set('CurrentViewDate', new Date(contract.startDate).toDateString());
    return this.http.put<Month>(`${this.baseUrl}/repeats/${contract.id}`, contract, {params: params}).toPromise();
  }


}

import { Injectable } from '@angular/core';
import { environment } from "../../../environments/environment";
import { HttpClient, HttpParams } from "@angular/common/http";
import { RepeatContract } from "../Interfaces/RepeatContract";
import { Month } from "../Interfaces/Month";
import { StateService } from "./state.service";
import { HandleHttpError } from "./handle-error.service";

@Injectable({
  providedIn: 'root'
})
export class RepeatService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient,
              private stateService: StateService) {
  }

  public GetRepeatContract(id: string): Promise<RepeatContract> {
    return this.http.get<RepeatContract>(`${this.baseUrl}/repeats/${id}`)
      .pipe(HandleHttpError())
      .toPromise();
  }

  public UpdateRepeatContract(id: string, contract: RepeatContract): Promise<Month> {
    const params = new HttpParams().set('CurrentViewDate', new Date(contract.startDate).toDateString());
    return this.http.put<Month>(`${this.baseUrl}/repeats/${contract.id}`, contract, { params: params })
      .pipe(HandleHttpError())
      .toPromise();
  }

  public DeleteRepeatContract(id: string): Promise<Month> {
    const state = this.stateService.currentState.getValue();
    const params = new HttpParams().set('ViewDate', state.currentViewDate.toDateString());
    return this.http.delete<Month>(`${this.baseUrl}/repeats/${id}`, { params: params })
      .pipe(HandleHttpError())
      .toPromise();
  }


}

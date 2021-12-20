import { Injectable } from '@angular/core';
import {environment} from "../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {RepeatContract} from "./Shared/Interfaces/RepeatContract";
import {Observable} from "rxjs";
import {Month} from "./Shared/Interfaces/Month";
import {TransactionService} from "./transaction.service";
import {StateService} from "./state.service";

@Injectable({
  providedIn: 'root'
})
export class RepeatService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient,
  private transactionService: TransactionService,
  private  stateService: StateService) { }

  public GetRepeatContract(id: string): void {
    this.http.get<RepeatContract>(`${this.baseUrl}/repeats/${id}`).toPromise()
      .then(x => this.stateService.EditRepeatSetting(x));
  }

  public UpdateRepeatContract(id: string, contract: RepeatContract): void {
    const params = new HttpParams().set('CurrentViewDate', new Date(contract.startDate).toDateString());
    this.http.put<Month>(`${this.baseUrl}/repeats/${contract.id}`, contract, {params: params}).toPromise().then(x => {
      this.transactionService.Month.next(x);
      this.stateService.editingRepeatSettings.next(undefined);
      this.transactionService.malleableTransaction.next(undefined);
    })
  }


}

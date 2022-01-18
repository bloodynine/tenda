import {Injectable} from '@angular/core';
import {environment} from "../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import { BehaviorSubject, Observable, ReplaySubject, Subject } from "rxjs";
import {Month} from "./Shared/Interfaces/Month";
import {BearerToken} from "./Shared/Interfaces/BearerToken";
import {HubConnectionBuilder, IHttpConnectionOptions, LogLevel} from "@microsoft/signalr";
import {Transaction, TransactionType} from "./Shared/Interfaces/Transaction";
import { catchError } from "rxjs/operators";
import { StateService } from "./state.service";
import { TendaError, ValidationError } from "./Shared/Interfaces/ValidationError";
import { HandleHttpError } from "./Shared/Services/handle-error.service";

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  baseUrl: string = environment.apiUrl;

  public total: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  public malleableRepeatSettingId: Subject<string> = new ReplaySubject<string>();
  public malleableTransaction: Subject<Transaction> = new ReplaySubject<Transaction>();

  options: IHttpConnectionOptions = {
    accessTokenFactory(): string | Promise<string> {
      return TransactionService.GetBearerToken() ?? '';
    }
  }
  signalRConnection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Error)
    .withUrl(`${this.baseUrl}/resolvedTotal`, this.options)
    .build();
  constructor(private http: HttpClient,
              private stateService: StateService) { }

  public SubscribeToSignalR(): void {
    this.signalRConnection.start().then(x => {
    });

    this.signalRConnection.on("ResolvedTotal", (x: Seed) => {
      this.total.next(x.amount);
    });
  }

  public UnSubscribeToSignalR(): void {
    this.signalRConnection.stop().then(x => {});
  }

  public UpdateTransaction(transaction: Transaction): Promise<Month> {
    return this.http.put<Month>(`${this.baseUrl}${TransactionService.GetTransactionUri(transaction)}/${transaction.id}`, transaction)
      .pipe(HandleHttpError())
      .toPromise();
  }

  public CreateNewTransaction(transaction: Transaction): Promise<Month> {
    return this.http.post<Month>(`${this.baseUrl}${TransactionService.GetTransactionUri(transaction)}`, transaction)
      .pipe(HandleHttpError())
      .toPromise();
  }

  public DeleteTransaction(transaction: Transaction): Promise<Month> {
    const params = new HttpParams().set('ViewDate', transaction.date.toString());
    return this.http.delete<Month>(`${this.baseUrl}${TransactionService.GetTransactionUri(transaction)}/${transaction.id}/`, {params:params})
      .pipe(HandleHttpError())
      .toPromise();
  }

  public CreateBulkTransactions(transactions: Transaction[]): Promise<Month>{
    const body = {oneOffs: transactions};
    return this.http.post<Month>(`${this.baseUrl}/oneOffs/bulk`, body)
      .pipe(HandleHttpError())
      .toPromise();
  }

  public GetTags(): Promise<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/tags`).toPromise();
  }

  private static GetTransactionUri(transaction: Transaction): string {
    switch (TransactionType[transaction.type]) {
      case 'Income':
        return "/incomes"
      case 'Bill':
        return "/bills"
      default:
        return "/oneOffs"
    }
  }
  private static GetBearerToken(): string | undefined {
    const tokenString = localStorage.getItem('bearerToken');
    if (tokenString) {
      let bearerToken: BearerToken;
      bearerToken = JSON.parse(tokenString);
      return bearerToken.token;
    }
    return undefined;
  }


}

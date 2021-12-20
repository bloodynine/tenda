import {Injectable} from '@angular/core';
import {environment} from "../environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {BehaviorSubject, ReplaySubject, Subject} from "rxjs";
import {Month} from "./Shared/Interfaces/Month";
import {BearerToken} from "./Shared/Interfaces/BearerToken";
import {HubConnectionBuilder, IHttpConnectionOptions, LogLevel} from "@microsoft/signalr";
import {Transaction, TransactionType} from "./Shared/Interfaces/Transaction";

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  baseUrl: string = environment.apiUrl;
  public Month: BehaviorSubject<Month> = new BehaviorSubject<Month>({} as Month);
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
  constructor(private http: HttpClient) { }

  public GetMonth(year: number, month: number): void{
    this.http.get<Month>(`${this.baseUrl}/month/${month}/year/${year}`).subscribe(x => this.Month.next(x)
    );
  }

  public SubscribeToSignalR(): void {
    this.signalRConnection.start().then(x => {
    });

    this.signalRConnection.on("Foo", (x: Seed) => {
      this.total.next(x.amount);
    });
  }

  public UpdateTransaction(transaction: Transaction): void {
    this.http.put<Month>(`${this.baseUrl}${TransactionService.GetTransactionUri(transaction)}/${transaction.id}`, transaction)
      .toPromise()
      .then(x => {
        this.Month.next(x);
        this.malleableTransaction.next(undefined);
      });
  }

  public CreateNewTransaction(transaction: Transaction): void {
    this.http.post<Month>(`${this.baseUrl}${TransactionService.GetTransactionUri(transaction)}`, transaction).toPromise()
      .then(x => {
        this.Month.next(x);
        this.malleableTransaction.next(undefined)
      });
  }

  public DeleteTransaction(transaction: Transaction): void {
    const params = new HttpParams().set('ViewDate', transaction.date.toString());
    this.http.delete<Month>(`${this.baseUrl}${TransactionService.GetTransactionUri(transaction)}/${transaction.id}/`, {params:params}).toPromise().then(x => {
      this.Month.next(x);
      this.malleableTransaction.next(undefined);
    })
  }

  public EditRepeatSetting(repeatId: string): void {
    this.malleableRepeatSettingId.next(repeatId);
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

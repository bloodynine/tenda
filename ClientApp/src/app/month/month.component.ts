import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Month} from "../Shared/Interfaces/Month";
import {TransactionService} from "../transaction.service";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import {Transaction} from "../Shared/Interfaces/Transaction";
import {RepeatService} from "../repeat.service";
import {StateService} from "../state.service";
import {RepeatContract} from "../Shared/Interfaces/RepeatContract";
import { BehaviorSubject } from "rxjs";
import { CurrentState, ModelWindow } from "../Shared/Interfaces/CurrentState";
import { MonthService } from "../month.service";
import { LoginService } from "../Shared/Services/login.service";
import { Day } from "../Shared/Interfaces/Day";

@Component({
  selector: 'app-month',
  templateUrl: './month.component.html',
  styleUrls: ['./month.component.css']
})

export class MonthComponent implements OnInit {
  month: Month | undefined;
  total: number = 0;
  currentState: CurrentState = {} as CurrentState;
  monthlyTransactions: Transaction[] = [];
  public modalWindow = ModelWindow;

  selectedDate: Date = new Date();
  isMenuActive: boolean = false;

  constructor(
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private changeDectectorRef: ChangeDetectorRef,
    private repeatService: RepeatService,
    private stateService: StateService,
    private router: Router,
    private monthService: MonthService,
    private loginSerivce: LoginService
  ) { }

  ngOnInit(): void {
    this.stateService.currentState.subscribe(x => this.currentState = x);
    this.stateService.month.subscribe(x => {
      this.month = x;
      if(x.days){
        x.days.forEach(x => {
          this.addDaysTransactionToMonthlyList(x);
        });
      }
    });
    this.transactionService.SubscribeToSignalR();
    this.transactionService.total.subscribe(x => {
      if(x != 0){
        this.total = x;
        this.changeDectectorRef.detectChanges();
      }
    });
    this.route.paramMap.subscribe((params: ParamMap) => {
      const now = new Date();
      const month = Number.parseInt(params.get('month') ?? now.getMonth().toString());
      const year = Number.parseInt(params.get('year') ?? now.getFullYear().toString());
      this.selectedDate = new Date(year, month - 1);
      this.stateService.UpdateViewDate(this.selectedDate);
      this.monthService.GetMonth(year, month).subscribe(x => {
        this.stateService.UpdateMonth(x)});
    });
  }

  exitModal(): void {
    this.stateService.ExitAllModals();
  }

  goToPreviousMonth() {
    this.selectedDate = new Date(this.selectedDate.setMonth(this.selectedDate.getMonth() - 1))
    this.navigateToDay();
  }

  goToNextMonth() {
    this.selectedDate = new Date(this.selectedDate.setMonth(this.selectedDate.getMonth() + 1))
    this.navigateToDay();
  }

  goToToday() {
    this.selectedDate = new Date();
    this.navigateToDay();
  }

  navigateToDay(){
    this.router.navigateByUrl(`year/${this.selectedDate.getFullYear()}/month/${this.selectedDate.getMonth() + 1}`)
  }

  toggleMenu(){
    this.isMenuActive = !this.isMenuActive
  }

  signOut() {
    this.loginSerivce.SignOut();
  }

  private addDaysTransactionToMonthlyList(day: Day){
    day.incomes.forEach(x => this.monthlyTransactions.push(x));
    day.bills.forEach(x => this.monthlyTransactions.push(x));
    day.oneOffs.forEach(x => this.monthlyTransactions.push(x));
  }

  scroll(id: string){
    let el = (<HTMLInputElement>document.getElementById(id));
    let elPosition = el.getBoundingClientRect().top;
    const offSet = 63;
    const position = elPosition + window.pageYOffset - offSet;
    window.scrollTo({top: position, behavior: "smooth"})
  }
}

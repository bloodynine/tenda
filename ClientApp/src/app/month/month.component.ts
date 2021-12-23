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

@Component({
  selector: 'app-month',
  templateUrl: './month.component.html',
  styleUrls: ['./month.component.css']
})

export class MonthComponent implements OnInit {
  month: Month | undefined;
  total: number = 0;
  malleableTransaction: Transaction | null = null;
  editingRepeatContract: RepeatContract  | null = null;
  scrollDate: BehaviorSubject<Date> = new BehaviorSubject<Date>(new Date());
  currentState: CurrentState = {} as CurrentState;
  public modalWindow = ModelWindow;

  selectedDate: Date = new Date();

  constructor(
    private transactionService: TransactionService,
    private route: ActivatedRoute,
    private changeDectectorRef: ChangeDetectorRef,
    private repeatService: RepeatService,
    private stateService: StateService,
    private router: Router,
    private monthService: MonthService
  ) { }

  ngOnInit(): void {
    this.transactionService.GetTags().then(x => console.log(x));
    this.stateService.currentState.subscribe(x => this.currentState = x);
    this.stateService.month.subscribe(x => {
      this.month = x;
      // this.total = x.resolvedTotal;
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

      this.monthService.GetMonth(year, month).then(x => this.stateService.UpdateMonth(x));
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
}

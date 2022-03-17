import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Month } from "../Shared/Interfaces/Month";
import { TransactionService } from "../Shared/Services/transaction.service";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { Transaction } from "../Shared/Interfaces/Transaction";
import { RepeatService } from "../Shared/Services/repeat.service";
import { StateService } from "../Shared/Services/state.service";
import { CurrentState, ModelWindow } from "../Shared/Interfaces/CurrentState";
import { MonthService } from "../Shared/Services/month.service";
import { LoginService } from "../Shared/Services/login.service";
import { Day } from "../Shared/Interfaces/Day";

@Component({
  selector: 'app-month',
  templateUrl: './month.component.html',
  styleUrls: ['./month.component.css']
})

export class MonthComponent implements OnInit {
  month: Month | undefined;
  currentState: CurrentState = {} as CurrentState;
  isAdmin = false;
  public modalWindow = ModelWindow;

  selectedDate: Date = new Date();

  constructor(
    private transactionService: TransactionService,
    private changeDectectorRef: ChangeDetectorRef,
    private repeatService: RepeatService,
    private stateService: StateService,
    private router: Router,
    private monthService: MonthService,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit(): void {
    this.stateService.currentState.subscribe(x => this.currentState = x);
    this.stateService.month.subscribe(x => {
      this.month = x;
    });
    this.route.paramMap.subscribe((params: ParamMap) => {
      const now = new Date();
      const month = Number.parseInt(params.get('month') ?? now.getMonth().toString());
      const year = Number.parseInt(params.get('year') ?? now.getFullYear().toString());
      this.selectedDate = new Date(year, month - 1);
      this.stateService.UpdateViewDate(this.selectedDate);
      this.monthService.GetMonth(year, month).subscribe(x => {
        this.stateService.UpdateMonth(x)
      });
    });
    
    
  }

  exitModal(): void {
    this.stateService.ExitAllModals();
  }
}

import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Month} from "../Shared/Interfaces/Month";
import {TransactionService} from "../transaction.service";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import {Transaction} from "../Shared/Interfaces/Transaction";
import {RepeatService} from "../repeat.service";
import {StateService} from "../state.service";
import {RepeatContract} from "../Shared/Interfaces/RepeatContract";
import { BehaviorSubject } from "rxjs";

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

  selectedDate: Date = new Date();

  constructor(
    private monthService: TransactionService,
    private route: ActivatedRoute,
    private changeDectectorRef: ChangeDetectorRef,
    private repeatService: RepeatService,
    private stateService: StateService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.monthService.Month.subscribe(x => {
      this.month = x;
      this.total = x.resolvedTotal;
    });
    this.monthService.total.subscribe(x => {
      if(x != 0){
        this.total = x;
        this.changeDectectorRef.detectChanges();
      }
    });
    this.route.paramMap.subscribe((params: ParamMap) => {
      const now = new Date();
      const month = Number.parseInt(params.get('month') ?? now.getMonth().toString());
      const year = Number.parseInt(params.get('year') ?? now.getFullYear().toString());
      this.monthService.GetMonth(year, month);
      this.selectedDate = new Date(year, month - 1);
    })

    this.monthService.malleableTransaction.subscribe(x => {
      this.malleableTransaction = x;
      this.changeDectectorRef.detectChanges();
    });
    this.stateService.editingRepeatSettings.subscribe(x => {
      this.editingRepeatContract = x;
      this.changeDectectorRef.detectChanges();
    })
  }

  exitEditingTransaction(): void {
    this.monthService.malleableTransaction.next(undefined);
  }
  exitEditingRepeat(): void {
    this.monthService.malleableRepeatSettingId.next(undefined);
  }
  editRepeatContract(id: string): void {
    this.repeatService.GetRepeatContract(id);
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

import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LoginService } from "../Shared/Services/login.service";
import { StateService } from "../Shared/Services/state.service";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { Month } from "../Shared/Interfaces/Month";
import { Transaction } from "../Shared/Interfaces/Transaction";
import { Day } from "../Shared/Interfaces/Day";
import { TransactionService } from "../Shared/Services/transaction.service";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  selectedDate: Date = new Date();
  isMenuActive = false;
  isAdmin = false;
  disableMenu = false;
  month: Month | undefined;
  monthlyTransactions: Transaction[] = [];
  total: number = 0;
  
  constructor(
      private loginService: LoginService,
      private stateService: StateService,
      private router: Router,
      private transactionService: TransactionService,
      private changeDectectorRef: ChangeDetectorRef,
) { }

  ngOnInit(): void {
    this.loginService.GetUser().then(x => this.isAdmin = x.isAdmin);
    this.stateService.month.subscribe(x => {
      this.month = x;
      if (x.days) {
        x.days.forEach(x => {
          this.addDaysTransactionToMonthlyList(x);
        });
      }
    });
    this.transactionService.SubscribeToSignalR();
    this.transactionService.total.subscribe(x => {
      if (x != 0) {
        this.total = x;
        this.changeDectectorRef.detectChanges();
      }
    });
    this.stateService.selectedDate.subscribe(x => this.selectedDate = x);
    
  }
  toggleMenu() {
    this.isMenuActive = !this.isMenuActive
  }

  signOut() {
    this.loginService.SignOut();
  }

  scroll(id: string) {
    let el = (<HTMLInputElement>document.getElementById(id));
    let elPosition = el.getBoundingClientRect().top;
    const offSet = 63;
    const position = elPosition + window.pageYOffset - offSet;
    window.scrollTo({ top: position, behavior: "smooth" })
  }

  openAdminPanel() {
    this.stateService.EditAdminSettings();
  }
  goToReports() {
    this.router.navigateByUrl('reports');
  }
  goToPreviousMonth() {
    this.navigateToDay(this.modMonth(-1));
    this.isMenuActive = false;
  }

  goToNextMonth() {
    this.navigateToDay(this.modMonth(1));
    
    this.isMenuActive = false;
  }

  goToToday() {
    this.navigateToDay(new Date());
    this.isMenuActive = false;
  }
  navigateToDay(date: Date) {
    this.router.navigateByUrl(`year/${date.getFullYear()}/month/${date.getMonth() + 1 }`)
  }
  private addDaysTransactionToMonthlyList(day: Day) {
    day.incomes.forEach(x => this.monthlyTransactions.push(x));
    day.bills.forEach(x => this.monthlyTransactions.push(x));
    day.oneOffs.forEach(x => this.monthlyTransactions.push(x));
  }
  
  private modMonth(adjustment: number): Date {
    let d = this.stateService.selectedDate.value;
    d.setMonth(d.getMonth() + adjustment);
    return d;
  }
}

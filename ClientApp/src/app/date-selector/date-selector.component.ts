import { Component, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as bulmaCalendar from "bulma-calendar";

@Component({
  selector: 'app-date-selector',
  templateUrl: './date-selector.component.html',
  styleUrls: ['./date-selector.component.css']
})
export class DateSelectorComponent implements OnInit {
  calRef: any;
  @Input() startDate: Date = new Date();
  @Output() startDateChange: EventEmitter<Date> = new EventEmitter<Date>();

  constructor(
    private elRef: ElementRef
  ) { }

  ngOnInit(): void {
  }

  ngAfterContentInit(): void {
    this.calRef = this.elRef.nativeElement.querySelector('input[type="date"]');
    bulmaCalendar.attach(this.calRef, {startDate: new Date(this.startDate)});
    this.calRef.bulmaCalendar.on('save', (x: any)=> {
      this.startDateChange.emit(new Date(x.data.value()))
    });

  }

}

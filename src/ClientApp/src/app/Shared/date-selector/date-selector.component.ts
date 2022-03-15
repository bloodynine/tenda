import { AfterContentInit, Component, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import * as bulmaCalendar from "bulma-calendar";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
  selector: 'app-date-selector',
  templateUrl: './date-selector.component.html',
  styleUrls: ['./date-selector.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: DateSelectorComponent
    }
  ]
})
export class DateSelectorComponent implements ControlValueAccessor, AfterContentInit {
  calRef: any;
  @Input() startDate: Date | undefined = undefined;
  @Output() startDateChange: EventEmitter<Date> = new EventEmitter<Date>();
  onChange = (date: Date) => {};
  onTouched = () => {};
  touched: boolean = false;

  constructor(
    private elRef: ElementRef
  ) { }

  writeValue(obj: any): void {
    if(obj){
      this.startDate = new Date(obj);
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.touched = fn;
  }

  markAsTouched() {
    if (!this.touched) {
      this.onTouched();
      this.touched = true;
    }
  }

  ngAfterContentInit(): void {
    this.calRef = this.elRef.nativeElement.querySelector('input[type="date"]');
    bulmaCalendar.attach(this.calRef, {startDate: this.startDate != undefined ? new Date(this.startDate) : undefined});
    this.calRef.bulmaCalendar.on('save', (x: any)=> {
      this.markAsTouched()
      this.startDateChange.emit(new Date(x.data.value()))
      this.onChange(new Date(x.data.value()));
    });

  }

}

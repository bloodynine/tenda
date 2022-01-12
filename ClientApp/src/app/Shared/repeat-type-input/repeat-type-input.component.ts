import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RepeatType, RepeatTypeLabel } from "../Interfaces/RepeatSettings";
import { FormGroup, FormGroupDirective } from "@angular/forms";

@Component({
  selector: 'app-repeat-type-input',
  templateUrl: './repeat-type-input.component.html',
  styleUrls: ['./repeat-type-input.component.css']
})
export class RepeatTypeInputComponent implements OnInit {

  // @ts-ignore
  @Input() form: FormGroup;
  public repeatTypes:RepeatType[] = [ RepeatType.ByDay, RepeatType.ByWeek, RepeatType.ByMonth]
  public repeatTypeLabelMapping = RepeatTypeLabel;
  selectedRepeatType: RepeatType | undefined = undefined;

  get showClear(): boolean {
    return this.form.get('repeatFrequency')?.value != ''
  }

  constructor(
  ) { }

  ngOnInit(): void {
  }

  selectRepeatType(type: RepeatType){
    this.selectedRepeatType = type;
  }

  clearDropDown() {
    this.form.controls['repeatFrequency'].setValue('');
  }

}

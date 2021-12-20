import {Component, ElementRef, Input, OnInit} from '@angular/core';
import {RepeatService} from "../repeat.service";
import {RepeatContract} from "../Shared/Interfaces/RepeatContract";
import {FormControl, FormGroup} from "@angular/forms";
import * as bulmaCalendar from "bulma-calendar";
import {RepeatType, RepeatTypeLabel} from "../Shared/Interfaces/RepeatSettings";
import {StateService} from "../state.service";

@Component({
  selector: 'app-repeat-form',
  templateUrl: './repeat-form.component.html',
  styleUrls: ['./repeat-form.component.css']
})
export class RepeatFormComponent implements OnInit {

  @Input() repeatContract: RepeatContract | undefined = undefined;
  form: FormGroup = new FormGroup({});
  calRef: any;
  public repeatTypes:RepeatType[] = [RepeatType.None, RepeatType.ByDay, RepeatType.ByWeek, RepeatType.ByMonth]
  public repeatTypeLabelMapping = RepeatTypeLabel;

  constructor(
    private repeatService: RepeatService,
    private elRef: ElementRef,
    private stateService: StateService
  ) { }


  ngOnInit(): void {
    this.form = new FormGroup({
      interval: new FormControl(this.repeatContract?.interval),
      repeatType: new FormControl(this.repeatContract?.repeatType),
      startDate: new FormControl(this.repeatContract?.startDate)
    })
  }

    ngAfterContentInit(): void {
      this.calRef = this.elRef.nativeElement.querySelector('input[type="date"]');
      bulmaCalendar.attach(this.calRef, {startDate: new Date(this.repeatContract?.startDate ?? '2020-11-01')});
      this.calRef.bulmaCalendar.on('select', (x: any) => {
        console.log('set date')
        console.log(x.data.value())
        this.form.controls['startDate'].setValue(x.data.value());
      })
    }

    updateContract(): void {
      if(this.repeatContract){
        this.repeatContract.repeatType = parseInt(this.form.get('repeatType')?.value);
        this.repeatContract.interval = this.form.get('interval')?.value;
        this.repeatContract.startDate = new Date(this.form.get('startDate')?.value);
        console.log(this.repeatContract)
        this.repeatService.UpdateRepeatContract(this.repeatContract.id, this.repeatContract);
      }
    }

  cancel() {
    this.stateService.NullRepeatSetting();
  }
}

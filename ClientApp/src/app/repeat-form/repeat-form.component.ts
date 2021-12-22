import {Component, ElementRef, Input, OnInit} from '@angular/core';
import {RepeatService} from "../repeat.service";
import {RepeatContract} from "../Shared/Interfaces/RepeatContract";
import {FormControl, FormGroup} from "@angular/forms";
import {RepeatType, RepeatTypeLabel} from "../Shared/Interfaces/RepeatSettings";
import {StateService} from "../state.service";

@Component({
  selector: 'app-repeat-form',
  templateUrl: './repeat-form.component.html',
  styleUrls: ['./repeat-form.component.css']
})
export class RepeatFormComponent implements OnInit {

  repeatContract: RepeatContract | undefined = undefined;
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
    this.stateService.editingRepeatSettings.subscribe(x => {
      this.repeatContract = x
      this.form = new FormGroup({
        interval: new FormControl(x.interval),
        repeatType: new FormControl(x.repeatType),
        startDate: new FormControl(x.startDate)
      });
    });
  }

    updateContract(): void {
      if(this.repeatContract){
        this.repeatContract.repeatType = parseInt(this.form.get('repeatType')?.value);
        this.repeatContract.interval = this.form.get('interval')?.value;
        this.repeatContract.startDate = new Date(this.form.get('startDate')?.value);
        this.repeatService.UpdateRepeatContract(this.repeatContract.id, this.repeatContract)
          .then(x => this.stateService.UpdateMonth(x));
      }
    }

  cancel() {
    this.stateService.ExitAllModals();
  }
}

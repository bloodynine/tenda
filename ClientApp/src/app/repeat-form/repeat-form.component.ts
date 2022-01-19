import { Component, ElementRef, OnInit } from '@angular/core';
import { RepeatService } from "../repeat.service";
import { RepeatContract } from "../Shared/Interfaces/RepeatContract";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { RepeatType, RepeatTypeLabel } from "../Shared/Interfaces/RepeatSettings";
import { StateService } from "../state.service";

@Component({
  selector: 'app-repeat-form',
  templateUrl: './repeat-form.component.html',
  styleUrls: ['./repeat-form.component.css']
})
export class RepeatFormComponent implements OnInit {

  repeatContract: RepeatContract | undefined = undefined;
  form: FormGroup = new FormGroup({});

  constructor(
    private repeatService: RepeatService,
    private elRef: ElementRef,
    private stateService: StateService
  ) {
  }

  doesFieldHaveError(fieldName: string, error?: string): boolean {
    const pristine = this.form.get(fieldName)?.pristine;
    if(!error){
      return !pristine && (this.form.get(fieldName)?.invalid ?? false);
    }
    return !pristine && (this.form.get(fieldName)?.hasError(error) ?? false);
  }

  ngOnInit(): void {
    const numRegex = /^-?\d*[.,]?\d{0,2}$/;
    this.stateService.editingRepeatSettings.subscribe(x => {
      this.repeatContract = x
      this.form = new FormGroup({
        transactionName: new FormControl(x.name, [Validators.required]),
        interval: new FormControl(x.interval),
        repeatFrequency: new FormControl(x.repeatType),
        startDate: new FormControl(x.startDate),
        amount: new FormControl(x.amount, [Validators.required, Validators.pattern(numRegex)])
      });
    });
  }

  updateContract(): void {
    if (this.repeatContract) {
      this.repeatContract.repeatType = parseInt(this.form.get('repeatFrequency')?.value);
      this.repeatContract.interval = this.form.get('interval')?.value;
      this.repeatContract.startDate = new Date(this.form.get('startDate')?.value);
      this.repeatContract.name = this.form.get('transactionName')?.value;
      this.repeatContract.amount = this.form.get('amount')?.value;
      this.repeatService.UpdateRepeatContract(this.repeatContract.id, this.repeatContract)
        .then(x => {
          this.stateService.ExitAllModals();
          this.stateService.UpdateMonth(x);
        });
    }
  }

  deleteContract(): void {
    console.log('wtv')
    if(this.repeatContract){
      this.repeatService.DeleteRepeatContract(this.repeatContract.id).then(x => {
        this.stateService.ExitAllModals();
        this.stateService.UpdateMonth(x);
      })
    }
  }

  cancel() {
    this.stateService.ExitAllModals();
  }
}

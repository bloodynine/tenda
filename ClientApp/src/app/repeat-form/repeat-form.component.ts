import { Component, ElementRef, OnInit } from '@angular/core';
import { RepeatService } from "../repeat.service";
import { RepeatContract } from "../Shared/Interfaces/RepeatContract";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
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
    private stateService: StateService,
    private fb: FormBuilder
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
      this.form = this.fb.group({
        transactionName: new FormControl(x.name, [Validators.required]),
        startDate: new FormControl(new Date(x.startDate)),
        amount: new FormControl(x.amount, [Validators.required, Validators.pattern(numRegex)]),
        repeatSettings: this.fb.group({
          repeatFrequency: new FormControl(this.repeatContract.repeatType),
          endDate: new FormControl(this.repeatContract.endDate),
          interval: new FormControl(this.repeatContract.interval, Validators.pattern(numRegex))
        })
      });
    });
  }

  updateContract(): void {
    if (this.repeatContract) {
      const repeatSettings = this.form.get('repeatSettings') as FormGroup;
      this.repeatContract.repeatType = parseInt(repeatSettings.get('repeatFrequency')?.value);
      this.repeatContract.interval = repeatSettings.get('interval')?.value;
      this.repeatContract.startDate = new Date(this.form.get('startDate')?.value);
      this.repeatContract.name = this.form.get('transactionName')?.value;
      this.repeatContract.amount = this.form.get('amount')?.value;
      this.repeatContract.endDate = new Date(repeatSettings.get('endDate')?.value);

      this.repeatService.UpdateRepeatContract(this.repeatContract.id, this.repeatContract)
        .then(x => {
          this.stateService.ExitAllModals();
          this.stateService.UpdateMonth(x);
        });
    }
  }

  deleteContract(): void {
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

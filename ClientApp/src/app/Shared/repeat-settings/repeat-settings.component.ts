import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormGroupDirective } from "@angular/forms";

@Component({
  selector: 'app-repeat-settings',
  templateUrl: './repeat-settings.component.html',
  styleUrls: ['./repeat-settings.component.css']
})
export class RepeatSettingsComponent implements OnInit {
  @Input() formGroupName: string = "";
  form: FormGroup = new FormGroup({});
  endDate: Date | undefined;

  get showRepeatSettings(): boolean{
    return this.form.get('repeatFrequency')?.value != "";
  }

  constructor(
    private rootFormGroup: FormGroupDirective
  ) { }

  ngOnInit(): void {
    this.form = this.rootFormGroup.control.get(this.formGroupName) as FormGroup;
  }

  doesFieldHaveError(fieldName: string, error?: string): boolean {
    const pristine = this.form.get(fieldName)?.pristine;
    if(!error){
      return !pristine && (this.form.get(fieldName)?.invalid ?? false);
    }
    return !pristine && (this.form.get(fieldName)?.hasError(error) ?? false);
  }
}

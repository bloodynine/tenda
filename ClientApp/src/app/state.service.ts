import { Injectable } from '@angular/core';
import {BehaviorSubject, ReplaySubject, Subject} from "rxjs";
import {RepeatSettings} from "./Shared/Interfaces/RepeatSettings";
import {RepeatContract} from "./Shared/Interfaces/RepeatContract";
import {Month} from "./Shared/Interfaces/Month";

@Injectable({
  providedIn: 'root'
})
export class StateService {

  public editingRepeatSettings: Subject<RepeatContract> = new ReplaySubject();
  public month: BehaviorSubject<Month> = new BehaviorSubject<Month>({} as Month);

  constructor() { }

  public EditRepeatSetting(setting: RepeatContract): void{
    this.editingRepeatSettings.next(setting);
  }

  public NullRepeatSetting(): void {
    this.editingRepeatSettings.next(undefined);
  }

  public UpdateMonth(month: Month): void {
    this.month.next(month);
  }
}

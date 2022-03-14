import { InjectorInstance } from "../../app.module";
import { Observable } from "rxjs";
import { StateService } from "./state.service";
import { TendaError } from "../Interfaces/ValidationError";


export function HandleHttpError(overrideMsg?: string) {
  const stateService = InjectorInstance.get<StateService>(StateService);
  return function <T>(source: Observable<T>): Observable<T> {
    return new Observable(subscriber => {
      source.subscribe({
        next(value) {
          subscriber.next(value);
        },
        error(error) {
          if (overrideMsg) {
            stateService.AddErrorMsg(overrideMsg);
            return stateService.month.getValue();
          }
          const err = new TendaError(error);
          stateService.AddErrorMsg(err.message);
          return stateService.month.getValue();
        },
        complete() {
          subscriber.complete();
        }
      })
    });
  }
}

export class HandleErrorService {

  constructor() {
  }
}

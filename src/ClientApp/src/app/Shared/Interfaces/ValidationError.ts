export interface FluentValidation {
  statusCode: number;
  errors: Record<string, string[]>
}

export interface ValidationError{
  status: number;
  statusText: string;
  error: FluentValidation;
}

export class TendaError {
  message: string;

  constructor(validationError: ValidationError) {
    if(validationError.statusText == "Internal Server Error"){
      this.message = "Something has gone wrong";
    }else{
      this.message = this.parseValidationErrorForMessage(validationError);
    }
  }


  private parseValidationErrorForMessage(validationError: ValidationError): string {
    let msg = "";
    Object.entries(validationError.error.errors).forEach(([key, val]) => {
      msg = `${msg}${val} `;
    })
    return msg;
  }
}

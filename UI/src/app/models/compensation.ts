export interface CompensationModel {
  compensationId: string,
  firstName: string,
  surname: string,
  verdict?: string,
  operatorFullName?: string,
  companyName: string,
  amount: number,
  insuranceAmount: number,
  appliedOn: Date,
  description?: string,
  status: RequestState,
}

export enum RequestState {
  Open,
  Processing,
  Approved,
  Closed
}

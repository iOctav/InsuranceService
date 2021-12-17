import {CompensationModel} from "./compensation";

export interface InsuranceContractModel {
  insuranceContractId: string,
  consumerId: string,
  insuranceAgent: string,
  startDate: Date,
  endDate: Date,
  compensations: CompensationModel[],
  insurancePremium: number,
  insuranceAmount: number,
  state: InsuranceContractState,
  agentCompanyName: string,
}

export enum InsuranceContractState {
  Created,
  Processing,
  Approved,
  Rejected
}

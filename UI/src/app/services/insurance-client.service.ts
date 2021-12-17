import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from "rxjs";
import {InsuranceAgentModel} from "../models/insurance-agent";
import {NewApplicationModel} from "../models/new-application";
import {InsuranceContractModel} from "../models/insurance-contract";
import {ConsumerApplicationModel} from "../models/consumer-application";
import {OperatorApplicationModel} from "../models/operator-application";

@Injectable()
export class InsuranceClientService {
  constructor(private http: HttpClient) { }

  public getAgents(): Observable<InsuranceAgentModel[]> {
    return this.http.get<InsuranceAgentModel[]>('/api/insurance/agents');
  }

  public applyInsurance(application: NewApplicationModel): Observable<string> {
    return this.http.post<string>('/api/insurance/create',  application);
  }

  public getConsumerInsurance(): Observable<InsuranceContractModel> {
    return this.http.get<InsuranceContractModel>('/api/insurance/consumer-insurance');
  }

  public stopInsurance(insuranceId: string): Observable<string> {
    return this.http.put<string>(`/api/insurance/stop-insurance/${insuranceId}`, {});
  }

  public getOperatorApplication(): Observable<ConsumerApplicationModel[]> {
    return this.http.get<ConsumerApplicationModel[]>('/api/insurance/operator-applications');
  }

  public approveApplication(applicationId: string, operatorId: string): Observable<void> {
    return this.http.post<void>(`/api/insurance/approve/${applicationId}`, { operatorId });
  }

  public getAgentApplication(): Observable<OperatorApplicationModel[]> {
    return this.http.get<OperatorApplicationModel[]>('/api/insurance/agent-applications');
  }

  public signApplication(applicationId: string, agentId: string): Observable<void> {
    return this.http.put<void>(`/api/insurance/sign/${applicationId}`, { operatorId: agentId });
  }

  public rejectApplication(applicationId: string): Observable<string> {
    return this.http.put<string>(`/api/insurance/reject-application/${applicationId}`, {});
  }
}

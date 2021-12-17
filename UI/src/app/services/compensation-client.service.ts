import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import {CompensationModel} from "../models/compensation";
import {NewCompensationModel} from "../models/new-compensation";
import {CompensationVerdictModel} from "../models/compensation-verdict";

@Injectable()
export class CompensationClientService {
  constructor(private http: HttpClient) { }

  public getCompensation(compensationId: string): Observable<CompensationModel> {
    return this.http.get<CompensationModel>(`/api/compensation/${compensationId}`);
  }
  public rejectCompensation(compensationId: string): Observable<CompensationModel> {
    return this.http.get<CompensationModel>(`/api/compensation/reject/${compensationId}`);
  }

  public getOperatorCompensations(): Observable<CompensationModel[]> {
    return this.http.get<CompensationModel[]>('/api/compensation/operator-compensations');
  }

  public getAgentCompensations(): Observable<CompensationModel[]> {
    return this.http.get<CompensationModel[]>('/api/compensation/agent-compensations');
  }

  public getConsumerCompensations(): Observable<CompensationModel[]> {
    return this.http.get<CompensationModel[]>('/api/compensation/consumer-compensations');
  }

  public createNewCompensationRequest(model: NewCompensationModel): Observable<string> {
    return this.http.post<string>('/api/compensation/new', model);
  }

  public approveCompensation(compensationId: string, model: CompensationVerdictModel): Observable<string> {
    return this.http.post<string>(`/api/compensation/approve/${compensationId}`, model);
  }

  public signCompensation(compensationId: string): Observable<string> {
    return this.http.post<string>(`/api/compensation/sign/${compensationId}`, {});
  }
}

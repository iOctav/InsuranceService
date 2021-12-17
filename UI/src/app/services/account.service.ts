import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from "rxjs";
import {RouteResult} from "../models/route-result";
import {StringFieldModel } from "../models/string-field";
import {ConsumerModel} from "../models/consumer";
import {PersonModel} from "../models/person";
import {InsuranceAgentModel} from "../models/insurance-agent";

@Injectable()
export class AccountService {
  constructor(private http: HttpClient) { }

  public login(username: string, password: string): Observable<string | undefined> {
    return this.http.post<StringFieldModel>(`/api/auth/login`, { username, password })
      .pipe(map(user => {
        if (user.field)
          localStorage.setItem('user', user.field);
        return user.field;
      }));
  }

  public getRole(): Observable<RouteResult> {
    return this.http.get<RouteResult>('/api/auth/get-role');
  }

  public getConsumer(): Observable<ConsumerModel> {
    return this.http.get<ConsumerModel>('/api/auth/get-consumer');
  }

  public getOperator(): Observable<PersonModel> {
    return this.http.get<PersonModel>('/api/auth/get-operator');
  }

  public getAgent(): Observable<InsuranceAgentModel> {
    return this.http.get<InsuranceAgentModel>('/api/auth/get-agent');
  }
}

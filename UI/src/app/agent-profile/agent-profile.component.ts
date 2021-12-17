import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {InsuranceClientService} from "../services/insurance-client.service";
import {AccountService} from "../services/account.service";
import {finalize} from "rxjs";
import {InsuranceContractState} from "../models/insurance-contract";
import {InsuranceAgentModel} from "../models/insurance-agent";
import {OperatorApplicationModel} from "../models/operator-application";
import {CompensationModel} from "../models/compensation";
import {CompensationClientService} from "../services/compensation-client.service";

@Component({
  selector: 'app-agent-profile',
  templateUrl: './agent-profile.component.html',
  styleUrls: ['./agent-profile.component.scss']
})
export class AgentProfileComponent implements OnInit {
  public agent?: InsuranceAgentModel;
  public loading: boolean = true;
  public applications: OperatorApplicationModel[] = [];
  public compensations: CompensationModel[] = [];

  constructor(private router: Router,
              private insuranceClientService: InsuranceClientService,
              private compensationClientService: CompensationClientService,
              private accountService: AccountService) {
    this.accountService.getAgent()
      .pipe(
        finalize(() => this.loading = false)
      )
      .subscribe(x => this.agent = x);
    this.insuranceClientService.getAgentApplication()
      .subscribe(x => this.applications = x);
    this.compensationClientService.getAgentCompensations()
      .subscribe(x => this.compensations = x);
  }

  ngOnInit(): void {
    if (!localStorage.getItem('user')) {
      this.router.navigate(['..']).then();
    }
  }

  getStateString(state: InsuranceContractState): string {
    return InsuranceContractState[state];
  }

  formatFate(val: Date): string {
    let date = new Date(val);
    return `${date.getDate()}\\${date.getMonth() + 1}\\${date.getFullYear()}`;
  }

  logout(): void {
    localStorage.removeItem('user');
    this.router.navigate(['..']).then();
  }

  sign(applicationId: string) {
    this.insuranceClientService.signApplication(applicationId, this.agent?.id!)
      .subscribe(x => {
        this.applications = [];
        this.loading = true;
        this.insuranceClientService.getAgentApplication()
          .pipe(
            finalize(() => this.loading = false)
          )
          .subscribe(x => this.applications = x);
      })
  }

  refund(compensationId: string) {
    return this.compensationClientService.signCompensation(compensationId)
      .subscribe(x => {
        this.compensations = [];
        this.loading = true;
        this.compensationClientService.getOperatorCompensations()
          .pipe(
            finalize(() => this.loading = false)
          )
          .subscribe(x => this.compensations = x);
      });
  }

  rejectCompensation(compensationId: string) {
    return this.compensationClientService.rejectCompensation(compensationId)
      .subscribe(x => {
        this.compensations = [];
        this.loading = true;
        this.compensationClientService.getOperatorCompensations()
          .pipe(
            finalize(() => this.loading = false)
          )
          .subscribe(x => this.compensations = x);
      });
  }

  reject(applicationId: string) {
    this.insuranceClientService.rejectApplication(applicationId)
      .subscribe(x => {
        this.compensations = [];
        this.loading = true;
        this.compensationClientService.getOperatorCompensations()
          .pipe(
            finalize(() => this.loading = false)
          )
          .subscribe(x => this.compensations = x);
      });
  }

}

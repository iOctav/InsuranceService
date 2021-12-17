import { Component, OnInit } from '@angular/core';
import { InsuranceContractState} from "../models/insurance-contract";
import {Router} from "@angular/router";
import {InsuranceClientService} from "../services/insurance-client.service";
import {AccountService} from "../services/account.service";
import {PersonModel} from "../models/person";
import {finalize} from "rxjs";
import {ConsumerApplicationModel} from "../models/consumer-application";
import {CompensationModel} from "../models/compensation";
import {CompensationClientService} from "../services/compensation-client.service";

@Component({
  selector: 'app-operator-profile',
  templateUrl: './operator-profile.component.html',
  styleUrls: ['./operator-profile.component.scss']
})
export class OperatorProfileComponent implements OnInit {
  public controller?: PersonModel;
  public loading: boolean = true;
  public applications: ConsumerApplicationModel[] = [];
  public compensations: CompensationModel[] = [];

  constructor(private router: Router,
              private insuranceClientService: InsuranceClientService,
              private compensationClientService: CompensationClientService,
              private accountService: AccountService) {
    this.accountService.getOperator()
      .pipe(
        finalize(() => this.loading = false)
      )
      .subscribe(x => this.controller = x);
    this.insuranceClientService.getOperatorApplication()
      .subscribe(x => this.applications = x);
    this.compensationClientService.getOperatorCompensations()
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

  approve(applicationId: string) {
    this.insuranceClientService.approveApplication(applicationId, this.controller?.id!)
      .subscribe(x => {
        this.applications = [];
        this.loading = true;
        this.insuranceClientService.getOperatorApplication()
          .pipe(
            finalize(() => this.loading = false)
          )
          .subscribe(x => this.applications = x);
      })
  }

  consider(compensationId: string) {
    this.router.navigate([`operator/refund/${compensationId}`]).then();
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

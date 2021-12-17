import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {InsuranceClientService} from "../services/insurance-client.service";
import {InsuranceContractModel, InsuranceContractState} from "../models/insurance-contract";
import {finalize} from "rxjs";
import {AccountService} from "../services/account.service";
import {ConsumerModel} from "../models/consumer";
import {CompensationModel, RequestState} from "../models/compensation";
import {CompensationClientService} from "../services/compensation-client.service";

@Component({
  selector: 'app-consumer-profile',
  templateUrl: './consumer-profile.component.html',
  styleUrls: ['./consumer-profile.component.scss']
})
export class ConsumerProfileComponent implements OnInit {
  public insuranceContract?: InsuranceContractModel;
  public consumer?: ConsumerModel;
  public loading: boolean = true;
  public compensations: CompensationModel[] = [];

  constructor(private router: Router,
              private insuranceClientService: InsuranceClientService,
              private accountService: AccountService,
              private compensationClientService: CompensationClientService) {
    this.compensationClientService.getConsumerCompensations()
      .subscribe(x => this.compensations = x);
    this.accountService.getConsumer()
      .subscribe(x => this.consumer = x);
    this.insuranceClientService.getConsumerInsurance()
      .pipe(
        finalize(() => this.loading = false)
      )
      .subscribe(x => {
        this.insuranceContract = x;
      });
  }

  ngOnInit(): void {
    if (!localStorage.getItem('user')) {
      this.router.navigate(['..']).then();
    }
  }

  buyInsurance(): void {
    this.router.navigate(['consumer/new']).then();
  }

  refund(): void {
    this.router.navigate(['consumer/refund']).then();
  }

  stopInsurance(): void {
    this.loading = true;
    if(confirm("Are you sure to request cancellation of the insurance?")) {
      this.insuranceClientService.stopInsurance(this.insuranceContract?.insuranceContractId!)
        .pipe(
          finalize(() => this.loading = false)
        )
        .subscribe(x => {
          this.compensations = [];
          this.insuranceContract = undefined;
          this.insuranceClientService.getConsumerInsurance()
            .pipe(
              finalize(() => this.loading = false)
            )
            .subscribe(x => {
              this.insuranceContract = x;
            });
        });
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

  cancel(compensationId: string) {
    return null;
  }

  statusText(state: RequestState): string {
    return RequestState[state];
  }

}

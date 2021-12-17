import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {InsuranceAgentModel} from "../../models/insurance-agent";
import {InsuranceClientService} from "../../services/insurance-client.service";
import {NewApplicationModel} from "../../models/new-application";

@Component({
  selector: 'app-insurance-request',
  templateUrl: './insurance-request.component.html'
})
export class InsuranceRequestComponent implements OnInit {
  insuranceForm: FormGroup = new FormGroup({
    agent: new FormControl(''),
    startDate: new FormControl(''),
    endDate: new FormControl(''),
    premium: new FormControl(0),
    amount: new FormControl(0),
  });
  loading = true;
  submitted = false;
  agents: InsuranceAgentModel[] = [];
  selectedAgent?: InsuranceAgentModel = undefined;
  minDate: Date = new Date();
  dayRange: number = 0;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private insuranceClientService: InsuranceClientService
  ) {
    this.insuranceClientService.getAgents().subscribe(x => {
      this.agents = x;
      this.loading = false;
    });
  }

  ngOnInit() {
    this.insuranceForm = this.formBuilder.group({
      agent: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      premium: [{ value: 0, disabled: true }, Validators.required],
      amount: [{ value: 0, disabled: true }, Validators.required],
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.insuranceForm?.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.insuranceForm?.invalid) {
      return;
    }

    this.loading = true;
    this.insuranceClientService.applyInsurance({
      insuranceAgentId: this.f['agent'].value,
      startDate: this.f['startDate'].value,
      endDate: this.f['endDate'].value,
      insurancePremium: this.f['premium'].value,
      insuranceAmount: this.f['amount'].value,
    } as NewApplicationModel)
      .subscribe(x => {
        this.router.navigate(['consumer']);
      })
  }

  changeAgent(event: any) {
    let val = event.target.value.split(' ')[1];
    this.selectedAgent = this.agents.find(x => x.id === val);
    this.f['agent'].setValue(val);
    this.f['premium'].setValue(this.dayRange * (this.selectedAgent?.tariff ?? 0) * 0.1);
    this.f['amount'].setValue(this.dayRange * (this.selectedAgent?.tariff ?? 0));
  }

  changeStartDate(event: any) {
    this.f['startDate'].setValue(event.target.value);
    var diff = Math.abs(new Date(this.f['startDate'].value).getTime() - new Date(this.f['endDate'].value).getTime());
    this.dayRange = Math.ceil(diff / (1000 * 3600 * 24));
    this.f['premium'].setValue(this.dayRange * (this.selectedAgent?.tariff ?? 0) * 0.1);
    this.f['amount'].setValue(this.dayRange * (this.selectedAgent?.tariff ?? 0));

  }

  changeEndDate(event: any) {
    this.f['endDate'].setValue(event.target.value);
    var diff = Math.abs(new Date(this.f['startDate'].value).getTime() - new Date(this.f['endDate'].value).getTime());
    this.dayRange = Math.ceil(diff / (1000 * 3600 * 24));
    this.f['premium'].setValue(this.dayRange * (this.selectedAgent?.tariff ?? 0) * 0.1);
    this.f['amount'].setValue(this.dayRange * (this.selectedAgent?.tariff ?? 0));
  }

}

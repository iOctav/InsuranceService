import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {CompensationClientService} from "../../services/compensation-client.service";
import {CompensationVerdictModel} from "../../models/compensation-verdict";
import {finalize} from "rxjs";
import {CompensationModel} from "../../models/compensation";

@Component({
  selector: 'app-compensation-form',
  templateUrl: './compensation-form.component.html'
})
export class CompensationFormComponent implements OnInit {
  compensationForm: FormGroup = new FormGroup({
    amount: new FormControl(0),
    description: new FormControl(''),
    verdict: new FormControl(''),
  });
  loading = true;
  submitted = false;
  compensationId?: string;
  compensation?: CompensationModel;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private compensationClientService: CompensationClientService
  ) {
  }

  ngOnInit() {

    this.compensationId = this.route.snapshot.params['compensationId'];
    this.compensationClientService.getCompensation(this.compensationId!)
      .pipe(
        finalize(() => this.loading = false)
      )
      .subscribe(x => {
        this.compensation = x;
        this.compensationForm = this.formBuilder.group({
          amount: [x.amount, Validators.required],
          description: [{value: x.description, disabled: true}],
          verdict: [x.verdict, Validators.required],
        });
      });
  }

  // convenience getter for easy access to form fields
  get f() { return this.compensationForm?.controls; }

  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.compensationForm?.invalid) {
      return;
    }

    this.loading = true;
    this.compensationClientService.approveCompensation(
      this.compensationId!, {
        amount: this.f['amount'].value,
        description: this.f['description'].value,
        verdict: this.f['verdict'].value,
        approved: true,
    } as CompensationVerdictModel)
      .subscribe(x => {
        this.router.navigate(['consumer']);
      })
  }

}

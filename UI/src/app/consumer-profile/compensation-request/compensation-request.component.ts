import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {CompensationClientService} from "../../services/compensation-client.service";
import {NewCompensationModel} from "../../models/new-compensation";
import {CompensationModel} from "../../models/compensation";

@Component({
  selector: 'app-compensation-request',
  templateUrl: './compensation-request.component.html'
})
export class CompensationRequestComponent implements OnInit {
  compensationForm: FormGroup = new FormGroup({
    amount: new FormControl(0),
    description: new FormControl(''),
  });
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private compensationClientService: CompensationClientService
  ) {
  }

  ngOnInit() {
    this.compensationForm = this.formBuilder.group({
      amount: [0, Validators.required],
      description: [''],
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
    this.compensationClientService.createNewCompensationRequest({
      amount: this.f['amount'].value,
      description: this.f['description'].value,
    } as NewCompensationModel)
      .subscribe(x => {
        this.router.navigate(['consumer']);
      })
  }

}

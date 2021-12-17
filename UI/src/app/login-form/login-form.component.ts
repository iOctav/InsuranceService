import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import { first } from 'rxjs/operators';
import {AccountService} from "../services/account.service";
import {finalize} from "rxjs";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html'
})
export class LoginFormComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({
    username: new FormControl(''),
    password: new FormControl('')
  });
  loading = false;
  submitted = false;
  errorMessage?: string = undefined;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AccountService
  ) {
  }

  ngOnInit() {
    if (localStorage.getItem('user')) {
      this.authService.getRole()
        .pipe(first())
        .subscribe(x => this.router.navigate([x.routePath!]));
    }
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm?.controls; }

  onSubmit() {
    this.errorMessage = undefined;
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm?.invalid) {
      return;
    }

    this.loading = true;
    this.authService.login(this.f['username'].value, this.f['password']!.value)
      .pipe(
        first()
      )
      .subscribe(
        data => {
          if (data)
            this.authService.getRole()
              .pipe(first())
              .subscribe(x => this.router.navigate([x.routePath!]));
          else
            this.loading = false;
        }, error => {
          this.errorMessage = "Invalid username or password";
          this.loading = false
        });
  }
}

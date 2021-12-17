import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConsumerProfileComponent } from './consumer-profile/consumer-profile.component';
import {AccountService} from "./services/account.service";
import {InsuranceClientService} from "./services/insurance-client.service";
import {AuthHeaderInterceptor} from "./interceptors/auth-header.interceptor";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {LoginFormComponent} from "./login-form/login-form.component";
import {InsuranceRequestComponent} from "./consumer-profile/insurance-request/insurance-request.component";
import { OperatorProfileComponent } from './operator-profile/operator-profile.component';
import { AgentProfileComponent } from './agent-profile/agent-profile.component';
import {CompensationClientService} from "./services/compensation-client.service";
import {CompensationRequestComponent} from "./consumer-profile/compensation-request/compensation-request.component";
import {CompensationFormComponent} from "./operator-profile/compensation-request/compensation-form.component";

@NgModule({
  declarations: [
    AppComponent,
    ConsumerProfileComponent,
    LoginFormComponent,
    InsuranceRequestComponent,
    OperatorProfileComponent,
    AgentProfileComponent,
    CompensationRequestComponent,
    CompensationFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    AccountService,
    InsuranceClientService,
    CompensationClientService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHeaderInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }

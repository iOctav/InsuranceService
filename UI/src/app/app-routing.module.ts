import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConsumerProfileComponent } from "./consumer-profile/consumer-profile.component";
import { LoginFormComponent } from "./login-form/login-form.component";
import { InsuranceRequestComponent } from "./consumer-profile/insurance-request/insurance-request.component";
import {OperatorProfileComponent} from "./operator-profile/operator-profile.component";
import {AgentProfileComponent} from "./agent-profile/agent-profile.component";
import {CompensationRequestComponent} from "./consumer-profile/compensation-request/compensation-request.component";
import {CompensationFormComponent} from "./operator-profile/compensation-request/compensation-form.component";

const routes: Routes = [
  { path: '', component: LoginFormComponent },
  { path: 'consumer', component: ConsumerProfileComponent },
  { path: 'consumer/new', component: InsuranceRequestComponent },
  { path: 'consumer/refund', component: CompensationRequestComponent },
  { path: 'agent', component: AgentProfileComponent },
  { path: 'operator', component: OperatorProfileComponent },
  { path: 'operator/refund/:compensationId', component: CompensationFormComponent },
  { path: '**', component: LoginFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

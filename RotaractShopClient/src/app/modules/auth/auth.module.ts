import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import {AuthRouting} from './auth-routing.module';
import {ReactiveFormsModule} from '@angular/forms';
import { LoginComponent } from './login/login.component';
import {NgbAlertModule} from "@ng-bootstrap/ng-bootstrap";



@NgModule({
  declarations: [RegisterComponent, LoginComponent],
    imports: [
        AuthRouting,
        CommonModule,
        ReactiveFormsModule,
        NgbAlertModule
    ]
})
export class AuthModule { }

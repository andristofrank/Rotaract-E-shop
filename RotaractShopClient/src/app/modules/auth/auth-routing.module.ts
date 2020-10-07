
// Route
import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {RegisterComponent} from './register/register.component';
import {LoginComponent} from './login/login.component';

const authRoutes: Routes = [
  {path: '', redirectTo: 'login' },
  {path: 'createUser', component: RegisterComponent},
  {path: 'login', component: LoginComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(authRoutes)
  ],
  exports: [
    RouterModule
  ]
})


export class AuthRouting {}

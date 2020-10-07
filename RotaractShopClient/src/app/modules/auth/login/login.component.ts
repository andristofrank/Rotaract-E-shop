import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthService} from '../../../services/auth.service';
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
loginForm: FormGroup;
submitted = false;
  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private route: Router,
              public toastrService: ToastrService) {
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  ngOnInit(): void {
  }
  get f() { return this.loginForm.controls; }
  onSubmit() {
    this.submitted = true;
    // stop if form is invalid
    if (this.loginForm.invalid) {
      return;
    }
    const formUser = this.loginForm.getRawValue();
    this.authService.login(formUser.username, formUser.password);
  }
  cancel() {
    this.route.navigate(['home']);
  }
  registerUser() {
    this.route.navigate(['auth/createUser']);
  }

}

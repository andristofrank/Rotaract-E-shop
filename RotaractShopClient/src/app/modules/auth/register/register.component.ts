import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {AuthService} from '../../../services/auth.service';
import {Role} from '../../../../../APIs/models/Role';
import {User} from "../../../../../APIs/models";
import {ValidatorsService} from "../../../services/validators.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  createUserForm: FormGroup;
  submitted = false;
  usernameNotUnique = false;
  emailNotUnique = false;
  districtExists = false;
  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private route: Router,
              private validatorsService: ValidatorsService) {


  }
  ngOnInit(): void {
    this.createUserForm = this.formBuilder.group({
      firstName: new FormControl(null,
        {validators: [Validators.required,
            Validators.minLength(4)]}),
      lastName: new FormControl(null,
        {validators: [Validators.required,
            Validators.minLength(4)]}),
      districtNumber: new FormControl(null,
        {validators: [Validators.required]}),
      username: new FormControl(null,
        {validators: [Validators.required,
            Validators.minLength(4)]}),
      email: new FormControl(null,
        {validators: [Validators.required,
            Validators.email,
            Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]}),
      password: new FormControl(null,
        {validators: [Validators.required,
            Validators.minLength(8)]})
    });
  }
  onSubmit() {
    /*
     stop if form is invalid
     */
    this.submitted = true;
    if (this.createUserForm.invalid ||
      this.usernameNotUnique ||
      this.emailNotUnique ||
      this.districtExists) {
      return;
    }
    const formUser = this.createUserForm.getRawValue();
    const userCreateRequest: User = {
      FirstName: formUser.firstName,
      LastName: formUser.lastName,
      DistrictNumber: formUser.districtNumber,
      Username: formUser.username,
      Email: formUser.email,
      Password: formUser.password,
      Role: Role.Customer
    };
    this.authService.register(userCreateRequest);
  }
  cancel() {
    this.route.navigate(['home/catalogue']);
  }
  login() {
    this.route.navigate(['auth/login']);
  }
  disctrictNoExistsValidate() {
    if(this.f.districtNumber.value){
      this.validatorsService.validateDistrictNumber(this.f.districtNumber.value).subscribe(
        response => { if(response) {this.districtExists = false; } else { this.districtExists = true; } }
      )
    }
  }
  uniquenessUsername() {
    if(this.f.username.value) {
      this.validatorsService.uniquenessUsername(this.f.username.value).subscribe(
        response => { if(response) { this.usernameNotUnique = true; } else {
          this.usernameNotUnique = false; } }
      )
    }
  }
  uniquenessEmail() {
    if(this.f.email.value) {
      this.validatorsService.uniquenessEmail(this.f.email.value).subscribe(
        response => {if(response) { this.emailNotUnique = true;} else {
          this.emailNotUnique = false; } }
      )
    }
  }
  // getter for easy access to form fields
  get f() { return this.createUserForm.controls; }
}

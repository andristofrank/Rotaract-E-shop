import {async, ComponentFixture, fakeAsync, TestBed} from '@angular/core/testing';
import {RegisterComponent} from "./register.component";
import {RouterTestingModule} from "@angular/router/testing";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FormBuilder, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";
import {Router} from "@angular/router";
import {ValidatorsService} from "../../../services/validators.service";
import {Observable, of} from "rxjs";
import {Role} from "../../../../../APIs/models/Role";

const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
const authServiceSpy = jasmine.createSpyObj('AuthService', ['register']);
const validatorsServiceSpy = jasmine.createSpyObj('ValidatorsService',
  ['validateDistrictNumber', 'uniquenessUsername', 'uniquenessEmail']);
const valid_createUser = {
  FirstName: 'TestFirstName',
  LastName: 'TestLastName',
  DistrictNumber: 2,
  Username: 'TestUsername',
  Email: 'test-2mail@gmail.com',
  Password: 'password'
}
describe('Register Component Integrated Test', () => {
  let fixture: ComponentFixture<RegisterComponent>;
  function updateForm(firstName, lastName, distrcitNumber, username, email, password) {
    fixture.componentInstance.createAccountForm.controls['firstName'].setValue(firstName);
    fixture.componentInstance.createAccountForm.controls['lastName'].setValue(lastName);
    fixture.componentInstance.createAccountForm.controls['districtNumber'].setValue(distrcitNumber);
    fixture.componentInstance.createAccountForm.controls['username'].setValue(username);
    fixture.componentInstance.createAccountForm.controls['email'].setValue(email);
    fixture.componentInstance.createAccountForm.controls['password'].setValue(password);

  };

  let registerSpy;
  let validateDistrictNumberSpy;
  let uniquenessUsernameSpy;
  let uniquenessEmailSpy;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        ReactiveFormsModule
      ],
      providers: [
        {provide: AuthService, useValue: authServiceSpy},
        FormBuilder,
        {provide: Router, useValue: routerSpy },
        {provide: ValidatorsService, useValue: validatorsServiceSpy}
      ],
      declarations: [ RegisterComponent ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);

    registerSpy = authServiceSpy.register.and.returnValue(true);
    validateDistrictNumberSpy = validatorsServiceSpy.validateDistrictNumber.and.returnValue(of(true));
    uniquenessUsernameSpy = validatorsServiceSpy.uniquenessUsername.and.returnValue(of(true));
    uniquenessEmailSpy = validatorsServiceSpy.uniquenessEmail.and.returnValue(of(true));
  }));

  it('validators uniquenessUsername() should be called', fakeAsync(() => {
    updateForm(valid_createUser.FirstName,
      valid_createUser.LastName,
      valid_createUser.DistrictNumber,
      valid_createUser.Username,
      valid_createUser.Email,
      valid_createUser.Password);
    fixture.detectChanges();

    //Simulate Blur event on input
    let input = fixture.debugElement.nativeElement.querySelector('#username');
    input.dispatchEvent(new Event('blur'));

    expect(validatorsServiceSpy.uniquenessUsername).toHaveBeenCalled();
  }));

  it('validators validateDistrictNumber() should be called', fakeAsync(() => {
    updateForm(valid_createUser.FirstName,
      valid_createUser.LastName,
      valid_createUser.DistrictNumber,
      valid_createUser.Username,
      valid_createUser.Email,
      valid_createUser.Password);
    fixture.detectChanges();
    //Simulate Blur event on input

    let input = fixture.debugElement.nativeElement.querySelector('#districtNumber');
    input.dispatchEvent(new Event('blur'));

    expect(validatorsServiceSpy.validateDistrictNumber).toHaveBeenCalled();
  }));

  it('validators uniquenessEmail() should be called', fakeAsync(() => {
    updateForm(valid_createUser.FirstName,
      valid_createUser.LastName,
      valid_createUser.DistrictNumber,
      valid_createUser.Username,
      valid_createUser.Email,
      valid_createUser.Password);
    fixture.detectChanges();

    //Simulate Blur event on input
    let input = fixture.debugElement.nativeElement.querySelector('#email');
    input.dispatchEvent(new Event('blur'));

    expect(validatorsServiceSpy.uniquenessEmail).toHaveBeenCalled();
  }));

  it('authService register() should be called', fakeAsync(() => {
    updateForm(valid_createUser.FirstName,
      valid_createUser.LastName,
      valid_createUser.DistrictNumber,
      valid_createUser.Username,
      valid_createUser.Email,
      valid_createUser.Password);
    fixture.detectChanges();

    const button = fixture.debugElement.nativeElement.querySelector('#createUser');
    button.click();
    fixture.detectChanges();

    expect(authServiceSpy.register).toHaveBeenCalled();
  }));

  it('should route to home/catalogue if cancel', fakeAsync(() => {
    const button = fixture.debugElement.nativeElement.querySelector('#cancel');
    button.click();
    fixture.detectChanges();
    expect(routerSpy.navigate).toHaveBeenCalled();

    const navArgs = routerSpy.navigate.calls.mostRecent().args[0];
    expect(navArgs).toEqual(['home/catalogue'], 'should nav to Catalogue home page')
  }));

  it('should route to auth/login if login', fakeAsync(() => {
    const button = fixture.debugElement.nativeElement.querySelector('#loginBtn');
    button.click();
    fixture.detectChanges();

    expect(routerSpy.navigate).toHaveBeenCalled();

    const navArgs = routerSpy.navigate.calls.mostRecent().args[0];
    expect(navArgs).toEqual(['auth/login'], 'should nav to login page')
  }));
});

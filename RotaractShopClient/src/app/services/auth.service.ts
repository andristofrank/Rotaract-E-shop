import {Inject, Injectable} from '@angular/core';
import {ApiAuthService} from '../../../APIs/auth/api-auth.service';
import {ToastrService} from 'ngx-toastr';
import {Router} from '@angular/router';
import {User} from "../../../APIs/models";
import {ApiUserService} from "../../../APIs/resources/api-user.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private apiAuthService: ApiAuthService,
              private toast: ToastrService,
              private route: Router,
              private apiUserService: ApiUserService) {
  }

  register(user: User) {
    this.apiAuthService.createUser(user).subscribe(
    (response)  => {
    this.toast.success(response.toString());
    this.route.navigate(['auth/login']);
  },
    (err) => {this.toast.error(err.error.message);});
  }
  login(username: string, password: string){
   this.apiAuthService.login(username, password).subscribe(
     (response) => {
      this.store(response);
      // window.location.href= 'http://localhost:4200/home/catalogue';
      window.location.href= 'https://rotaract-eshop.web.app/home/catalogue';
    }, (err) => { this.toast.error('Username or password are incorrect!')});
  }
  logout() {
    sessionStorage.clear();
    this.toast.warning('You have been logged out');
    window.location.href= 'http://localhost:4200/home/catalogue';
    // window.location.href= 'https://rotaract-eshop.web.app/home/catalogue';
  }
  store(response) {
    sessionStorage.setItem('key', response.access_token);
  }
  get isAuthenticated(): boolean{
    return sessionStorage.getItem('key') !== null;
  }
  getUserDetails() {
    return this.apiUserService.getUserDetails();
  }
}

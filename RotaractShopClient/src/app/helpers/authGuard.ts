import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {AuthService} from '../services/auth.service';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate{
  constructor(private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot,
              state: RouterStateSnapshot) {
    const currentAccount = JSON.parse(sessionStorage.getItem('account'));
    if (currentAccount) {
      // check if route is restricted by role
      if (route.data.roles && route.data.roles.indexOf(currentAccount.Role) === -1) {
        this.router.navigate(['home/catalogue']);
        return false;
      }
      // authorized, So return true
      return true;
    }
    // not logged in so redirect to login page
    this.router.navigate(['auth/login']);
    return false;
  }
}

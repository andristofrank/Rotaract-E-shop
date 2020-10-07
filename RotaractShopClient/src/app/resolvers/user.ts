import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot} from "@angular/router";
import {Observable, of} from "rxjs";
import {AuthService} from "../services/auth.service";
import {catchError, map} from "rxjs/operators";

@Injectable()
export class UserDetailsResolver implements Resolve<any>{
  constructor(private authService: AuthService, private route: Router) {}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    if (sessionStorage.getItem('key')) {
      return this.authService.getUserDetails().pipe(
        map( response => { return response }),
        catchError( err => {

          //empty old storage
          sessionStorage.removeItem('key');
          sessionStorage.removeItem('account');

          //redirect to main page
          //window.location.href = 'http://localhost:4200/';
          window.location.href = 'https://rotaract-eshop.web.app/home/catalogue';
          return of(null);
        })
      )
    }
  }
}

import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs";
import {ResourcesService} from "../services/resources.service";

@Injectable()
export class ProductsResolver implements Resolve<any>{
  constructor(private resourcesService: ResourcesService) {}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return this.resourcesService.getProducts(1, 10);
  }
}

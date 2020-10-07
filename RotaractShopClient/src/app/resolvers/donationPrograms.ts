import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs";
import {ResourcesService} from "../services/resources.service";

@Injectable()
export class DonationsProgramsCatalogueResolver implements Resolve<any>{
  constructor(private resourcesService: ResourcesService) {}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return this.resourcesService.getDonationProgramsCatalogue();
  }
}
@Injectable()
export class DonationsProgramsAdministrationResolver implements Resolve<any>{
  constructor(private resourcesService: ResourcesService) {}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return this.resourcesService.getDonationPrograms(1, 10);
  }
}

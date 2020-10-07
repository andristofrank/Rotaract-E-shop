import { Injectable } from '@angular/core';
import {ApiValidatorsService} from "../../../APIs/validators/api-validators.service";

@Injectable({
  providedIn: 'root'
})
export class ValidatorsService {

  constructor(private apiValidatorsService: ApiValidatorsService) {}

  validateDistrictNumber(districtNo: number) {
    return this.apiValidatorsService.checkDistrictNumber(districtNo);
  }
  uniquenessEmail(email: string) {
    return this.apiValidatorsService.validateEmail(email);
  }
  uniquenessUsername(username: string) {
    return this.apiValidatorsService.validateUserName(username);
  }
}

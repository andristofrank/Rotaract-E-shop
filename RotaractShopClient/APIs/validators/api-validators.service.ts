import {Injectable, Optional} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../src/environments/environment';
import {Config} from '../config';

@Injectable({
  providedIn: 'root'
})
export class ApiValidatorsService {
  public configuration = new Config();
  constructor(private httpClient: HttpClient, @Optional() config: Config) {
    if (config) {this.configuration = config; }
  }
  checkDistrictNumber(districtNo: number){
    const DistricNumber = {
      DistrictNumber: districtNo
    }
    return this.httpClient.post(`${this.configuration.basePath}validator/districtNumber`, DistricNumber);
  }
  validateUserName(username: string){
    const Username = {
    Username: username
  }
    return this.httpClient.post(`${this.configuration.basePath}validator/username`, Username);
  }
  validateEmail(email: string) {
    const Email = {
      email: email
    }
    return this.httpClient.post(`${this.configuration.basePath}validator/email`, Email);
  }
}

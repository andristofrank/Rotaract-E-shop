import {Injectable, Optional} from '@angular/core';
import {Config} from "../config";
import {HttpClient} from "@angular/common/http";
import {Product, User} from "../models";

@Injectable({
  providedIn: 'root'
})
export class ApiUserService {
  public configuration = new Config();
  constructor(private httpClient: HttpClient, @Optional() config: Config) {
    if (config) {this.configuration = config; }
  }
  getOrders(page, pageSize) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.get(`${this.configuration.basePath}api/user/order?pageResourceParameters.pageNumber=${page}&pageResourceParameters.pageSize=${pageSize}&pageResourceParameters.fields=OrderId%2COrderItems%2CShippingDetailsModel%2CStatus%2CDateStamp%2CTotalPrice`,
      {headers, observe: "response"});
  }
  getDonations(page?: number, pageSize?: number) {
    const headers = {
      'Content-Type': 'application/x-www-form-urlencoded',
      Authorization: `Bearer ${this.configuration.accessToken}`
    }
    if (page === null || page === undefined) {
      throw new Error('Required parameter page was null or undefined when calling getProducts. ')
    }
    if (pageSize === null || pageSize === undefined) {
      throw new Error('Required parameter pageSize was null or undefined when calling getProducts. ')
    }
    return this.httpClient.get(
      `${this.configuration.basePath}api/user/donation?pageResourceParameters.pageNumber=${page}&pageResourceParameters.pageSize=${pageSize}&pageResourceParameters.fields=DonationProgramId%2C%20DonationProgramName%2C%20Total`,
      { headers,  observe: "response"});
  }
  getUserDetails() {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.get<User>(`${this.configuration.basePath}api/user/details`, {headers});
  }
}

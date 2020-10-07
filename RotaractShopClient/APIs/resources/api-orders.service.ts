import {Injectable, Optional} from '@angular/core';
import {Config} from '../config';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiOrdersService {
  public configuration = new Config();
  constructor(private httpClient: HttpClient, @Optional() config: Config) {
    if (config) {this.configuration = config; }
  }
  getOrders(status, page, pageSize) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.get(`${this.configuration.basePath}api/order/${status}?pageResourceParameters.pageNumber=${page}&pageResourceParameters.pageSize=${pageSize}&pageResourceParameters.fields=OrderId%2COrderItems%2CShippingDetailsModel%2CStatus%2CDateStamp%2CTotalPrice`,
      {headers, observe: "response"});
  }
  placeOrder(order) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.post(`${this.configuration.basePath}api/order`, order,{headers});
  }
  changeOrderStatus(order) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.put(`${this.configuration.basePath}api/order`, order,{headers});
  }
}

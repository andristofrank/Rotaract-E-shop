import {Injectable, Optional} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Config} from '../config';
import {Product} from '../models/Product';

@Injectable({
  providedIn: 'root'
})
export class ApiProductsService {
  public configuration = new Config();
  constructor(private httpClient: HttpClient, @Optional() config: Config) {
    if (config) {this.configuration = config; }
  }
  getProducts(page: number, pageSize: number){
    const headers = {
      'Content-Type': 'application/x-www-form-urlencoded',
    }

    if (page === null || page === undefined) {
      throw new Error('Required parameter page was null or undefined when calling getProducts. ')
    }

    if (pageSize === null || pageSize === undefined) {
      throw new Error('Required parameter pageSize was null or undefined when calling getProducts. ')
    }

    return this.httpClient.get(
      `${this.configuration.basePath}api/product?pageResourceParameters.pageNumber=${page}
      &pageResourceParameters.pageSize=${pageSize}
      &pageResourceParameters.fields=Name%2C%20Description%2C%20ImageRef%2C%20Price%2C%20ProductId%2C%20Inventory`,
      { headers, observe: "response"});
  }
  addProduct(product: Product) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.post(`${this.configuration.basePath}api/product`, product, {headers});
  }
  updateProduct(product: Product) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.put(`${this.configuration.basePath}api/product`, product, {headers});
  }
  deleteProduct(productId: number) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.delete(`${this.configuration.basePath}api/product/${productId}`, {headers});
  }
  addInventory(product) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.put(`${this.configuration.basePath}api/product/inventory`,
      product,
      {headers});
  }
}

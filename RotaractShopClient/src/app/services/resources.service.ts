import { Injectable } from '@angular/core';
import {Product} from '../../../APIs/models/Product';
import {ApiProductsService} from '../../../APIs/resources/api-products.service';
import {ApiDonationsService} from '../../../APIs/resources/api-donations.service';
import {DonationProgram} from '../../../APIs/models/DonationProgram';
import {ApiOrdersService} from '../../../APIs/resources/api-orders.service';
import {Donation} from '../../../APIs/models/Donation';
import {ApiUserService} from "../../../APIs/resources/api-user.service";

@Injectable({
  providedIn: 'root'
})
export class ResourcesService {

  constructor(private apiProductsService: ApiProductsService,
              private apiDonationsService: ApiDonationsService,
              private apiOrdersService: ApiOrdersService,
              private apiUserService: ApiUserService) { }
  /*
   Products management
   */
   getProducts(page, pageSize) { return this.apiProductsService.getProducts(page, pageSize); }
   addProduct(product: Product) { return this.apiProductsService.addProduct(product); }
   updateProduct(product: Product) { return this.apiProductsService.updateProduct(product); }
   deleteProduct(productId: number) { return this.apiProductsService.deleteProduct(productId); }
   addInventory(product) { return this.apiProductsService.addInventory(product);}

   /*
    Donations management
    */
  getDonationPrograms(page, pageSize) { return this.apiDonationsService.getDonationPrograms(page, pageSize); }
  addDonationProgram(donation: DonationProgram) {
    return this.apiDonationsService.addDonationProgram(donation); }
  updateDonationProgram(donation: DonationProgram) {
    return this.apiDonationsService.updateDonationProgram(donation); }
  deleteDonationProgram(donationId: number) {
    return this.apiDonationsService.deleteDonationProgram(donationId); }
  getDonationProgramsCatalogue() { return this.apiDonationsService.getDonationProgramsCatalogue(); }
  makeDonation(donation: Donation, userId: number) { return this.apiDonationsService.makeDonation(donation, userId); }

  /*
   Orders
   */
  placeOrder(order) {return this.apiOrdersService.placeOrder(order); }
  getOrders(status, page, pageSize) {return this.apiOrdersService.getOrders(status, page, pageSize); }
  changeOrderStatus(order) {return this.apiOrdersService.changeOrderStatus(order); }

  /*
  Profile
   */
  getMyOrders(page, pageSize) { return this.apiUserService.getOrders(page, pageSize); }
  getMyDonations(page, pageSize) { return this.apiUserService.getDonations(page, pageSize); }
}

import { Component, OnInit } from '@angular/core';
import {CartService} from "../../../services/cart.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {min} from "rxjs/operators";
import {Router} from "@angular/router";
import {Order} from "../../../../../APIs/models/Order";
import {ResourcesService} from "../../../services/resources.service";

@Component({
  selector: 'app-place-order',
  templateUrl: './place-order.component.html',
  styleUrls: ['./place-order.component.css']
})
export class PlaceOrderComponent implements OnInit {
shippingDetailsForm: FormGroup;
submitted = false;
  constructor(private cartService: CartService,
              private formBuilder: FormBuilder,
              private router: Router,
              private resourcesService: ResourcesService) {
    this.shippingDetailsForm = this.formBuilder.group({
      country: [null, Validators.required],
      addressLine: [null, Validators.required],
      city: [null, Validators.required],
      county_region: [null, Validators.required],
      postalCode: [null, [Validators.required, Validators.min(1)]],
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      phoneNumber: [null, Validators.required]
    });
    this.submitted = false;
  }

  ngOnInit(): void {
  }
  onSubmit() {
    this.submitted = true;
    if(this.shippingDetailsForm.invalid) {
      return;
    }
    const obj = this.shippingDetailsForm.getRawValue();
    const orderItems = this.itemList().products.map( product => {
      return {
        product: {ProductId: product.item.ProductId},
        Quantity: product.quantity };
    });
    const order: Order = {
      ShippingDetailsModel : {
        Country: obj.country,
        City: obj.city,
        AddressLine: obj.addressLine,
        CountyOrRegion: obj.county_region,
        PostalCode: obj.postalCode,
        PhoneNumber: obj.phoneNumber,
        FirstName: obj.firstName,
        LastName: obj.lastName
      },
      OrderItems: orderItems,
      TotalPrice: this.total().toFixed(2)
    }
    this.resourcesService.placeOrder(order).subscribe(
      response => { this.cartService.emptyCart();},
      (err) => {
        this.submitted = false;
        },
      () => this.router.navigate(['home/profile/orders'])
    )
  }
  cancel() {
    this.router.navigate(['home/catalogue']);
  }
  itemList() {
    return this.cartService.cart();
  }
  total() {
   return this.cartService.getTotal();
  }
  get f() {
    return this.shippingDetailsForm.controls;
  }
}

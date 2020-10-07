import { Injectable } from '@angular/core';
import {ProductItem} from "../../../APIs/models";
import {ToastrService} from "ngx-toastr";
import {AuthService} from "./auth.service";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private toast: ToastrService,
              private authService: AuthService,
              private route: Router) {
  }

  addProductItem(item: ProductItem) {
    const cartList = this.cart();
    if( cartList.products.find(product => product.item.ProductId === item.item.ProductId)) {
      for ( let product of cartList.products){
        if ( product.item.ProductId === item.item.ProductId ) {
          product.quantity = product.quantity + item.quantity;
        }
      }
    } else {
      cartList.products.push(item);
    }
    this.updateCart(cartList);

    if (this.authService.isAuthenticated) {
      this.toast.success(item.quantity + ' ' +
        item.item.Name +
        ' has been added in the cart');
    } else {
      this.route.navigateByUrl('auth/login');
    }
  }
  removeProductItem(item: ProductItem) {
    const cartList = this.cart();
    let indexOf = -1;
    for (const [i, product] of cartList.products.entries()){
      if(product.item.ProductId === item.item.ProductId) {
        indexOf = i;
      }
    }
    if(indexOf > -1) {
      cartList.products.splice(indexOf, 1);
    }
    this.updateCart(cartList);
    this.toast.warning(item.quantity + ' ' +
      item.item.Name +
      ' has been removed from the cart')
  }
  changeQuantity(productItem: ProductItem, quantity: number) {
    const cartList = this.cart();
    /*
    find product and assign the new quantity if its changed
     */
    if(cartList.products.find(
      product => product.item.ProductId === productItem.item.ProductId)
      .quantity === quantity) { return; }
    cartList.products.find(
      product => product.item.ProductId === productItem.item.ProductId)
      .quantity = quantity;
    this.updateCart(cartList);
  }
  updateCart(cartList) {
    /*
    set the storage item cart to the new Json string value
     */
    localStorage.setItem('cart', JSON.stringify(cartList));
    if ( this.authService.isAuthenticated) {
      this.toast.success('The cart has been updated, new total: ' +
        this.getTotal());
    }
  }
  emptyCart() {
    localStorage.removeItem('cart');
  }
  getTotal() {
    const cartList = this.cart();
    // calculate the total
    const total = cartList.products.reduce((acc, item) =>{
      return acc += item.item.Price * item.quantity;
    }, 0);
    return total;
  }
  cart() {
    // get cart object from localStorage
    const cart = JSON.parse(localStorage.getItem('cart'));
    if(cart) {
      return cart
    } else {
      // return empty object with empty list of products
      return { products: [] }
    }
  }
}

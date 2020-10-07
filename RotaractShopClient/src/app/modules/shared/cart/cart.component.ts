import { Component, OnInit } from '@angular/core';
import {CartService} from "../../../services/cart.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {Router} from "@angular/router";

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartList;
  constructor(private cartService: CartService,
              private modalService:NgbModal,
              private router: Router) { }

  ngOnInit(): void {
    this.getCartList();
  }
  getCartList() {
    this.cartList = this.cartService.cart();
  }
  // Products
  getProductPrice(item) {
    if(item.item.hasOwnProperty('ProductId')){
      return item.quantity * item.item.Price;
    }
  }
  removeItem(product) {
    this.cartService.removeProductItem(product);
    this.getCartList();
    if (this.cartList.products.length < 1) {
      this.modalService.dismissAll();
      this.router.navigateByUrl('home/catalogue');
    }
  }
  quantityChange(productItem) {
    if(productItem.quantity == 0) {
      this.removeItem(productItem);
    } else {
      this.cartService.changeQuantity(productItem, productItem.quantity);
    }
  }
  get total() {
    return this.cartService.getTotal();
  }
  get itemsCount() {
    return this.cartService.cart().products.length;
  }
  placeOrder() {
    this.modalService.dismissAll();
    this.router.navigateByUrl('home/place-order');
  }
  close() {
    this.modalService.dismissAll();
    this.router.navigateByUrl('home/catalogue');
  }
}


import {Component, Input, OnInit} from '@angular/core';
import {CartService} from '../../../../../services/cart.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  quantity = 1;
  @Input() product;
  constructor(private cartService: CartService,
              private modalService: NgbModal) {
  }

  ngOnInit(): void {
  }
  addToCart() {
    this.cartService.addProductItem({ item: this.product, quantity: this.quantity});
    this.close();
  }
  close() {
    this.modalService.dismissAll();
  }
}

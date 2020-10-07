import { Component, OnInit } from '@angular/core';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-products-overview',
  templateUrl: './products-overview.component.html',
  styleUrls: ['./products-overview.component.css']
})
export class ProductsOverviewComponent implements OnInit {
products;
modalOptions;
page: number = 1;
pageSize: number = 10;
totalPages;
  constructor(private assetsService: ResourcesService,
              private modalService: NgbModal,
              private activatedRoute: ActivatedRoute) {
    this.modalOptions = {
      backdrop: 'static',
      backdropClass: 'customBackdrop',
      size: 'lg'
    };
    this.activatedRoute.data.subscribe(
      response => {
        this.products = response.products.body;
        this.totalPages = JSON.parse(response.products.headers.get('x-pagination')).totalPages; }
    )
  }

  ngOnInit(): void {
  }
  /*
  Product Functionality on Overview
   */
  getProducts() {
    this.assetsService.getProducts(this.page, this.pageSize).subscribe(
      response => {
        this.products = response.body;
        this.totalPages = JSON.parse(response.headers.get('x-pagination')).totalPages;
      }
    )
  }
  delete(productId){
    this.assetsService.deleteProduct(productId).subscribe(
      response => this.getProducts()
    )
  }
  update(changed) {
    if(changed) {
      this.getProducts();
    }
  }
  /*
   Modal
   */
   openModal(targetModal) {
    this.modalService.open(targetModal, this.modalOptions);
   }
   /*
   Pagination
    */
  pageChanged(page) {
    this.page = page;
    this.getProducts();
  }
  pageSizeChanged(pageSize) {
    this.pageSize = pageSize;
    this.getProducts();
  }


}

import {Component, Input, OnInit} from '@angular/core';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
@Input() products;
error = false;
modalOptions;
page: number = 1;
pageSize: number = 10;
@Input() totalElements;
  constructor(private resourcesService: ResourcesService,
              private modalService: NgbModal) {
    this.modalOptions = {
      backdropClass: 'customBackdrop',
      size: 'lg'
    };
  }

  ngOnInit(): void {}
  getProducts() {
    this.resourcesService.getProducts(this.page, this.pageSize).subscribe(
      (response) => {
        this.products = response.body;
        this.totalElements = JSON.parse(response.headers.get('x-pagination')).totalPages;
      }
    )
  }
  pageChanged(page) {
    this.page = page;
    this.getProducts();
  }
  pageSizeChanged(pageSize) {
    this.pageSize = pageSize;
    this.getProducts();
  }
  openModal(target) {
    this.modalService.open(target, this.modalOptions);
  }
}

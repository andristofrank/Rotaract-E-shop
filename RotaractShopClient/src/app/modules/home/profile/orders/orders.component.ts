import { Component, OnInit } from '@angular/core';
import {ResourcesService} from "../../../../services/resources.service";

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
orders;
totalElements;
page = 1;
pageSize = 5;
  constructor(private resourcesService: ResourcesService) {
  }

  ngOnInit(): void {
    this.getOrders();
  }
  getOrders() {
    this.resourcesService.getMyOrders(this.page, this.pageSize).subscribe(
      response => {
        this.orders = response.body;
        this.totalElements = JSON.parse(response.headers.get('x-pagination')).totalPages;
        console.log(this.totalElements);
      }
    )
  }
  pageChanged(page) {
    this.page = page;
    this.getOrders();
  }
  pageSizeChanged(pageSize) {
    this.pageSize = pageSize;
    this.getOrders();
  }
}

import {Component, Input, OnInit} from '@angular/core';
import {ResourcesService} from "../../../../services/resources.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {Status} from "../../../../../../APIs/models/Status";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  @Input() status;
  page = 1;
  pageSize = 5;
  totalElements;
  orders;
  modalOptions;
  constructor(private resourcesService: ResourcesService,
              private modalService: NgbModal,
              private toastrService: ToastrService) {
    this.modalOptions = {
      centered: true,
      size: 'lg'
    }
  }

  ngOnInit(): void {
    this.getOrders();
  }
  getOrders() {
    this.resourcesService.getOrders(this.status, this.page, this.pageSize).subscribe(
      response => {
        this.orders = response.body;
        this.totalElements = JSON.parse(response.headers.get('x-pagination')).totalPages;}
    )
  }
  changeStatus(order) {
    switch (order.Status) {
      case Status.PENDING :  this.toastrService.warning('Please check the products availability'); break;
      case Status.CONFIRMED : this.resourcesService.changeOrderStatus({ OrderId: order.OrderId, Status: Status.INPROGRESS}).subscribe(
        res => { this.getOrders(); this.toastrService.success('Order has been moved to In Progress'); }),
        (err) => { this.toastrService.error('Something went wrong.')}; break;
      case Status.INPROGRESS : this.resourcesService.changeOrderStatus({ OrderId: order.OrderId, Status: Status.SHIPPING}).subscribe(
        res => { this.getOrders(); this.toastrService.success('Order has been moved to Shipping'); }),
        (err) => { this.toastrService.error('Something went wrong.')}; break;
      case Status.SHIPPING : this.resourcesService.changeOrderStatus({ OrderId: order.OrderId, Status: Status.COMPLETED}).subscribe(
        res => { this.getOrders(); this.toastrService.success('Order has been moved to Shipping'); }),
        (err) => { this.toastrService.error('Something went wrong.')}; break;
      default: this.toastrService.warning('Not allowed!')
    }
  }
  cancelOrder(order) {
    this.resourcesService.changeOrderStatus({ OrderId: order.OrderId, Status: Status.CANCELLED}).subscribe(
      res => { this.getOrders(); this.toastrService.success(res.toString()); },
      (err) => { this.toastrService.error('Something went wrong.')}
      );
  }
  update(changed) {
    if(changed) {
      this.getOrders();
    }
  }
  openModal(target) {
    this.modalService.open(target, this.modalOptions);
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

import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ResourcesService} from "../../../../services/resources.service";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-product-inventory',
  templateUrl: './product-inventory.component.html',
  styleUrls: ['./product-inventory.component.css']
})
export class ProductInventoryComponent implements OnInit {
@Input() product;
@Output() inventoryChanged = new EventEmitter<any>()
  amount
  submitted = false;
  constructor(private resourcesService: ResourcesService,
              private modalService: NgbModal,
              private toastrService: ToastrService) { }

  ngOnInit(): void {
  }
  updateInventory() {
    this.submitted = true;
  const product = {
    ProductId: this.product.ProductId,
    Inventory: this.amount
  }
    this.resourcesService.addInventory(product).subscribe(
      res => {
        this.inventoryChanged.emit(true);
        this.modalService.dismissAll();
        this.toastrService.success(`${this.amount} ${this.product.Name} has been added to Inventory`);
     },
        error => {this.modalService.dismissAll(); this.toastrService.warning('Something went wrong.')}
    )
  }

  close() {
  this.modalService.dismissAll();
  }

}

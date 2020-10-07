import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {OrdersRoutingModule} from "./orders-routing.module";
import { OrdersOverviewComponent } from './orders-overview/orders-overview.component';
import {NgbNavModule, NgbPopoverModule} from "@ng-bootstrap/ng-bootstrap";
import {FormsModule} from "@angular/forms";
import { OrdersComponent } from './orders/orders.component';
import {SharedComponentsModule} from "../../shared/shared.module";
import {ProductsModule} from "../products/products.module";



@NgModule({
  declarations: [OrdersOverviewComponent, OrdersComponent],
  imports: [
    OrdersRoutingModule,
    CommonModule,
    NgbPopoverModule,
    FormsModule,
    NgbNavModule,
    SharedComponentsModule,
    ProductsModule
  ]
})
export class OrdersModule { }

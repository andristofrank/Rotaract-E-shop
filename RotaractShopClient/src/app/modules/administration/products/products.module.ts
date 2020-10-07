import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsOverviewComponent } from './products-overview/products-overview.component';
import {ProductsRoutingModule} from './products-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {SharedComponentsModule} from '../../shared/shared.module';
import {NgbPopoverModule} from '@ng-bootstrap/ng-bootstrap';
import { ProductFormComponent } from './product-form/product-form.component';
import { ProductInventoryComponent } from './product-inventory/product-inventory.component';


@NgModule({
  declarations: [ProductsOverviewComponent, ProductFormComponent, ProductInventoryComponent],
  exports: [
    ProductInventoryComponent
  ],
  imports: [
    ProductsRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    SharedComponentsModule,
    NgbPopoverModule,
    FormsModule
  ]
})
export class ProductsModule { }

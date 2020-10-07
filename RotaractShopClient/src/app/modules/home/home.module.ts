import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CatalogueComponent } from './catalogue/catalogue.component';
import {CatalogueRouting} from './home-routing.module';
import {SharedComponentsModule} from '../shared/shared.module';
import { DonationProgramsComponent } from './catalogue/donation-programs/donation-programs.component';
import {NgbCarouselModule} from '@ng-bootstrap/ng-bootstrap';
import { ProductsComponent } from './catalogue/products/products.component';
import { ProductDetailsComponent } from './catalogue/products/product-details/product-details.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {DonationDetailsComponent} from "./catalogue/donation-programs/donation-details/donation-details.component";
import { PlaceOrderComponent } from './place-order/place-order.component';
import { HomeComponent } from './home/home.component';
import { DonateComponent } from './catalogue/donation-programs/donate/donate.component';

@NgModule({
    declarations: [CatalogueComponent,
      DonationProgramsComponent,
      ProductsComponent,
      ProductDetailsComponent,
      DonationDetailsComponent,
      PlaceOrderComponent,
      HomeComponent,
      DonateComponent],
  imports: [
    CatalogueRouting,
    CommonModule,
    SharedComponentsModule,
    NgbCarouselModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class HomeModule { }

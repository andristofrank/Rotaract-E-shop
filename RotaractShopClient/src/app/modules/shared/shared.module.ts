import {NgModule} from '@angular/core';
import {NavigationBarComponent} from './navigation-bar/navigation-bar.component';
import { ImageLoaderComponent } from './image-loader/image-loader.component';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {NgxImageCompressService} from 'ngx-image-compress';
import { CartComponent } from './cart/cart.component';
import {RouterModule} from "@angular/router";
import { PaginationComponent } from './pagination/pagination.component';
import {NgbPaginationModule} from "@ng-bootstrap/ng-bootstrap";
import { ErrorPageComponent } from './error-page/error-page.component';
@NgModule({
  declarations: [NavigationBarComponent,
    ImageLoaderComponent,
    CartComponent,
    PaginationComponent,
    ErrorPageComponent],
  exports: [NavigationBarComponent,
    ImageLoaderComponent, PaginationComponent],
  imports: [CommonModule, FormsModule, RouterModule, NgbPaginationModule],
  providers: [NgxImageCompressService]
})
export class SharedComponentsModule {}

import {RouterModule, Routes} from '@angular/router';
import {ProductsOverviewComponent} from './products-overview/products-overview.component';
import {NgModule} from '@angular/core';
import {ProductsResolver} from "../../../resolvers/products";
const routes: Routes = [
  {path: '', children: [
      {path: '',
        resolve: { products: ProductsResolver },
        component: ProductsOverviewComponent }
    ]}
]
@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ProductsRoutingModule {}

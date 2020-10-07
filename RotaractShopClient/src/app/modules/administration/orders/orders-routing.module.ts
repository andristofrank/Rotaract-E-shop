import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {OrdersOverviewComponent} from "./orders-overview/orders-overview.component";

const routes: Routes = [
  {path: '', children: [
      {path: '',
        component: OrdersOverviewComponent}
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
export class OrdersRoutingModule {}

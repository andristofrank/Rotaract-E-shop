import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {OrdersComponent} from "./orders/orders.component";
import {ProfileComponent} from "./profile/profile.component";
import {DonationsComponent} from "./donations/donations.component";

const profileRoutes: Routes = [
  {path: '', component: ProfileComponent, children: [
      {path: '', redirectTo: '/orders', pathMatch: 'full'},
      {path: 'orders',
        component: OrdersComponent},
      {path: 'donations',
        component: DonationsComponent}
    ]},

];

@NgModule({
  imports: [
    RouterModule.forChild(profileRoutes)
  ],
  exports: [
    RouterModule
  ]
})

export class ProfileRoutingModule {}

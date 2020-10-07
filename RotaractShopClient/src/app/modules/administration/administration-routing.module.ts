import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {AdministrationComponent} from "./administration/administration.component";
import {UserDetailsResolver} from "../../resolvers/user";

const administrationRoutes: Routes = [
  {path: '', resolve: { user: UserDetailsResolver},
    component: AdministrationComponent, children: [
      {path: '', redirectTo: 'orders', pathMatch: 'full'},
      {path: 'products',
        loadChildren: () => import('./products/products.module').then( m => m.ProductsModule)},
      {path: 'donationPrograms',
        loadChildren: () => import('./donationPrograms/donationsPrograms.module').then(m => m.DonationsProgramsModule)},
      {path: 'orders', loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule)}
    ]}
];

@NgModule({
  imports: [
    RouterModule.forChild(administrationRoutes)
  ],
  exports: [
    RouterModule
  ]
})

export class AdministrationRoutingModule {}

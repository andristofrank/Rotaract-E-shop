import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {CatalogueComponent} from './catalogue/catalogue.component';
import {PlaceOrderComponent} from "./place-order/place-order.component";
import {AuthGuard} from "../../helpers/authGuard";
import {HomeComponent} from "./home/home.component";
import {ProductsResolver} from "../../resolvers/products";
import {UserDetailsResolver} from "../../resolvers/user";
import {DonationsProgramsCatalogueResolver} from "../../resolvers/donationPrograms";

const homeRoutes: Routes = [
  {path: '',
    resolve: { user: UserDetailsResolver},
    component: HomeComponent, children: [
      {path: '', redirectTo: 'catalogue', pathMatch: 'full' },
      {path: 'catalogue',
        resolve: {
          products: ProductsResolver,
          donationPrograms: DonationsProgramsCatalogueResolver},
        component: CatalogueComponent },
      {path: 'place-order',
        component: PlaceOrderComponent,
        canActivate: [AuthGuard]},
      {path: 'profile', loadChildren: () =>
          import('./profile/profile.module').then(m => m.ProfileModule),
        canActivate: [AuthGuard]}
    ]}
];

@NgModule({
  imports: [
    RouterModule.forChild(homeRoutes)
  ],
  exports: [
    RouterModule
  ],
  providers: [
    DonationsProgramsCatalogueResolver,
    ProductsResolver,
    UserDetailsResolver
  ]
})


export class CatalogueRouting {}

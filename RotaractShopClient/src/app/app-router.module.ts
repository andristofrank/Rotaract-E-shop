import {RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import {AuthGuard} from './helpers/authGuard';
import {Role} from '../../APIs/models/Role';
import {ErrorPageComponent} from "./modules/shared/error-page/error-page.component";

// Routes
const appRoutes: Routes = [
  {path: '', redirectTo: '/home/catalogue', pathMatch: 'full'},
  {path: 'auth', loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule)},
  {path: 'home', loadChildren: () => import('./modules/home/home.module').then(m => m.HomeModule)},
  {path: 'administration', loadChildren: () =>
      import('./modules/administration/administration.module').then(m => m.AdministrationModule),
    canActivate: [AuthGuard],
    data: { roles: [Role.Administrator]}},
  {path: '**', component: ErrorPageComponent}
];

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes)
    ],
    exports: [
        RouterModule
    ],
  providers: []
})


export class AppRouterModule {}

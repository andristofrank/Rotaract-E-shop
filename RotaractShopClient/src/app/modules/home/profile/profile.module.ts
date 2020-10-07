import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders/orders.component';
import {ProfileRoutingModule} from "./profile-routing.module";
import {NgbPopoverModule} from "@ng-bootstrap/ng-bootstrap";
import { DonationsComponent } from './donations/donations.component';
import { ProfileComponent } from './profile/profile.component';
import {SharedComponentsModule} from "../../shared/shared.module";



@NgModule({
  declarations: [OrdersComponent, DonationsComponent, ProfileComponent],
    imports: [
        ProfileRoutingModule,
        CommonModule,
        NgbPopoverModule,
        SharedComponentsModule
    ]
})
export class ProfileModule { }

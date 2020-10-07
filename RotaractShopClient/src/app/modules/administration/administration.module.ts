import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AdministrationRoutingModule} from './administration-routing.module';
import {HomeModule} from '../home/home.module';
import {SharedComponentsModule} from '../shared/shared.module';
import { AdministrationComponent } from './administration/administration.component';



@NgModule({
  declarations: [AdministrationComponent],
  imports: [
    CommonModule,
    AdministrationRoutingModule,
    HomeModule,
    SharedComponentsModule
  ]
})
export class AdministrationModule { }

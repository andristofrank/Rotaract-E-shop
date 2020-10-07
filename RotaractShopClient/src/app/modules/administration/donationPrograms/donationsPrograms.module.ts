import {NgModule} from '@angular/core';
import {DonationsProgramsOverviewComponent} from './donationsPrograms-overview/donationsPrograms-overview.component';
import {SharedComponentsModule} from '../../shared/shared.module';
import {CommonModule} from '@angular/common';
import {NgbDatepickerModule, NgbPopoverModule} from '@ng-bootstrap/ng-bootstrap';
import {DonationsProgramsRoutingModule} from './donationsPrograms-routing.module';
import { DonationProgramFormComponent } from './donationProgram-form/donationProgram-form.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

@NgModule({
  declarations: [DonationsProgramsOverviewComponent, DonationProgramFormComponent],
    imports: [
        DonationsProgramsRoutingModule,
        SharedComponentsModule,
        CommonModule,
        NgbPopoverModule,
        NgbDatepickerModule,
        ReactiveFormsModule,
        FormsModule
    ]
})
export class DonationsProgramsModule {

}

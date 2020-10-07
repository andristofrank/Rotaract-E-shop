import {RouterModule, Routes} from '@angular/router';
import {DonationsProgramsOverviewComponent} from './donationsPrograms-overview/donationsPrograms-overview.component';
import {NgModule} from '@angular/core';
import {DonationsProgramsAdministrationResolver} from "../../../resolvers/donationPrograms";

const routes: Routes = [
  {path: '', children: [
      {path: '',
        resolve: { donationPrograms: DonationsProgramsAdministrationResolver},
        component: DonationsProgramsOverviewComponent}
    ]}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ],
  providers: [
    DonationsProgramsAdministrationResolver
  ]
})
export class DonationsProgramsRoutingModule {}

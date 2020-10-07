import {ModuleWithProviders, NgModule, Optional, SkipSelf} from '@angular/core';
import {Config} from './config';
import {HttpClient} from '@angular/common/http';
import {ApiOrdersService} from "./resources/api-orders.service";
import {ApiDonationsService} from "./resources/api-donations.service";
import {ApiProductsService} from "./resources/api-products.service";
import {ApiAuthService} from "./auth/api-auth.service";
import {ApiValidatorsService} from "./validators/api-validators.service";
import {ApiUserService} from "./resources/api-user.service";

@NgModule({
  imports: [],
  declarations: [],
  exports: [],
  providers: [ApiOrdersService,
    ApiDonationsService,
    ApiProductsService,
    ApiAuthService,
    ApiValidatorsService,
    ApiUserService]
})
export class ApiModule {
  public static forRoot(configurationFactory: () => Config): ModuleWithProviders {
    return {
      ngModule: ApiModule,
      providers: [ { provide: Config, useFactory: configurationFactory }]
    };
  }
  constructor(@Optional() @SkipSelf() parentModule: ApiModule,
              @Optional() httpClient: HttpClient) {
    if (parentModule) {
      throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
    }
    if (!httpClient) {
      throw new Error('You need to import the HttpClientModule in your AppModule!');
    }
  }
}

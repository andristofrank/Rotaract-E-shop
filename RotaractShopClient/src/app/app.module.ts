import { BrowserModule } from '@angular/platform-browser';
import {ErrorHandler, Inject, Injectable, InjectionToken, NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRouterModule } from './app-router.module';
import {ApiValidatorsService} from '../../APIs/validators/api-validators.service';
import {AuthService} from './services/auth.service';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {ToastrModule} from 'ngx-toastr';
import {ApiModule} from '../../APIs/api.module';
import {Config, ConfigParameters} from '../../APIs/config';
import {environment} from '../environments/environment';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {SharedComponentsModule} from "./modules/shared/shared.module";
import {HttpErrorInterceptor} from "./helpers/ErrorInterceptor";

@NgModule({
  declarations: [
    AppComponent,
  ],
    imports: [
        NgbModule,
        BrowserModule,
        HttpClientModule,
        AppRouterModule,
        ApiModule.forRoot(apiConfigFactory),
        BrowserAnimationsModule,
        ToastrModule.forRoot({
            timeOut: 2000,
            positionClass: 'toast-bottom-left',
            preventDuplicates: true,
        }),
        SharedComponentsModule
    ],
  providers: [
    AuthService,
    ApiValidatorsService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
export function apiConfigFactory() {
  const params: ConfigParameters = {
    basePath: environment.apiUrl,
    accessToken: sessionStorage.getItem('key')
  };
  return new Config(params);
}


import {HttpParameterCodec} from '@angular/common/http';

export interface ConfigParameters {
  accessToken?: string| (() => string);
  basePath?: string;
}
export class Config {
  accessToken?: string| (() => string);
  basePath?: string;
  constructor(configParameters: ConfigParameters = {}) {
    this.accessToken = configParameters.accessToken;
    this.basePath = configParameters.basePath;
  }
}

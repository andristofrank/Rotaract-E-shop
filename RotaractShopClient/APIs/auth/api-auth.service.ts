import {Inject, Injectable, Optional} from '@angular/core';
import {HttpClient, HttpEvent, HttpHeaders, HttpParameterCodec, HttpParams, HttpResponse} from '@angular/common/http';
import {environment} from '../../src/environments/environment';
import {Config} from '../config';
import {Observable} from 'rxjs';
import {User} from "../models";

@Injectable({
  providedIn: 'root'
})
export class ApiAuthService {
public configuration = new Config();
  constructor(private httpClient: HttpClient, @Optional() config: Config) {
    if (config) { this.configuration = config; }
  }
  public createUser( user: User): Observable<string> {
    if (user === null || user === undefined ) {
      throw new Error('Required User was null or undefined when calling register.');
    }
    return this.httpClient.post<string>(`${this.configuration.basePath}auth/createUser`, user);
  }
  public login( username: string, password: string): Observable<any> {
    if (username === null || password === null || username === undefined || password === undefined) {
      throw new Error('Required User was null or undefined when calling register.');
    }
    const headers = {
      'Content-type': 'application/x-www-form-urlencoded'
    };
    const body = new HttpParams()
    .set('username', username)
    .set('password', password)
    .set('grant_type', 'password');

    return this.httpClient.post<any>(`${this.configuration.basePath}auth/login`,  body, {headers});
  }
}

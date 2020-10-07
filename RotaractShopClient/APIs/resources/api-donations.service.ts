import {Injectable, Optional} from '@angular/core';
import {Config} from '../config';
import {HttpClient, HttpParams} from '@angular/common/http';
import {DonationProgram} from '../models/DonationProgram';
import {Donation} from '../models/Donation';

@Injectable({
  providedIn: 'root'
})
export class ApiDonationsService {
  public configuration = new Config();

  constructor(private httpClient: HttpClient, @Optional() config: Config) {
    if (config) {
      this.configuration = config;
    }
  }
  getDonationProgramsCatalogue() {
    return this.httpClient.get(`${this.configuration.basePath}api/donationProgram/catalogue`);
  }
  getDonationPrograms(page?: number, pageSize?: number) {
    const headers = {
      'Content-Type': 'application/x-www-form-urlencoded',
      Authorization: `Bearer ${this.configuration.accessToken}`
    }
    if (page === null || page === undefined) {
      throw new Error('Required parameter page was null or undefined when calling getProducts. ')
    }
    if (pageSize === null || pageSize === undefined) {
      throw new Error('Required parameter pageSize was null or undefined when calling getProducts. ')
    }
    return this.httpClient.get(
      `${this.configuration.basePath}api/donationProgram?pageResourceParameters.pageNumber=${page}
      &pageResourceParameters.pageSize=${pageSize}
      &pageResourceParameters.fields=DonationProgramId%2C%20DonationProgramName%2C%20Description%2C%20ImageRef%2C%20StartDate%2C%20EndDate%2C%20Total`,
      { headers,  observe: "response"});
  }
  addDonationProgram(donationProgram: DonationProgram) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.post(
      `${this.configuration.basePath}api/donationProgram`, donationProgram,
      {headers});
  }
  updateDonationProgram(donationProgram: DonationProgram) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.put(
      `${this.configuration.basePath}api/donationProgram`, donationProgram, {headers});
  }
  deleteDonationProgram(donationProgramId: number) {
    const headers = {
      Authorization: `Bearer ${this.configuration.accessToken}`
    };
    return this.httpClient.delete(
      `${this.configuration.basePath}api/donationProgram/${donationProgramId}`, {headers});
  }
  makeDonation(donation: Donation, userId) {
    return this.httpClient.post(`${this.configuration.basePath}api/donation/${userId}`, donation);
  }
}

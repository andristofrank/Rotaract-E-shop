import { Component, OnInit } from '@angular/core';
import {ResourcesService} from "../../../../services/resources.service";

@Component({
  selector: 'app-donations',
  templateUrl: './donations.component.html',
  styleUrls: ['./donations.component.css']
})
export class DonationsComponent implements OnInit {
page = 1;
pageSize = 5;
donations;
totalElements;
  constructor( private resourcesService: ResourcesService) {
  }

  ngOnInit(): void {
    this.getDonations();
  }
  getDonations() {
    this.resourcesService.getMyDonations(this.page, this.pageSize).subscribe(
      response => {
        this.donations = response.body;
        this.totalElements = JSON.parse(response.headers.get('x-pagination')).totalPages;
      }
    )
  }
  pageChanged(page) {
    this.page = page;
    this.getDonations();
  }
  pageSizeChanged(pageSize) {
    this.pageSize = pageSize;
    this.getDonations();
  }
}

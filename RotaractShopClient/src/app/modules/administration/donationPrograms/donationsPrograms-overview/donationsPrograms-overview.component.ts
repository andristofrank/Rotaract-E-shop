import { Component, OnInit } from '@angular/core';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ActivatedRoute} from "@angular/router";
import {DateFormat} from "../../../../helpers/dateFormat";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-donations-overview',
  templateUrl: './donationsPrograms-overview.component.html',
  styleUrls: ['./donationsPrograms-overview.component.css']
})
export class DonationsProgramsOverviewComponent implements OnInit {
donationPrograms;
modalOptions;
page = 1;
pageSize = 10;
totalElements;
  constructor(private resourcesService: ResourcesService,
              private modalService: NgbModal,
              private activatedRoute: ActivatedRoute,
              private dateFormat: DateFormat,
              private toastrService: ToastrService) {
    this.modalOptions = {
      backdrop: 'static',
      backdropClass: 'customBackdrop',
      size: 'lg'
    };
    this.activatedRoute.data.subscribe(
      response => {
        this.donationPrograms =  response.donationPrograms.body;
        this.totalElements = JSON.parse(response.donationPrograms.headers.get('x-pagination')).totalPages;}
    )
  }

  ngOnInit(): void {
  }
  getDonationPrograms() {
    this.resourcesService.getDonationPrograms(this.page, this.pageSize).subscribe(
      (response) => {
        this.donationPrograms = response.body;
        this.totalElements = JSON.parse(response.headers.get('x-pagination')).totalPages; },
      (err) => { this.toastrService.error(err.error.message); }
    );
  }
  date(date) {
    return date.split('T')[0];
  }
  delete(donationProgramId) {
    this.resourcesService.deleteDonationProgram(donationProgramId).subscribe(
      (res) =>
      { this.toastrService.success('The donation program has been deleted')},
    (err) => { this.toastrService.error(err.error.message); },
      () => { this.getDonationPrograms(); }
    );
  }

  open(target){
    this.modalService.open(target, this.modalOptions);
  }

  status(endDate, startDate) {
    const today = new Date();
    return new Date(endDate) > today && today > new Date(startDate);
  }
  pageChanged(page) {
    this.page = page;
    this.getDonationPrograms();
  }
  pageSizeChanged(pageSize) {
    this.pageSize = pageSize;
    this.getDonationPrograms();
  }

}

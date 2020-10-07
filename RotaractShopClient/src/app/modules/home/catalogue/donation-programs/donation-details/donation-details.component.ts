import {Component, Input, OnInit} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-donation-details',
  templateUrl: './donation-details.component.html',
  styleUrls: ['./donation-details.component.css']
})
export class DonationDetailsComponent implements OnInit {
  @Input() donationProgram;
  modalOptions;
  constructor(private modalService: NgbModal) {
    this.modalOptions = {
      backdropClass: 'customBackdrop',
      size: 'lg'
    };
  }
  ngOnInit(): void {}
  close() {
    this.modalService.dismissAll();
  }
  openModal(target){
    this.modalService.open(target, this.modalOptions);
  }
}

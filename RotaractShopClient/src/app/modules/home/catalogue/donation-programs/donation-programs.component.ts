import {Component, Input, OnInit} from '@angular/core';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbCarouselConfig, NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-donation-programs',
  templateUrl: './donation-programs.component.html',
  styleUrls: ['./donation-programs.component.css'],
  providers: [NgbCarouselConfig]
})
export class DonationProgramsComponent implements OnInit {
@Input() donationPrograms;
modalOptions;
  constructor(private assetsService: ResourcesService,
              private config: NgbCarouselConfig,
              private modalService: NgbModal) {
    this.modalOptions = {
      backdropClass: 'customBackdrop',
      size: 'lg'
    };
  }

  ngOnInit(): void {
  }
  openModal(target){
    this.modalService.open(target, this.modalOptions);
  }
}

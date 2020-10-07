import {Component, Input, OnInit} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ResourcesService} from '../../../../../services/resources.service';
import {Donation, User} from '../../../../../../../APIs/models';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-donate',
  templateUrl: './donate.component.html',
  styleUrls: ['./donate.component.css']
})
export class DonateComponent implements OnInit {
  @Input() donationProgram;
  amount;
  submitted = false;
  constructor(private modalService: NgbModal,
              private resourcesService: ResourcesService,
              private toastrService: ToastrService ){ }

  ngOnInit(): void {}
  makeDonation() {
    let userId = 0;
    if(sessionStorage.getItem('account')) {
      const account: User = JSON.parse(sessionStorage.getItem('account'));
      if(account) {
        userId = account.UserId;
      }
    }
    this.submitted = true;
    const donationItem: Donation = {
      donationProgram: {
        DonationProgramId: this.donationProgram.DonationProgramId },
      amount: this.amount,
    };
    this.resourcesService.makeDonation(donationItem, userId).subscribe(
      (response) => { this.modalService.dismissAll();
                      this.toastrService.success('Thank you for your donation!'); },
      (err) => { alert(err); }
    );
  }
  close() {
    this.modalService.dismissAll();
  }
}

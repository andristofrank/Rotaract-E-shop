import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {DonationProgram} from '../../../../../../APIs/models/DonationProgram';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbCalendar, NgbDate, NgbDateParserFormatter, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {DateFormat} from '../../../../helpers/dateFormat';
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-donationProgram-form',
  templateUrl: './donationProgram-form.component.html',
  styleUrls: ['./donationProgram-form.component.css']
})
export class DonationProgramFormComponent implements OnInit {
@Input() donationProgram: DonationProgram;
@Output() donationProgramChange = new EventEmitter<any>();
donationProgramForm: FormGroup;
image: string;
submitted = false;
header;
  hoveredDate: NgbDate | null = null;
  fromDate: NgbDate | null;
  toDate: NgbDate | null;
  constructor(private resourcesService: ResourcesService,
              private formBuilder: FormBuilder,
              private modalService: NgbModal,
              private dateFormat: DateFormat,
              private calendar: NgbCalendar,
              public formatter: NgbDateParserFormatter,
              private toastrService: ToastrService) {

  }

  ngOnInit(): void {
    if (this.donationProgram) {
      this.header = 'Update programme: ' + this.donationProgram.DonationProgramName;
      this.donationProgramForm = this.formBuilder.group({
        donationProgramName: new FormControl(this.donationProgram.DonationProgramName,
          {validators: Validators.required}),
        description: new FormControl(this.donationProgram.Description)
      });
      this.image = this.donationProgram.ImageRef;
      this.fromDate = this.dateFormat.dateFromString(this.donationProgram.StartDate);
      this.toDate = this.dateFormat.dateFromString(this.donationProgram.EndDate);
    } else {
      this.header = 'Add new Donation Program';
      this.donationProgramForm = this.formBuilder.group({
        donationProgramName: new FormControl(null,
          {validators: Validators.required}),
        description: new FormControl(null)
      });
      this.fromDate = this.calendar.getToday();
      this.toDate = this.calendar.getNext(this.calendar.getToday(), 'd', 40);
      this.image = null;
    }
  }
  onSubmit() {
    this.submitted = true;
    // stop if form is invalid
    if (this.donationProgramForm.invalid) {
      return;
    }
    const donationForm = this.donationProgramForm.getRawValue();
    const donationProgram: DonationProgram = {
      DonationProgramName: donationForm.donationProgramName,
      Description: donationForm.description,
      ImageRef: this.image,
      StartDate: this.dateFormat.dateToString(this.fromDate),
      EndDate: this.dateFormat.dateToString(this.toDate)
    };
    if (this.donationProgram){
      const donationProgram: DonationProgram = {
        DonationProgramId: this.donationProgram.DonationProgramId,
        DonationProgramName: donationForm.donationProgramName,
        Description: donationForm.description,
        ImageRef: this.image,
        StartDate: this.dateFormat.dateToString(this.fromDate),
        EndDate: this.dateFormat.dateToString(this.toDate)
      };
      this.resourcesService.updateDonationProgram(donationProgram).subscribe(
          (res) => {
            this.donationProgramChange.emit(res);
            this.modalService.dismissAll();
            this.toastrService.success('The donation program has been updated');
          },
          (err) => { this.toastrService.error(err.error.message); }
        );
    } else {
      this.resourcesService.addDonationProgram(donationProgram).subscribe(
        (res) => {
          this.donationProgramChange.emit(res);
          this.modalService.dismissAll();
          this.toastrService.success('A donation program has been added');
        },
        (err) => { this.toastrService.error(err.error.message); }
      );
    }
  }
  get f() { return this.donationProgramForm.controls; }ƒ
  getImage(image) {
    this.image = image;
  }
  closeModal() {
    this.modalService.dismissAll();
  }
  //Date Range Selection
  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
    } else if (this.fromDate && !this.toDate && date && date.after(this.fromDate)) {
      this.toDate = date;
    } else {
      this.toDate = null;
      this.fromDate = date;
    }
  }
  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }
  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }
  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  }
  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const parsed = this.formatter.parse(input);
    return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
  }
}


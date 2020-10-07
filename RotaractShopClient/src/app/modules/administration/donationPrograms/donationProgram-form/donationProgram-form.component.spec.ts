import {async, ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';
import {RouterTestingModule} from '@angular/router/testing';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormBuilder, ReactiveFormsModule} from '@angular/forms';
import {ResourcesService} from '../../../../services/resources.service';
import {of} from 'rxjs';
import {
  NgbCalendar,
  NgbDateParserFormatter,
  NgbDatepickerModule,
  NgbModal
} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {DateFormat} from '../../../../helpers/dateFormat';
import {DonationProgramFormComponent} from './donationProgram-form.component';
import {SharedComponentsModule} from '../../../shared/shared.module';

// Mock all the used services
const modalServiceSpy = jasmine.createSpyObj('NgbModal', ['dismissAll']);
const resourcesServiceSpy = jasmine.createSpyObj('ResourcesService',
  ['updateDonationProgram', 'addDonationProgram']);
const toastrServicespy = jasmine.createSpyObj('ToastrService', ['error', 'success']);
const calendarSpy = jasmine.createSpyObj('NgbCalendar', ['getToday', 'getNext']);
const dateFormatSpy = jasmine.createSpyObj('DateFormat', ['dateFromString', 'dateToString']);
const formatterSpy = jasmine.createSpyObj('NgbDateParserFormatter', ['format', 'parse']);

function advance(f: ComponentFixture<any>) {
  tick();
  f.detectChanges();
}

describe('Donation Program Form Component- Edit Integrated Test', () => {
  let fixture: ComponentFixture<DonationProgramFormComponent>;
// mock the donationProgram supplied by DonationOverview parent Component
  const donationProgram = { DonationProgramId: 14,
    DonationProgramName: 'Donation 3',
    Description: 'This is a banner that has the purpose of a substitute for a donation.',
    ImageRef: 'data:image/png;base64,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',
    StartDate: '2019-07-10T00:00:00',
    EndDate: '2021-08-22T00:00:00',
    Total: 242372};
  const updateDonationProgramSpy = resourcesServiceSpy.updateDonationProgram.and.returnValue(of('Successful Updated'));
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        SharedComponentsModule,
        NgbDatepickerModule
      ],
      providers: [
        {provide: ResourcesService, useValue: resourcesServiceSpy},
        FormBuilder,
        {provide: NgbModal, useValue: modalServiceSpy},
        {provide: ToastrService, useValue: toastrServicespy},
        {provide: DateFormat, useValue: dateFormatSpy},
        {provide: NgbCalendar, useValue: calendarSpy},
        {provide: NgbDateParserFormatter, useValue: formatterSpy}
      ],
      declarations: [DonationProgramFormComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(DonationProgramFormComponent);

    // supply the Input directive as the parent would do it
    fixture.componentInstance.donationProgram = donationProgram;
    fixture.detectChanges();
  }));

  it('should have been supplied with a donationProgram from the parent Component', fakeAsync(() => {
    expect(fixture.componentInstance.donationProgram).toEqual(donationProgram);
    expect(fixture.componentInstance.donationProgramForm.controls.donationProgramName.value)
      .toEqual(donationProgram.DonationProgramName);
    expect(fixture.componentInstance.donationProgramForm.controls.description.value)
      .toEqual(donationProgram.Description);
    expect(fixture.componentInstance.image).toEqual(donationProgram.ImageRef);
    expect(dateFormatSpy.dateFromString).toHaveBeenCalled();
  }));

  it('resourceService updateDonationProgram() should be called when save', fakeAsync(() => {
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue('TestDonationProgramName');
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();

    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    fixture.detectChanges();

    expect(resourcesServiceSpy.updateDonationProgram).toHaveBeenCalled();
  }));

  it('resourceService updateDonationProgram() should not be called', fakeAsync(() => {
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue(null);
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();

    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    advance(fixture);
    expect(fixture.componentInstance.submitted).toBeFalse();
  }));

  it('donationProgramChange emit() should be called when updateDonation is successfull', fakeAsync(() => {
    spyOn(fixture.componentInstance.donationProgramChange, 'emit');
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue('NewTestName');
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();
    advance(fixture);
    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(fixture.componentInstance.donationProgramChange.emit).toHaveBeenCalled();
  }));

  it('modalService dismissAll() should be called when updateDonation is successfull', fakeAsync(() => {
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue('NewTestName');
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();
    advance(fixture);
    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(modalServiceSpy.dismissAll).toHaveBeenCalled();
  }));

  it('modalService dismissAll() should be called when cancel', fakeAsync(() => {
    const button = fixture.debugElement.nativeElement.querySelector('#cancel');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(modalServiceSpy.dismissAll).toHaveBeenCalled();
  }));
});
describe('Donation Program Form Component- Add Integrated Test', () => {
  let fixture: ComponentFixture<DonationProgramFormComponent>;
  function advance(f: ComponentFixture<any>) {
    tick();
    f.detectChanges();
  }
  const addDonationProgram = resourcesServiceSpy.addDonationProgram.and.returnValue(of('Successful Added'));
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        SharedComponentsModule,
        NgbDatepickerModule
      ],
      providers: [
        {provide: ResourcesService, useValue: resourcesServiceSpy},
        FormBuilder,
        {provide: NgbModal, useValue: modalServiceSpy},
        {provide: ToastrService, useValue: toastrServicespy},
        {provide: DateFormat, useValue: dateFormatSpy},
        {provide: NgbCalendar, useValue: calendarSpy},
        {provide: NgbDateParserFormatter, useValue: formatterSpy}
      ],
      declarations: [DonationProgramFormComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(DonationProgramFormComponent);

    // supply donation program input with null as the parent would do in this case
    fixture.componentInstance.donationProgram = null;
    fixture.detectChanges();
  }));

  it('should not have been supplied with a donationProgram from the parent Component', fakeAsync(() => {
    expect(fixture.componentInstance.donationProgram).toEqual(null);
  }));

  it('resourceService addDonationProgram() should be called when save', fakeAsync(() => {
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue('TestDonationProgramName');
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();

    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    fixture.detectChanges();

    expect(resourcesServiceSpy.addDonationProgram).toHaveBeenCalled();
  }));

  it('resourceService addDonationProgram() should not be called if from is invalid', fakeAsync(() => {
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue(null);
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();

    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    advance(fixture);
    expect(fixture.componentInstance.submitted).toBeFalse();
  }));

  it('donationProgramChange emit() should be called when addDonationProgram is successfull', fakeAsync(() => {
    spyOn(fixture.componentInstance.donationProgramChange, 'emit');
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue('NewTestName');
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();
    advance(fixture);
    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(fixture.componentInstance.donationProgramChange.emit).toHaveBeenCalled();
  }));

  it('modalService dismissAll() should be called when addDonationProgram is successfull', fakeAsync(() => {
    // Update form
    fixture.componentInstance.donationProgramForm.controls.donationProgramName.setValue('NewTestName');
    fixture.componentInstance.donationProgramForm.controls.description.setValue('TestDescription');
    fixture.componentInstance.image = 'New Image';
    fixture.detectChanges();
    advance(fixture);
    const button = fixture.debugElement.nativeElement.querySelector('#save');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(modalServiceSpy.dismissAll).toHaveBeenCalled();
  }));

  it('modalService dismissAll() should be called when cancel', fakeAsync(() => {
    const button = fixture.debugElement.nativeElement.querySelector('#cancel');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(modalServiceSpy.dismissAll).toHaveBeenCalled();
  }));
});

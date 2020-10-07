// Mock all the used services
import {async, ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';
import {OrdersComponent} from './orders.component';
import {RouterTestingModule} from '@angular/router/testing';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormBuilder, ReactiveFormsModule} from '@angular/forms';
import {ResourcesService} from '../../../../services/resources.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {of} from 'rxjs';
import {HttpHeaders} from '@angular/common/http';
import {SharedComponentsModule} from '../../../shared/shared.module';

const modalServiceSpy = jasmine.createSpyObj('NgbModal', ['dismissAll', 'open']);
const resourcesServiceSpy = jasmine.createSpyObj('ResourcesService',
  ['changeOrderStatus', 'getOrders']);
const toastrServiceSpy = jasmine.createSpyObj('ToastrService',
  ['error', 'success', 'warning']);


const header = new HttpHeaders({
  'x-pagination' : JSON.stringify({totalPages: 9})
});
const dataPending = {body: [
    {
      OrderId: 71,
      OrderItems: [
        {
          product: {
            ProductId: 98,
            Name: 'Rotary pin for people',
            Description: 'This is a Rotary pin for festive occasions or events of the Rotary clubs around the world.',
            ImageRef: null,
            Price: 3,
            Inventory: 366
          },
          Quantity: 300
        },
        {
          product: {
            ProductId: 104,
            Name: 'his',
            Description: 'bsdfbndfn',
            ImageRef: null,
            Price: 53245,
            Inventory: 399
          },
          Quantity: 30000
        }
      ],
      ShippingDetailsModel: {
        Country: 'Denmark',
        City: 'bsbrbe',
        AddressLine: 'brberbes',
        CountyOrRegion: 'bresr',
        PostalCode: 543523,
        FirstName: 'vdsvs',
        LastName: '4325434',
        PhoneNumber: 4325434
      },
      Status: 'PENDING',
      DateStamp: '2020-05-19',
      TotalPrice: 1597350900
    }
  ], headers: header};
const dataConfirmed = {body: [
    {
      OrderId: 24,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 1
          },
          Quantity: 1
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 8700,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 81929286
      },
      Status: 'CONFIRMED',
      DateStamp: '2020-05-08',
      TotalPrice: 19
    },
    {
      OrderId: 27,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 19
          },
          Quantity: 5
        }
      ],
      ShippingDetailsModel: {
        Country: 'Denmark',
        City: 'Horsens',
        AddressLine: 'Kollegievaenget',
        CountyOrRegion: 'Jutland',
        PostalCode: 8700,
        FirstName: 'Viktoria',
        LastName: 'Kouni',
        PhoneNumber: 87987654
      },
      Status: 'CONFIRMED',
      DateStamp: '2020-05-08',
      TotalPrice: 64
    },
    {
      OrderId: 29,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 13
          },
          Quantity: 4
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 0,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 0
      },
      Status: 'CONFIRMED',
      DateStamp: '2020-05-08',
      TotalPrice: 74
    },
    {
      OrderId: 30,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 13
          },
          Quantity: 1
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 0,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 0
      },
      Status: 'CONFIRMED',
      DateStamp: '2020-05-09',
      TotalPrice: 19
    },
    {
      OrderId: 32,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 10
          },
          Quantity: 1
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 0,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 0
      },
      Status: 'CONFIRMED',
      DateStamp: '2020-05-09',
      TotalPrice: 19
    }
  ], headers: header};
const dataProgress = {body: [
    {
      OrderId: 23,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 6
          },
          Quantity: 10
        }
      ],
      ShippingDetailsModel: {
        Country: 'Denmark',
        City: 'Horsens',
        AddressLine: 'kolegievaenget',
        CountyOrRegion: 'Jutland',
        PostalCode: 8700,
        FirstName: 'Elena',
        LastName: 'Smeu',
        PhoneNumber: 89658526
      },
      Status: 'INPROGRESS',
      DateStamp: '2020-05-07',
      TotalPrice: 400
    },
    {
      OrderId: 25,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 0
          },
          Quantity: 1
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 8700,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 81929286
      },
      Status: 'INPROGRESS',
      DateStamp: '2020-05-08',
      TotalPrice: 19
    }
  ], headers: header};
const dataShipping = {body: [
    {
      OrderId: 22,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 8
          },
          Quantity: 12
        }
      ],
      ShippingDetailsModel: {
        Country: 'Denmark',
        City: 'Horsens',
        AddressLine: 'kolegievaenget',
        CountyOrRegion: 'Jutland',
        PostalCode: 8700,
        FirstName: 'Elena',
        LastName: 'Smeu',
        PhoneNumber: 89658526
      },
      Status: 'SHIPPING',
      DateStamp: '2020-05-07',
      TotalPrice: 400
    },
    {
      OrderId: 26,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 24
          },
          Quantity: 1
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 8700,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 81929286
      },
      Status: 'SHIPPING',
      DateStamp: '2020-05-08',
      TotalPrice: 19
    }
  ], headers: header};
const dataCompleted = {body: [
    {
      OrderId: 18,
      OrderItems: [
        {
          product: {
            ProductId: 88,
            Name: 'T-shirt',
            Description: 'T-shirts for Programmers',
            ImageRef: null,
            Price: 11,
            Inventory: 145
          },
          Quantity: 4
        }
      ],
      ShippingDetailsModel: {
        Country: 'Greece',
        City: 'Chios',
        AddressLine: 'Kallamos',
        CountyOrRegion: 'kallimasia',
        PostalCode: 82100,
        FirstName: 'Adamantios',
        LastName: 'Alexandros',
        PhoneNumber: 81929286
      },
      Status: 'COMPLETED',
      DateStamp: '2020-05-06',
      TotalPrice: 44
    },
    {
      OrderId: 21,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 24
          },
          Quantity: 26
        }
      ],
      ShippingDetailsModel: {
        Country: 'Denmark',
        City: 'Horsens',
        AddressLine: 'Kollegie',
        CountyOrRegion: 'Jutland',
        PostalCode: 8700,
        FirstName: 'Adamantios',
        LastName: 'Kounis',
        PhoneNumber: 81929286
      },
      Status: 'COMPLETED',
      DateStamp: '2020-05-07',
      TotalPrice: 500
    },
    {
      OrderId: 31,
      OrderItems: [
        {
          product: {
            ProductId: 87,
            Name: 'Mug',
            Description: 'A Mug for Programmers',
            ImageRef: null,
            Price: 19,
            Inventory: 11
          },
          Quantity: 1
        }
      ],
      ShippingDetailsModel: {
        Country: 'string',
        City: 'string',
        AddressLine: 'string',
        CountyOrRegion: 'string',
        PostalCode: 0,
        FirstName: 'string',
        LastName: 'string',
        PhoneNumber: 0
      },
      Status: 'COMPLETED',
      DateStamp: '2020-05-09',
      TotalPrice: 19
    }
  ], headers: header};


function advance(f: ComponentFixture<any>) {
  tick();
  f.detectChanges();
}

describe('Orders Component Integrated Test', () => {
  let fixture: ComponentFixture<OrdersComponent>;
  const getOrders = resourcesServiceSpy.getOrders.and.returnValue(of(dataConfirmed));
  const changeStatusSpy = resourcesServiceSpy.changeOrderStatus.and.returnValue(of(true));

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        SharedComponentsModule
      ],
      providers: [
        {provide: ResourcesService, useValue: resourcesServiceSpy},
        FormBuilder,
        {provide: NgbModal, useValue: modalServiceSpy},
        {provide: ToastrService, useValue: toastrServiceSpy}
      ],
      declarations: [OrdersComponent]
    }).compileComponents();
    fixture = TestBed.createComponent(OrdersComponent);

    /*
     Simulate status Input() directive binding from parent component
     */
    fixture.detectChanges();
  }));
  afterEach(() => {
    fixture.destroy();
  });

  it('ResourceService getOrders() should be called', fakeAsync( () => {
    expect(resourcesServiceSpy.getOrders).toHaveBeenCalled();
  }));
  it('ResourceService changeStatusOrder() should be called when cancel order', fakeAsync( () => {
      fixture.detectChanges();
      const button = fixture.debugElement.nativeElement.querySelector('#cancel24');
      button.click();
      advance(fixture);
      expect(resourcesServiceSpy.changeOrderStatus).toHaveBeenCalled();
    }));
  it('ResourceService changeStatusOrder() should be called on Confirmed/Progress/Shipping', fakeAsync( () => {
    fixture.detectChanges();
    const button = fixture.debugElement.nativeElement.querySelector('#changeStatus27');
    button.click();
    advance(fixture);
    expect(resourcesServiceSpy.changeOrderStatus).toHaveBeenCalled();
  }));
  it('ResourceService changeStatusOrder() should not be called on Completed', fakeAsync( () => {
    fixture.componentInstance.orders = dataCompleted.body;
    fixture.detectChanges();
    advance(fixture);
    const button = fixture.debugElement.nativeElement.querySelector('#changeStatus18');
    button.click();
    advance(fixture);
    expect(toastrServiceSpy.warning).toHaveBeenCalled();
    }));
  it('ModalService open() should be called when addInventory on Pending Orders', fakeAsync( () => {
      fixture.componentInstance.orders = dataPending.body;
      fixture.detectChanges();
      advance(fixture);
      const button = fixture.debugElement.nativeElement.querySelector('#addInventory71-104');
      button.click();
      advance(fixture);
      expect(resourcesServiceSpy.changeOrderStatus).toHaveBeenCalled();
    }));
}
);

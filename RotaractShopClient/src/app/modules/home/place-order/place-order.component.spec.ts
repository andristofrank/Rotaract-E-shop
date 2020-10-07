import {async, ComponentFixture, fakeAsync, TestBed, tick} from '@angular/core/testing';
import {PlaceOrderComponent} from './place-order.component';
import {RouterTestingModule} from '@angular/router/testing';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormBuilder, ReactiveFormsModule} from '@angular/forms';
import {Router} from '@angular/router';
import {ResourcesService} from '../../../services/resources.service';
import {CartService} from '../../../services/cart.service';
import {of} from 'rxjs';

const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
const resourcesServiceSpy = jasmine.createSpyObj('ResourcesService', ['placeOrder']);
const cartServiceSpy = jasmine.createSpyObj('CartService', ['cart', 'getTotal', 'emptyCart']);
const validShippingAddress = {
  Country: 'Denmark',
  City: 'Horsens',
  AddressLine: 'Addressline 3',
  CountyOrRegion: 'MidIutland',
  PostalCode: 8700,
  PhoneNumber: 423545,
  FirstName: 'TestSFirstName',
  LastName: 'TestSFirstName'
};
const cartItems = {
  products: [
    {
      item: {
        Name: 'Rotary pin for people',
        Description: 'This is a Rotary pin for festive occasions or events of the Rotary clubs around the world.',
        ImageRef: 'Image',
        Price: 3,
        ProductId: 98,
        Inventory: 366
      },
      quantity: 3
    },
    {
      item: {
        Name: 'TV',
        Description: 'This is a TV',
        ImageRef: 'Image',
        Price: 289,
        ProductId: 101,
        Inventory: 198
      },
      quantity: 20
    },
    {
      item: {
        Name: 'his',
        Description: 'bsdfbndfn',
        ImageRef: 'Image',
        Price: 53245,
        ProductId: 104,
        Inventory: 399
      },
      quantity: 1
    }
  ]
};
describe('Place Order Component Integrated Test', () => {
  let fixture: ComponentFixture<PlaceOrderComponent>;

  function updateForm(Country, City, AddressLine, CountyOrRegion, PostalCode, PhoneNumber, FirstName, LastName ) {
    fixture.componentInstance.shippingDetailsForm.controls.country.setValue(Country);
    fixture.componentInstance.shippingDetailsForm.controls.city.setValue(City);
    fixture.componentInstance.shippingDetailsForm.controls.addressLine.setValue(AddressLine);
    fixture.componentInstance.shippingDetailsForm.controls.county_region.setValue(CountyOrRegion);
    fixture.componentInstance.shippingDetailsForm.controls.postalCode.setValue(PostalCode);
    fixture.componentInstance.shippingDetailsForm.controls.phoneNumber.setValue(PhoneNumber);
    fixture.componentInstance.shippingDetailsForm.controls.firstName.setValue(FirstName);
    fixture.componentInstance.shippingDetailsForm.controls.lastName.setValue(LastName);
  }
  function advance(f: ComponentFixture<any>) {
    tick();
    f.detectChanges();
  }
  let placeOrderSpy;
  let getCartSpy;
  let getTotalCartSpy;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        ReactiveFormsModule
      ],
      providers: [
        {provide: ResourcesService, useValue: resourcesServiceSpy},
        FormBuilder,
        {provide: Router, useValue: routerSpy },
        {provide: CartService, useValue: cartServiceSpy}
      ],
      declarations: [ PlaceOrderComponent ]
    }).compileComponents();

    fixture = TestBed.createComponent(PlaceOrderComponent);

    getCartSpy = cartServiceSpy.cart.and.returnValue(cartItems);
    getTotalCartSpy = cartServiceSpy.getTotal.and.returnValue(30);
    placeOrderSpy = resourcesServiceSpy.placeOrder.and.returnValue(of('Test Response'));
  }));

  it('ResourceService placeOrder() should be called', fakeAsync( () => {
    updateForm(validShippingAddress.Country,
      validShippingAddress.City,
      validShippingAddress.AddressLine,
      validShippingAddress.CountyOrRegion,
      validShippingAddress.PostalCode,
      validShippingAddress.PhoneNumber,
      validShippingAddress.FirstName,
      validShippingAddress.LastName);

    fixture.detectChanges();
    const button = fixture.debugElement.nativeElement.querySelector('#pay');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(resourcesServiceSpy.placeOrder).toHaveBeenCalled();
  }));

  it('ResourceService placeOrder() should not be called if form invalid', fakeAsync( () => {
    updateForm(validShippingAddress.Country,
      validShippingAddress.City,
      validShippingAddress.AddressLine,
      validShippingAddress.CountyOrRegion,
      validShippingAddress.PostalCode,
      validShippingAddress.PhoneNumber,
      validShippingAddress.FirstName,
      null);

    fixture.detectChanges();
    const button = fixture.debugElement.nativeElement.querySelector('#pay');
    button.click();
    fixture.detectChanges();
    expect(fixture.componentInstance.submitted).toEqual(false);
  }));

  it('cartService emptyCart() should be called when order is placed', fakeAsync( () => {
    updateForm(validShippingAddress.Country,
      validShippingAddress.City,
      validShippingAddress.AddressLine,
      validShippingAddress.CountyOrRegion,
      validShippingAddress.PostalCode,
      validShippingAddress.PhoneNumber,
      validShippingAddress.FirstName,
      validShippingAddress.LastName);

    fixture.detectChanges();
    const button = fixture.debugElement.nativeElement.querySelector('#pay');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(cartServiceSpy.emptyCart).toHaveBeenCalled();
  }));

  it('should route to home/profile/orders when order is placed', fakeAsync( () => {
    updateForm(validShippingAddress.Country,
      validShippingAddress.City,
      validShippingAddress.AddressLine,
      validShippingAddress.CountyOrRegion,
      validShippingAddress.PostalCode,
      validShippingAddress.PhoneNumber,
      validShippingAddress.FirstName,
      validShippingAddress.LastName);

    fixture.detectChanges();
    const button = fixture.debugElement.nativeElement.querySelector('#pay');
    button.click();
    fixture.detectChanges();
    advance(fixture);
    expect(routerSpy.navigate).toHaveBeenCalled();
    const navArgs = routerSpy.navigate.calls.mostRecent().args[0];
    expect(navArgs).toEqual(['home/profile/orders'], 'should nav to profile orders page');
  }));

  it('should route to home/catalogue if canceled', fakeAsync( () => {
    const button = fixture.debugElement.nativeElement.querySelector('#cancel');
    button.click();
    fixture.detectChanges();

    expect(routerSpy.navigate).toHaveBeenCalled();
    const navArgs = routerSpy.navigate.calls.mostRecent().args[0];
    expect(navArgs).toEqual(['home/catalogue'], 'should nav to home catalogue');
  }));
});

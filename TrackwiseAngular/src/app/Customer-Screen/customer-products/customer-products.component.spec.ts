import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerProductComponent } from './customer-products.component';

describe('CustomerLandingPageComponent', () => {
  let component: CustomerProductComponent;
  let fixture: ComponentFixture<CustomerProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CustomerProductComponent]
    });
    fixture = TestBed.createComponent(CustomerProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTrucksComponent } from './add-trucks.component';

describe('AddTrucksComponent', () => {
  let component: AddTrucksComponent;
  let fixture: ComponentFixture<AddTrucksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddTrucksComponent]
    });
    fixture = TestBed.createComponent(AddTrucksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

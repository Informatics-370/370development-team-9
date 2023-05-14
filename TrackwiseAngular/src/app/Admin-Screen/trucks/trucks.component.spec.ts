import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrucksComponent } from './trucks.component';

describe('TrucksComponent', () => {
  let component: TrucksComponent;
  let fixture: ComponentFixture<TrucksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TrucksComponent]
    });
    fixture = TestBed.createComponent(TrucksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTrailersComponent } from './add-trailers.component';

describe('AddTrailersComponent', () => {
  let component: AddTrailersComponent;
  let fixture: ComponentFixture<AddTrailersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddTrailersComponent]
    });
    fixture = TestBed.createComponent(AddTrailersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

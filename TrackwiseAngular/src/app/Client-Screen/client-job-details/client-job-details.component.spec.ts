import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientJobDetailsComponent } from './client-job-details.component';

describe('ClientJobDetailsComponent', () => {
  let component: ClientJobDetailsComponent;
  let fixture: ComponentFixture<ClientJobDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClientJobDetailsComponent]
    });
    fixture = TestBed.createComponent(ClientJobDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

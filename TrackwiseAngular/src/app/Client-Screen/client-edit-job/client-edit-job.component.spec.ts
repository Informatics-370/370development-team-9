import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientEditJobComponent } from './client-edit-job.component';

describe('ClientEditJobComponent', () => {
  let component: ClientEditJobComponent;
  let fixture: ComponentFixture<ClientEditJobComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClientEditJobComponent]
    });
    fixture = TestBed.createComponent(ClientEditJobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

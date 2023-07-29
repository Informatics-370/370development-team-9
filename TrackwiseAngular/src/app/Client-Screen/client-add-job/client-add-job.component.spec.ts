import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAddJobComponent } from './client-add-job.component';

describe('ClientAddJobComponent', () => {
  let component: ClientAddJobComponent;
  let fixture: ComponentFixture<ClientAddJobComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ClientAddJobComponent]
    });
    fixture = TestBed.createComponent(ClientAddJobComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

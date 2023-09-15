import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CancelNotificationComponent } from './cancel-notification.component';

describe('CancelNotificationComponent', () => {
  let component: CancelNotificationComponent;
  let fixture: ComponentFixture<CancelNotificationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CancelNotificationComponent]
    });
    fixture = TestBed.createComponent(CancelNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

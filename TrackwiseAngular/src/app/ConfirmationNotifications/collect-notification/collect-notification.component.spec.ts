import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectNotificationComponent } from './collect-notification.component';

describe('CollectNotificationComponent', () => {
  let component: CollectNotificationComponent;
  let fixture: ComponentFixture<CollectNotificationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CollectNotificationComponent]
    });
    fixture = TestBed.createComponent(CollectNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

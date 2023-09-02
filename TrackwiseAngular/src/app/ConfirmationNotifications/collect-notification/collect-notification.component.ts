import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-collect-notification',
  templateUrl: './collect-notification.component.html',
  styleUrls: ['./collect-notification.component.scss']
})
export class CollectNotificationComponent {
  constructor(
    public dialogRef: MatDialogRef<CollectNotificationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onYesClick(): void {
    this.dialogRef.close(true);
  }
}

import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-remove-notification',
  templateUrl: './remove-notification.component.html',
  styleUrls: ['./remove-notification.component.scss']
})
export class RemoveNotificationComponent {
  constructor(
    public dialogRef: MatDialogRef<RemoveNotificationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onYesClick(): void {
    this.dialogRef.close(true);
  }
}

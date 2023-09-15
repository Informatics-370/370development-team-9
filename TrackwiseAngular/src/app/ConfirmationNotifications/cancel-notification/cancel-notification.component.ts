import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-cancel-notification',
  templateUrl: './cancel-notification.component.html',
  styleUrls: ['./cancel-notification.component.scss']
})
export class CancelNotificationComponent {
  constructor(
    public dialogRef: MatDialogRef<CancelNotificationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onYesClick(): void {
    this.dialogRef.close(true);
  }

}

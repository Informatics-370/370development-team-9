import { Component } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  userName: string = 'John Doe';
  userEmail: string = 'john.doe@example.com';
  showEditForm: boolean = false;

  cardholderName: string = "";
  cardNumber: string = "";
  expirationDate: string = "";
  cvv: string = "";
  showPaymentInfoForm:boolean = false;

  toggleEditForm() {
    this.showEditForm = !this.showEditForm;
  }

  togglePaymentForm(){
    this.showPaymentInfoForm = !this.showPaymentInfoForm;
  }

  saveChanges() {
    // Implement your logic to save the changes
    // For example, you could send the data to a backend API
    // and update the user's information accordingly.
    console.log('Saving changes...');
    console.log('New Name:', this.userName);
    console.log('New Email:', this.userEmail);

    // Once the changes are saved, hide the form again.
    this.showEditForm = false;
  }
}

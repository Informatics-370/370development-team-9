import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Admin } from 'src/app/shared/admin';

@Component({
  selector: 'app-admin-profile',
  templateUrl: './admin-profile.component.html',
  styleUrls: ['./admin-profile.component.scss']
})
export class AdminProfileComponent {
  
    constructor(private dataService: DataService, private router:Router) {}
  
    ngOnInit(): void {
      this.dataService.revertToLogin();
      this.GetAdminProfile();
    }
  
    admin : Admin =
    {
      admin_ID: "",
      name: '',
      lastname: "",
      email: "",
      password:""
    }
  
    editAdmin: Admin = { ...this.admin };
    
    userName: string = 'John Doe';
    userEmail: string = 'john.doe@example.com';
    showEditForm: boolean = false;
  
    cardholderName: string = "";
    cardNumber: string = "";
    expirationDate: string = "";
    cvv: string = "";
    showPaymentInfoForm:boolean = false;
  
    GetAdminProfile(){
      this.dataService.GetAdminProfile().subscribe((result) => {
        this.admin = result;
        this.editAdmin = { ...this.admin };
        console.log(result)
      })
    }
  
    EditAdmin() {
      console.log(this.admin)
      this.dataService.EditAdmin(this.admin.admin_ID ,this.admin).subscribe({
        next: (response) => {
          this.router.navigate(['/Admin-Screen/admin-home']);
        }
      });
    }
  
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
  
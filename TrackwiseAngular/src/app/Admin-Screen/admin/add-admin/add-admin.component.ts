import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Admin } from 'src/app/shared/admin';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-admin',
  templateUrl: './add-admin.component.html',
  styleUrls: ['./add-admin.component.scss']
})
export class AddAdminComponent {

  adminDetails: Admin =
  {
    admin_ID:"",
    name:"",
    lastname:"",
    email:"",
    password:"",
   
  };
  showHelpModal: boolean = false;

  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddAdmin()
  {
    this.dataService.AddAdmin(this.adminDetails).subscribe({
      next: (admin) => {this.router.navigate(['/Admin-Screen/admins']);
      this.snackBar.open(this.adminDetails.name + ` successfully registered`, 'X', {duration: 3000});}
    })
  }

  OpenHelpModal() {
    this.showHelpModal = true;
    document.body.style.overflow = 'hidden';
  }

  CloseHelpModal() {
    this.showHelpModal = false;
    document.body.style.overflow = 'auto';
  }
}

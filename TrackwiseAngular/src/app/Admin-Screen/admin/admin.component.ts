import { Component , OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RemoveNotificationComponent } from 'src/app/ConfirmationNotifications/remove-notification/remove-notification.component';
import { DataService} from 'src/app/services/data.service';
import { Admin } from 'src/app/shared/admin';
import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  searchText: string = ''; // Property to store the search text
  admins: any[] = []; // Property to store the admin data
  originalAdmins: any[] = []; // Property to store the original admin data
  
  constructor(private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar) { }
  
  ngOnInit(): void {
    this.GetAdmins();
    this.dataService.revertToLogin();
  }

  GetAdmins() {
    this.dataService.GetAdmins().subscribe(result => {
      let adminList: any[] = result;
      this.originalAdmins = [...adminList]; // Store a copy of the original admin data
      adminList.forEach((element) => {
        this.admins.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original admin data
      this.admins = [...this.originalAdmins];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the admins based on the search text
      const filteredAdmins = this.originalAdmins.filter(admin => {
        const fullName = admin.name.toLowerCase() + ' ' + admin.lastname.toLowerCase();
        const email = admin.email.toLowerCase();
        const password = admin.password.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          admin.lastname.toLowerCase().includes(searchTextLower) ||
          email.includes(searchTextLower) || password.includes(searchTextLower)
          
        );
      });

      // Update the admins array with the filtered results
      this.admins = filteredAdmins;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  openConfirmationDialog(admin_ID : string): void {
    const dialogRef = this.dialog.open(RemoveNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { admin_ID }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteAdmin(admin_ID);
        this.snackBar.open(` Admin Successfully Removed`, 'X', {duration: 3000});
      }
    });
  }

  DeleteAdmin(admin_ID:string)
  {
    this.dataService.DeleteAdmin(admin_ID).subscribe({
      next: (response) => location.reload()
    })
  }
}

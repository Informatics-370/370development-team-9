import { Component , OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService} from 'src/app/services/data.service';
import { Client } from 'src/app/shared/client';
import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { RemoveNotificationComponent } from 'src/app/ConfirmationNotifications/remove-notification/remove-notification.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})

export class ClientsComponent implements OnInit {
  searchText: string = ''; // Property to store the search text
  clients: any[] = []; // Property to store the client data
  originalClients: any[] = []; // Property to store the original client data
  
  constructor(private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar) { }
  
  ngOnInit(): void {
    this.GetClients();
    this.dataService.revertToLogin();
  }

  GetClients() {
    this.dataService.GetClients().subscribe(result => {
      let clientList: any[] = result;
      this.originalClients = [...clientList]; // Store a copy of the original client data
      clientList.forEach((element) => {
        this.clients.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original client data
      this.clients = [...this.originalClients];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the clients based on the search text
      const filteredClients = this.originalClients.filter(client => {
        const fullName = client.name.toLowerCase();
        const phoneNumber = client.phoneNumber.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          phoneNumber.includes(searchTextLower)
         
        );
      });

      // Update the clients array with the filtered results
      this.clients = filteredClients;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  openConfirmationDialog(client_ID: string): void {
    const dialogRef = this.dialog.open(RemoveNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { client_ID }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteClient(client_ID);
        this.snackBar.open(` Client Successfully Removed`, 'X', {duration: 3000});
      }
    });
  }

  DeleteClient(client_ID:string)
  {
    this.dataService.DeleteClient(client_ID).subscribe({
      next: (response) => location.reload()
    })
  }

}

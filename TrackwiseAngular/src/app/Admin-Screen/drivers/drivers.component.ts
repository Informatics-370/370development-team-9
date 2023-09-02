import { Component , OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService} from 'src/app/services/data.service';
import { Driver } from 'src/app/shared/driver';
import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { RemoveNotificationComponent } from 'src/app/ConfirmationNotifications/remove-notification/remove-notification.component';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.scss']
})
export class DriversComponent implements OnInit {
  searchText: string = ''; // Property to store the search text
  drivers: any[] = []; // Property to store the driver data
  originalDrivers: any[] = []; // Property to store the original driver data
  
  constructor(private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar) { }
  
  ngOnInit(): void {
    this.GetDrivers();
    this.dataService.revertToLogin();
  }

  GetDrivers() {
    this.dataService.GetDrivers().subscribe(result => {
      let driverList: any[] = result;
      this.originalDrivers = [...driverList]; // Store a copy of the original driver data
      driverList.forEach((element) => {
        this.drivers.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original driver data
      this.drivers = [...this.originalDrivers];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the drivers based on the search text
      const filteredDrivers = this.originalDrivers.filter(driver => {
        const fullName = driver.name.toLowerCase() + ' ' + driver.lastname.toLowerCase();
        const status = driver.driverStatus.status.toLowerCase();
        const phoneNumber = driver.phoneNumber.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          driver.lastname.toLowerCase().includes(searchTextLower) ||
          phoneNumber.includes(searchTextLower) ||
          (searchTextLower === 'available' && status === 'available') ||
          (searchTextLower === 'unavailable' && status === 'unavailable') ||
          (searchTextLower === 'busy' && status === 'busy')
        );
      });

      // Update the drivers array with the filtered results
      this.drivers = filteredDrivers;
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
        this.DeleteDriver(client_ID);
        this.snackBar.open(` Driver Successfully Removed`, 'X', {duration: 3000});
      }
    });
  }

  DeleteDriver(driver_ID:string)
  {
    this.dataService.DeleteDriver(driver_ID).subscribe({
      next: (response) => location.reload()
    })
  }

}

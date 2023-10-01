import { Component } from '@angular/core';
import { RemoveNotificationComponent } from 'src/app/ConfirmationNotifications/remove-notification/remove-notification.component';
import { DataService } from 'src/app/services/data.service';
import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-suppliers',
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.scss']
})
export class SuppliersComponent {

  searchText: string = ''; // Property to store the search text
  suppliers: any[] = []; // Property to store the driver data
  originalSuppliers: any[] = []; // Property to store the original driver data
  showHelpModal: boolean = false;

  
  constructor(private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar) { }
  
  ngOnInit(): void {
    this.GetSuppliers();
    this.dataService.revertToLogin();
  }

  GetSuppliers() {
    this.dataService.GetSuppliers().subscribe(result => {
      let supplierList: any[] = result;
      this.originalSuppliers = [...supplierList]; // Store a copy of the original driver data
      supplierList.forEach((element) => {
        this.suppliers.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original driver data
      this.suppliers = [...this.originalSuppliers];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the drivers based on the search text
      const filteredsuppliers = this.originalSuppliers.filter(supplier => {
        const name = supplier.name.toLowerCase();
        const email = supplier.email.toLowerCase();
        const contactNumber = supplier.contact_Number.toLowerCase();
        return (
          name.includes(searchTextLower) ||
          email.includes(searchTextLower) ||
          contactNumber.includes(searchTextLower)
        );
      });

      // Update the drivers array with the filtered results
      this.suppliers = filteredsuppliers;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  openConfirmationDialog(supplier_ID: string): void {
    const dialogRef = this.dialog.open(RemoveNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { supplier_ID }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteSupplier(supplier_ID);
        this.snackBar.open(` Supplier Successfully Removed`, 'X', {duration: 3000});
      }
    });
  }

  DeleteSupplier(supplier_ID:string)
  {
    this.dataService.DeleteSupplier(supplier_ID).subscribe({
      next: (response) => location.reload()
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

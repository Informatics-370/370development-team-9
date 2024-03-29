import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RemoveNotificationComponent } from 'src/app/ConfirmationNotifications/remove-notification/remove-notification.component';
import { DataService } from 'src/app/services/data.service';
import { VAT } from 'src/app/shared/VAT';
import { Product } from 'src/app/shared/product';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent {


    products: Product[] = [];
    VAT: VAT = {
      vaT_Amount: 0
    };
    searchText: string = ''; // Property to store the search text
    originalProducts: Product[] = []; // Property to store the original trailer data
    showHelpModal: boolean = false;
  
  
    constructor( private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar,private http: HttpClient) { }
  
    ngOnInit(): void {
      this.GetProducts();
      this.GetVAT();
      this.dataService.revertToLogin();
    }
  
  
    GetProducts()
    {
      this.dataService.GetProducts().subscribe(result => {
        let productList:any[] = result
        this.originalProducts = [...productList]; // Store a copy of the original trailer data
        productList.forEach((element) => {
          this.products.push(element)
          console.log(element);
        });
      })
    }

    GetVAT()
    {
      this.dataService.GetVAT().subscribe({
        next: (response) => {
          this.VAT = response;
          console.log(this.VAT)
        }
      })
    }
  
    search() {
      if (this.searchText.trim() === '') {
        // If search text is empty, revert back to original product data
        this.products = [...this.originalProducts];
      } else {
        const searchTextLower = this.searchText.toLowerCase();
  
        // Filter the trailers based on the search text
        const filteredProducts = this.originalProducts.filter(product => {
          const name = product.product_Name.toLowerCase();
          const description = product.product_Description.toLowerCase();
          const price = product.product_Price;
          const category = product.product_Category.name.toLowerCase();
          const type = product.product_Type.name.toLowerCase();
          
          return (
            name.includes(searchTextLower)||
            price.toString().includes(searchTextLower)||
            description.includes(searchTextLower) || 
            name.includes(searchTextLower) ||
            category.includes(searchTextLower)||
            type.includes(searchTextLower)
          );
        });
  
        // Update the drivers array with the filtered results
        this.products = filteredProducts;
      }
    }
  
    handleKeyUp(event: KeyboardEvent) {
      if (event.key === 'Enter') {
        this.search();
      }
    }

    openConfirmationDialog(ProductID: string): void {
      const dialogRef = this.dialog.open(RemoveNotificationComponent, {
        width: '300px', // Adjust the width as needed
        data: { ProductID }
      });
    
      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.DeleteProduct(ProductID);
          this.snackBar.open(` Product Successfully Removed`, 'X', {duration: 3000});
        }
      });
    }
  
    DeleteProduct(ProductID:string)
    {
      this.dataService.DeleteProduct(ProductID).subscribe({
        next: (response) => location.reload()
      })
    }

    UnlistProduct(ProductID:string)
    {
      this.dataService.UnlistPorduct(ProductID).subscribe({
        next: (response) => {
          this.products = [];
          this.GetProducts();
        }
      })
    }

    RelistProduct(ProductID:string)
    {
      this.dataService.RelistPorduct(ProductID).subscribe({
        next: (response) => {
          this.products = [];
          this.GetProducts();
        }
      })
    }

    selectedFile: any | undefined;
    onFileSelected(event: any) {
      this.selectedFile = event.target.files[0];
    }
  
    async uploadFile() {
      if (this.selectedFile) {
        this.dataService.uploadCsv(this.selectedFile)
          .subscribe(
            (response) => {
              this.products = [];
              this.GetProducts();
              console.log('Response:', response);
              // Handle success (e.g., show a success message)
              this.snackBar.open(` Product Successfully Added`, 'X', {duration: 3000});
            },
            (error) => {
              this.products = [];
              this.GetProducts();
              this.snackBar.open(` Product Successfully Added`, 'X', {duration: 3000});
            }
          );
      }
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

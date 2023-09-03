import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss']
})
export class EditProductComponent {

  productDetails: Product =
  {
    product_ID:"",
    product_Name:"",
    product_Description:"",
    product_Price:0,
    quantity: 0,

    product_Type:{
      product_Type_ID:"",
      name:"",
      description:""
    },

    product_Category:{
      product_Category_ID:"",
      name:"",
      description:""
    },
  
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {

    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetProduct(params['product_ID']).subscribe({
            next: (response) => {
              this.productDetails = response;
            }
          })

      }
    })

    this.dataService.revertToLogin();
  }

  EditProduct()
  {    
    this.dataService.EditProduct(this.productDetails.product_ID, this.productDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/products']);
      this.snackBar.open(`Product successfully edited`, 'X', {duration: 3000});
    }
    })
  }

  onImageSelected(event: any): void {
    if (event.target.files && event.target.files.length > 0) {
      const file: File = event.target.files[0];
      const reader = new FileReader();
  
      reader.onload = (e: any) => {
        this.productDetails.image = e.target.result; // Assign the base64 string to the image property
      };
  
      reader.readAsDataURL(file);
    }
  }

  removeNegativeSign() {
    if (this.productDetails.product_Price < 0) {
      this.productDetails.product_Price = 0;
    }

    if (this.productDetails.quantity < 0) {
      this.productDetails.quantity = 0;
    }
  }

}

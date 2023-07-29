import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent {
  productTypes: any[] = []; 
  productCategories: any[] = []; 
  
  // AddProductRequest: {
  //   product_Name: string; 
  //   product_Description:string;
  //   product_Price: number;
  //   quantity: number; 
  //   product_Category_ID:string;
  //   product_Type_ID:string;
  // }[] = [];

  AddProductRequest: Product =
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




  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
    this.GetProductCategories();
    this.GetProductTypes();
  }

  AddProduct()
  {
    console.log(this.AddProductRequest);
    this.dataService.AddProduct(this.AddProductRequest).subscribe({
      next: (product) => {this.router.navigate(['/Admin-Screen/products'])}
    })
  }

  GetProductTypes() {
    this.dataService.GetProductTypes().subscribe(result => {
      this.productTypes = result; // Assign the retrieved product types directly to the "productTypes" array
      console.log(this.productTypes);
    });
  }
    
    GetProductCategories() {
      this.dataService.GetProductCategories().subscribe(result => {
        this.productCategories = result; // Assign the retrieved product types directly to the "productTypes" array
        console.log(this.productCategories);
      });
    }

}

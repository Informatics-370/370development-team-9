import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';
import { ProductCategoriesFilter } from 'src/app/shared/productCategoriesFilter';
import { ProductCategory } from 'src/app/shared/productCategory';
import { ProductType } from 'src/app/shared/productType';
import { ProductTypeFilter } from 'src/app/shared/productTypesFilter';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent {
  
  productTypes: ProductType[] = []; // This holds the list of available product types

  productCategories: ProductCategory[] = []; // This holds the list of available product types


  selectedImage: File | null = null;
  
  ProductTypeFilter : ProductTypeFilter[] = [];

  ProductCategoryFilter : ProductCategoriesFilter[] = [];
 

  AddProductRequest: Product =
  {
    product_ID:"",
    product_Name:"",
    product_Description:"",
    product_Price:0,
    quantity: 0,
    image:"",

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



  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
    this.GetProductCategories();
    this.GetProductTypes();
  }

  AddProduct()
  {
    console.log(this.AddProductRequest);
    this.dataService.AddProduct(this.AddProductRequest).subscribe({
      next: (product) => {this.router.navigate(['/Admin-Screen/products']);
      this.snackBar.open(this.AddProductRequest.product_Name + ` successfully added`, 'X', {duration: 3000});}
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

    // GetFilteredTypes(typeID: string){
    //   this.dataService.GetSpesificProductType(typeID).subscribe({
    //     next: (response) => {
    //       this.ProductTypeFilter = response;
    //     }
    //   })
    // }

    // GetFilteredCategories(categoryID: string){
    //   this.dataService.GetSpesificProductCategory(categoryID).subscribe({
    //     next: (response) => {
    //       this.ProductCategoryFilter = response;
    //     }
    //   })
    // }

    onImageSelected(event: any): void {
      if (event.target.files && event.target.files.length > 0) {
        const file: File = event.target.files[0];
        const reader = new FileReader();
    
        reader.onload = (e: any) => {
          this.AddProductRequest.image = e.target.result; // Assign the base64 string to the image property
        };
    
        reader.readAsDataURL(file);
      }
    }

    onTypeChange(event: any) {
      const selectedTypeId = event.target.value;
      console.log(selectedTypeId);
    
      this.GetFilteredTypes(selectedTypeId);
    }
    
    async GetFilteredTypes(typeID: string) {
      try {
        const response = await this.dataService.GetSpesificProductType(typeID).toPromise();
        if (response) {
          this.ProductTypeFilter = response;
          console.log(this.ProductTypeFilter);
    
          // Clear productCategories
          this.productCategories = [];
    
          // Add values from ProductTypeFilter to productCategories
          this.ProductTypeFilter.forEach(productfilter => {
            this.productCategories.push({
              name: productfilter.product_Category.name,
              description: productfilter.product_Category.description,
              product_Category_ID: productfilter.product_Category.product_Category_ID
            });
          });
    
          console.log(this.productCategories);
        } else {
          console.error("GetFilteredTypes did not return data.");
        }
      } catch (error) {
        console.error("Error fetching filtered types:", error);
      }
    }

    removeNegativeSign() {
      if (this.AddProductRequest.product_Price < 0) {
        this.AddProductRequest.product_Price = 0;
      }

      if (this.AddProductRequest.quantity < 0) {
        this.AddProductRequest.quantity = 0;
      }
    }

}

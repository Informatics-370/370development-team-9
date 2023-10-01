import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';
import { ProductCategoriesFilter } from 'src/app/shared/productCategoriesFilter';
import { ProductCategory } from 'src/app/shared/productCategory';
import { ProductType } from 'src/app/shared/productType';
import { ProductTypeFilter } from 'src/app/shared/productTypesFilter';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss']
})
export class EditProductComponent {

  productTypes: ProductType[] = []; // This holds the list of available product types

  productCategories: ProductCategory[] = []; // This holds the list of available product types

  ProductTypeFilter : ProductTypeFilter[] = [];

  ProductCategoryFilter : ProductCategoriesFilter[] = [];

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
  showHelpModal: boolean = false;

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

    this.GetProductCategories();
    this.GetProductTypes();

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

  onCategoryChange(event: any) {
    const selectedTypeId = event.target.value;
    console.log(selectedTypeId);
  
    this.GetFilteredCategories(selectedTypeId);
  }
  
  async GetFilteredCategories(categoryID: string) {
    try {
      const response = await this.dataService.GetSpesificProductCategory(categoryID).toPromise();
      if (response) {
        this.ProductCategoryFilter = response;
        console.log(this.ProductCategoryFilter);
  
        // Clear productCategories
        this.productTypes = [];
  
        // Add values from ProductTypeFilter to productCategories
        this.ProductCategoryFilter.forEach(productfilter => {
          this.productTypes.push({
            name: productfilter.product_Type.name,
            description: productfilter.product_Type.description,
            product_Type_ID: productfilter.product_Type.product_Type_ID
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
    if (this.productDetails.product_Price < 0) {
      this.productDetails.product_Price = 0;
    }

    if (this.productDetails.quantity < 0) {
      this.productDetails.quantity = 0;
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

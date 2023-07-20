import { Component } from '@angular/core';
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
    
    product_Category_ID:"",
    productCategory:{
      product_Category_ID:"",
      name:"",
      description:""
    },
    product_Type_ID:"",
    productType:{
      product_Type_ID:"",
      name:"",
      description:""
    }
  
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

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
  }

  EditProduct()
  {    
    this.dataService.EditProduct(this.productDetails.product_ID, this.productDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/products'])}
    })
  }

}

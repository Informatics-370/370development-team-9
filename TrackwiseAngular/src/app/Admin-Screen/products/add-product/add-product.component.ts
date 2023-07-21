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

  AddProductRequest: Product =
  {
    product_ID:"",
    product_Name:"",
    product_Description:"",
    product_Price:0,
    quantity: 0,

    productType:{
      product_Type_ID:"",
      name:"",
      description:""
    },

    productCategory:{
      product_Category_ID:"",
      name:"",
      description:""
    },

  };




  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  AddProduct()
  {
    this.dataService.AddProduct(this.AddProductRequest).subscribe({
      next: (trailer) => {this.router.navigate(['/Admin-Screen/products'])}
    })
  }



}

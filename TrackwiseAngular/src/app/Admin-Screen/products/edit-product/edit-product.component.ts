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
    product_ID:0,
    product_name:"",
    description:"",
    price:0,
    
    product_category_ID:0,
    productCategory:{
      product_category_ID:0,
      name:"",
      description:""
    }
  
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {

    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetProduct(params['productID']).subscribe({
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
    console.log('yes')
  }

}

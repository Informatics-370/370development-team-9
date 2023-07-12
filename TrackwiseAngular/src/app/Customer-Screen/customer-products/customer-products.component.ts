import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource } from '@angular/material/table';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';


@Component({
  selector: 'app-customer-products',
  templateUrl: './customer-products.component.html',
  styleUrls: ['./customer-products.component.scss'],
  
})
export class CustomerProductComponent {


  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.GetProducts(); 
  }

 products : Product[] = [];

GetAllProducts() {
  this.products=JSON.parse(localStorage.getItem('product')!);  
  }

  GetProducts()
  {
    this.dataService.GetProducts().subscribe(result => {
      let productList:any[] = result
//    this.originalProducts = [...productList]; 
      productList.forEach((element) => {
        this.products.push(element)
        console.log(element);
      });
    })
  }

  AddItemToCart(e: any) {
    console.log(e);
    let AddCartItem: any[] = JSON.parse(sessionStorage.getItem("cartItem") || '[]');
  
    if (AddCartItem.length === 0) {
      if (!e.Quantity)
      {
        e.Quantity = 1;
      }
      AddCartItem.push(e);
    } else {
      let res = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == e.product_ID);
  
      if (res === undefined) {
        if (!e.Quantity)
        {
          e.Quantity = 1;
        }
        AddCartItem.push(e);
      } else {
        res.Quantity++;
      }
    }
  
    sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
  }
  


 }

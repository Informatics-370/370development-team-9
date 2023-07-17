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
  products : Product[] = [];
  // cardFlipped=false;

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.GetProducts(); 
  }

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

  AddItemToCart(e:any){
    console.log(e)
    let AddCartItem:any = JSON.parse(sessionStorage.getItem("cartItem")|| '[]');
    AddCartItem.push(e)
    sessionStorage.setItem('cartItem',JSON.stringify(AddCartItem));
  }

  // flipCard(product: Product): void {
  //   product.this.cardFlipped = !product.cardFlipped;
  // }

 }

 

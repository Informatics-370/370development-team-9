import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource } from '@angular/material/table';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';


@Component({
  selector: 'app-customer-landing-page',
  templateUrl: './customer-landing-page.component.html',
  styleUrls: ['./customer-landing-page.component.scss'],
  
})
export class CustomerLandingPageComponent implements OnInit{

  productList: any[] = [];

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.GetProductList();
  }

  GetProductList() {
    this.dataService.GetProducts().subscribe(
      (result: any) => {
        let productList: any[] = result;
        productList.forEach((product) => {
          this.productList.push(product);
          console.log(product);
        });
      },
      (error: any) => {
        console.error('Error retrieving product list:', error);
      }
    );
  }

  addToCart(products: any) {
    console.log(products);
    let cartItems:any=[];
    cartItems.push(products);
    localStorage.setItem('cart',JSON.stringify(cartItems));
  }


 }

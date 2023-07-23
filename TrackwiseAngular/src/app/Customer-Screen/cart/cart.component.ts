import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

interface CartItem {
  image: string;
  name: string;
  quantity: number;
  price: number;
}

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})

export class CartComponent implements OnInit{

  products: any=[];
  cartItems: any=[];

  constructor(private router: Router, private dataService : DataService) {}

  ngOnInit(): void {
    this.CartItems();
    this.dataService.revertToLogin();
  }
  
  calculateTotal(): number {
    let total = 0;
    for (const item of this.cartItems) {
      total += item.quantity * item.price;
    }
    return total;
  }

  continueShopping(): void {
    this.router.navigate(['/Customer-Screen/customer-products']);
  }

  CartItems(){
    if(sessionStorage.getItem('cartItem')){
      this.cartItems = JSON.parse(sessionStorage.getItem('cartItem')!)
    }
  }

    increaseQuantity(item: any) {
    let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!)
    let res = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == item.product_ID);
    res.cartQuantity++; // Increment quantity by 1
    item.cartQuantity++;
    sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem)); 
  }

  decreaseQuantity(item: any) {
    if (item.cartQuantity && item.cartQuantity > 1) {
      let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!)
      let res = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == item.product_ID);
      res.cartQuantity--;
      item.cartQuantity--; 
      sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
    }
  }

  removeFromCart(index: number) {
    let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!);
  
    if (index !== -1) {
      this.cartItems.splice(index, 1);
      AddCartItem.splice(index, 1); // Remove the item from the cart
      sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
    }
  }
}
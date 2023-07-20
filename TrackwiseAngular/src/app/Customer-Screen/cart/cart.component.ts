import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

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

  constructor(private router: Router) {}

  ngOnInit(): void {
    this.CartItems();
  }
  

  // decreaseQuantity(index: number): void {
  //   if (this.cartItems[index].quantity > 1) {
  //     this.cartItems[index].quantity--;
  //   }
  // }

  // increaseQuantity(index: number): void {
  //   this.cartItems[index].quantity++;
  // }

  // removeItem(cartItem: any): void {
  //   let res = cartItem.find((element: { product_ID: any; }) => element.product_ID == cartItem.product_ID);
  // }

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
    res.Quantity++; // Increment quantity by 1
    item.Quantity++;
    sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem)); 
  }

  decreaseQuantity(item: any) {
    if (item.Quantity && item.Quantity > 1) {
      let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!)
      let res = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == item.product_ID);
      res.Quantity--;
      item.Quantity--; 
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
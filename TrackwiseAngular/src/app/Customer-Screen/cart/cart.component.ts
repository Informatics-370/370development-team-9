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
    
  }

  decreaseQuantity(index: number): void {
    if (this.cartItems[index].quantity > 1) {
      this.cartItems[index].quantity--;
    }
  }

  increaseQuantity(index: number): void {
    this.cartItems[index].quantity++;
  }

  removeItem(index: number): void {
    this.cartItems.splice(index, 1);
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

    // increaseQuantity(product: Product) {
  //   if (!product.Quantity) {
  //     product.Quantity = 1; // Set initial quantity to 1
  //   } else {
  //     product.Quantity++; // Increment quantity by 1
  //   }
  // }

  // decreaseQuantity(product: Product) {
  //   if (product.Quantity && product.Quantity > 1) {
  //     product.Quantity--; // Decrement quantity by 1 if it's greater than 1
  //   }
  // }
}

import { Component } from '@angular/core';
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

export class CartComponent {
  cartItems: CartItem[] = [
    { image: 'product1.jpg', name: 'Product 1', quantity: 2, price: 10 },
    { image: 'product2.jpg', name: 'Product 2', quantity: 1, price: 20 },
    { image: 'product3.jpg', name: 'Product 3', quantity: 3, price: 15 }
  ];

  constructor(private router: Router) {}

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
    this.router.navigate(['/Customer-Screen/customer-landing-page']);
  }
}

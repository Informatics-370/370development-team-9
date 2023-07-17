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
  cartItems: any[] = [
    {
      image: 'assets/Mail.jpg',
      name: 'Product 1',
      description: 'product description text',
      quantity: 2,
      price: 10.99
    },
    {
      image:  'assets/Mail.jpg',
      name: 'Product 2',
      description: 'product description text',
      quantity: 1,
      price: 5.99
    },
    {
      image:  'assets/Mail.jpg',
      name: 'Product 3',
      description: 'product description text',
      quantity: 3,
      price: 7.99
    }
    // Add more items as needed
  ];

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
}

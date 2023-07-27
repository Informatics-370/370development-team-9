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

export class CartComponent implements OnInit {

  products: any = [];
  cartItems: any = [];
  total: number = 0;
  orderLines: { productId: string; quantity: number; }[] = [];

  constructor(private router: Router, public dataService: DataService) { }

  ngOnInit(): void {
    this.CartItems();
    this.dataService.revertToLogin();
  }

  calculateTotal(): number {
    this.total = 0;
    if (!sessionStorage.getItem('cartItem')) {
      this.total = 0;
    } else {
      this.cartItems = JSON.parse(sessionStorage.getItem('cartItem')!)
      for (const item of this.cartItems) {
        this.total += item.cartQuantity * item.product_Price;
      }

    }

    return this.total;
  }

  continueShopping(): void {
    this.router.navigate(['/Customer-Screen/customer-products']);
  }

  CartItems() {
    if (sessionStorage.getItem('cartItem')) {
      this.cartItems = JSON.parse(sessionStorage.getItem('cartItem')!);
      this.calculateTotal();
      this.dataService.calculateQuantity();

      // Populate the orderLines array with product details
      this.orderLines = this.cartItems.map((item: { product_ID: any; cartQuantity: any; }) => ({
        productId: item.product_ID,
        quantity: item.cartQuantity
      }));
    }
  }

  increaseQuantity(item: any) {
    if (item.cartQuantity < item.quantity) {
      let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!)
      let res = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == item.product_ID);
      res.cartQuantity++; // Increment quantity by 1
      item.cartQuantity++;
      sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
      this.calculateTotal();
      this.dataService.calculateQuantity();
    }
  }

  decreaseQuantity(item: any) {
    if (item.cartQuantity > 1) {
      let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!)
      let res = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == item.product_ID);
      res.cartQuantity--;
      item.cartQuantity--;
      sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
      this.calculateTotal();
      this.dataService.calculateQuantity();
    }
  }

  removeFromCart(index: number) {
    let AddCartItem = JSON.parse(sessionStorage.getItem('cartItem')!);

    if (index !== -1) {
      this.cartItems.splice(index, 1);
      AddCartItem.splice(index, 1); // Remove the item from the cart
      sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
      this.calculateTotal();
      this.dataService.calculateQuantity();
    }
  }
  

  checkout(): void {
    // Perform any validation or additional logic before calling the service method

    // Call the CreateOrder method in your service and pass the orderLines
    this.dataService.CreateOrder({ orderLines: this.orderLines }).subscribe(
      () => {
        // Handle success or display success message
        sessionStorage.removeItem('cartItem');
        this.dataService.calculateQuantity();
        this.router.navigate(['/Customer-Screen/customer-products']);
      },
      (error) => {
        // Handle error or display error message
        console.error('Error creating order:', error);
      }
    );
  }
}
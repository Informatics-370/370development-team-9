import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Product, ProductCategories, ProductTypes } from 'src/app/shared/product';


@Component({
  selector: 'app-customer-products',
  templateUrl: './customer-products.component.html',
  styleUrls: ['./customer-products.component.scss'],
  
})
export class CustomerProductComponent {
  productTypes: any[] = []; 
  productCategories: any[] = []; 

  GetProductType: ProductTypes =
  {
    product_Type_ID:"",
    name:"",
    description:""
  };

  GetProductCategory: ProductCategories =
  {
    product_Category_ID:"",
    name:"",
    description:""
  };

  // cardFlipped=false;
  showModal: boolean = false;
  selectedProduct: Product | null = null;

  constructor(private dataService: DataService, private router : Router) {}

  ngOnInit(): void {
    this.GetProductCategories();
    this.GetProductTypes();
    this.GetProducts();
    this.dataService.calculateQuantity();
  }

 products : Product[] = [];

GetAllProducts() {
  this.products=JSON.parse(localStorage.getItem('product')!);
  }

  initializeVisibleQuantities() {
    let AddCartItem = JSON.parse(sessionStorage.getItem("cartItem") || '[]');

    this.products.forEach((product) => {
      let CartItem = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == product.product_ID);
      if(CartItem){
        product.quantity = CartItem.quantity - CartItem.cartQuantity;
      }

    });
  }

  GetProducts() {
    this.dataService.GetProducts().subscribe((result) => {
      let productList: any[] = result;
      
      // Add your code to process cart items and update product quantities
      let AddCartItem = JSON.parse(sessionStorage.getItem("cartItem") || '[]');
      productList.forEach((product) => {
        let CartItem = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == product.product_ID);
        if (CartItem) {
          product.quantity = CartItem.quantity - CartItem.cartQuantity;
        }
      });
      
      // Push the products into the products array after processing
      productList.forEach((element) => {
        this.products.push(element);
        console.log(element);
      });
    });
  }
  

  AddItemToCart(event: Event, product: any) {
    
    if(this.dataService.isLoggedIn == false){
      this.router.navigateByUrl('Authentication/login');
      return;
    }

    console.log(product);
    let AddCartItem: any[] = JSON.parse(sessionStorage.getItem("cartItem") || '[]');
    let CartItem = AddCartItem.find((element: { product_ID: any; }) => element.product_ID == product.product_ID);


    if (AddCartItem.length === 0) {
      if (!product.cartQuantity)
      {
        product.cartQuantity = 1;
      }
      AddCartItem.push(product);    
      sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));

      let UpdatedCartItem: any[] = JSON.parse(sessionStorage.getItem("cartItem") || '[]');
      let CartItem = UpdatedCartItem.find((element: { product_ID: any; }) => element.product_ID == product.product_ID);

      if (this.isOutOfStock(product)) {
        console.log("This product is out of stock.");
        return; // Exit the method without adding to the cart
      }

      product.quantity = CartItem.quantity - CartItem.cartQuantity;
    } else {
    
      if (CartItem === undefined) {
        if (!product.cartQuantity)
        {
          product.cartQuantity = 1;
        }
        AddCartItem.push(product);
        sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));

        let UpdatedCartItem: any[] = JSON.parse(sessionStorage.getItem("cartItem") || '[]');
        let CartItem = UpdatedCartItem.find((element: { product_ID: any; }) => element.product_ID == product.product_ID);
        product.quantity = CartItem.quantity - CartItem.cartQuantity;
      } else {
        CartItem.cartQuantity++;
        product.quantity = CartItem.quantity - CartItem.cartQuantity;
        sessionStorage.setItem('cartItem', JSON.stringify(AddCartItem));
      }
    }
    this.dataService.calculateQuantity();
    event.stopPropagation();
  }

  isOutOfStock(product: any): boolean {
    return product.quantity === 0;
  }

  GetProductTypes() {
    this.dataService.GetProductTypes().subscribe(result => {
      this.productTypes = result; // Assign the retrieved product types directly to the "productTypes" array
      console.log(this.productTypes);
    });
  }
    
    GetProductCategories() {
      this.dataService.GetProductCategories().subscribe(result => {
        this.productCategories = result; // Assign the retrieved product types directly to the "productTypes" array
        console.log(this.productCategories);
      });
    }


  OpenModal(product: Product) {
    this.selectedProduct = product; // Store the selected product
    this.showModal = true;
  }

  CloseModal() {
    this.selectedProduct = null; // Clear the selected product when closing the modal
    this.showModal = false;
  }


  // flipCard(product: Product): void {
  //   product.this.cardFlipped = !product.cardFlipped;
  // }

 }

 

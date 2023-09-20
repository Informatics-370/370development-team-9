import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { VAT } from 'src/app/shared/VAT';
import { Product, ProductCategories, ProductTypes } from 'src/app/shared/product';
import { ProductCategoriesFilter } from 'src/app/shared/productCategoriesFilter';
import { ProductTypeFilter } from 'src/app/shared/productTypesFilter';

@Component({
  selector: 'app-customer-products',
  templateUrl: './customer-products.component.html',
  styleUrls: ['./customer-products.component.scss'],
})
export class CustomerProductComponent {
  productTypes: any[] = [];
  productCategories: any[] = [];
  searchText: string = ''; // Property to store the search text
  originalProducts: Product[] = []; // Property to store the original trailer data
  VAT: VAT = {
    vaT_Amount: 0
  };

  ProductTypeFilter : ProductTypeFilter[] = [];

  ProductCategoryFilter : ProductCategoriesFilter[] = [];

  selectedSortOption: string = 'NameAZ'; // Default sorting option
  selectedCategory: string = ''; // Default category filter
  selectedType: string = ''; // Default type filter

  showModal: boolean = false;
  selectedProduct: Product | null = null;

  constructor(private dataService: DataService, private router: Router) {}

  ngOnInit(): void {
    this.GetProductCategories();
    this.GetProductTypes();
    this.GetProducts();
    this.GetVAT();
    this.dataService.calculateQuantity();
    this.filterProducts(); // Call the filter function initially
  }

  products: Product[] = [];

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
      this.originalProducts = [...productList];

      // Push the products into the products array after processing
      productList.forEach((element) => {
        this.products.push(element);
      });
    });
  }

  GetVAT()
  {
    this.dataService.GetVAT().subscribe({
      next: (response) => {
        this.VAT = response;
        console.log(this.VAT)
      }
    })
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
    this.dataService.GetProductTypes().subscribe((result) => {
      this.productTypes = result;
    });
  }

  GetProductCategories() {
    this.dataService.GetProductCategories().subscribe((result) => {
      this.productCategories = result;
    });
  }

  OpenModal(product: Product) {
    this.selectedProduct = product;
    this.showModal = true;
  }

  CloseModal() {
    this.selectedProduct = null;
    this.showModal = false;
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original product data
      this.products = [...this.originalProducts];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the products based on the search text
      const filteredProducts = this.originalProducts.filter((product) => {
        const name = product.product_Name.toLowerCase();
        const description = product.product_Description.toLowerCase();
        const price = product.product_Price;
        const category = product.product_Category.name.toLowerCase();
        const type = product.product_Type.name.toLowerCase();

        return (
          name.includes(searchTextLower) ||
          price.toString().includes(searchTextLower) ||
          description.includes(searchTextLower) ||
          category.includes(searchTextLower) ||
          type.includes(searchTextLower)
        );
      });

      this.products = filteredProducts;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }


  filterProducts() {
    let filteredProducts = [...this.originalProducts];

    // Apply sorting
    if (this.selectedSortOption === 'NameAZ') {
      filteredProducts.sort((a, b) => a.product_Name.localeCompare(b.product_Name));
    } else if (this.selectedSortOption === 'NameZA') {
      filteredProducts.sort((a, b) => b.product_Name.localeCompare(a.product_Name));
    } else if (this.selectedSortOption === 'LowHigh') {
      filteredProducts.sort((a, b) => a.product_Price - b.product_Price);
    } else if (this.selectedSortOption === 'HighLow') {
      filteredProducts.sort((a, b) => b.product_Price - a.product_Price);
    }

    // Apply category filter
    if (this.selectedCategory) {
      filteredProducts = filteredProducts.filter((product) => product.product_Category.product_Category_ID === this.selectedCategory);
    }

    // Apply type filter
    if (this.selectedType) {
      filteredProducts = filteredProducts.filter((product) => product.product_Type.product_Type_ID === this.selectedType);
    }

    this.products = filteredProducts;
  }

  resetFilters() {
    this.selectedSortOption = '';
    this.selectedCategory = '';
    this.selectedType = '';
    this.filterProducts();
  }
  
  // Add the following functions to respond to filter/sort changes
  onSortOptionChange() {
    this.filterProducts();
  }

  onCategoryChange() {
    this.filterProducts();
  }

  // onTypeChange() {
  //   this.filterProducts();
  // }

  onTypeChange(event: any) {
    const selectedTypeId = event.target.value;
    console.log(selectedTypeId);
  
    this.GetFilteredTypes(selectedTypeId);
  }
  
  async GetFilteredTypes(typeID: string) {
    try {
      const response = await this.dataService.GetSpesificProductType(typeID).toPromise();
      if (response) {
        this.ProductTypeFilter = response;
        console.log(this.ProductTypeFilter);
  
        // Clear productCategories
        this.productCategories = [];
  
        // Add values from ProductTypeFilter to productCategories
        this.ProductTypeFilter.forEach(productfilter => {
          this.productCategories.push({
            name: productfilter.product_Category.name,
            description: productfilter.product_Category.description,
            product_Category_ID: productfilter.product_Category.product_Category_ID
          });
        });
        this.filterProducts();
        console.log(this.productCategories);
      } else {
        console.error("GetFilteredTypes did not return data.");
      }
    } catch (error) {
      console.error("Error fetching filtered types:", error);
    }
  }

  // onCategoryChange(event: any) {
  //   const selectedTypeId = event.target.value;
  //   console.log(selectedTypeId);
  
  //   this.GetFilteredCategories(selectedTypeId);
  // }
  
  // async GetFilteredCategories(categoryID: string) {
  //   try {
  //     const response = await this.dataService.GetSpesificProductCategory(categoryID).toPromise();
  //     if (response) {
  //       this.ProductCategoryFilter = response;
  //       console.log(this.ProductCategoryFilter);
  
  //       // Clear productCategories
  //       this.productTypes = [];
  
  //       // Add values from ProductTypeFilter to productCategories
  //       this.ProductCategoryFilter.forEach(productfilter => {
  //         this.productTypes.push({
  //           name: productfilter.product_Type.name,
  //           description: productfilter.product_Type.description,
  //           product_Type_ID: productfilter.product_Type.product_Type_ID
  //         });
  //       });
  //       this.filterProducts();
  //       console.log(this.productCategories);
  //     } else {
  //       console.error("GetFilteredTypes did not return data.");
  //     }
  //   } catch (error) {
  //     console.error("Error fetching filtered types:", error);
  //   }
  // }
  
}

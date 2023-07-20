import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent {


    products: Product[] = [];
    searchText: string = ''; // Property to store the search text
    originalProducts: Product[] = []; // Property to store the original trailer data
  
    constructor( private dataService: DataService) { }
  
    ngOnInit(): void {
      this.GetProducts();
    }
  
  
    GetProducts()
    {
      this.dataService.GetProducts().subscribe(result => {
        let productList:any[] = result
        this.originalProducts = [...productList]; // Store a copy of the original trailer data
        productList.forEach((element) => {
          this.products.push(element)
          console.log(element);
        });
      })
    }
  
    search() {
      if (this.searchText.trim() === '') {
        // If search text is empty, revert back to original product data
        this.products = [...this.originalProducts];
      } else {
        const searchTextLower = this.searchText.toLowerCase();
  
        // Filter the trailers based on the search text
        const filteredProducts = this.originalProducts.filter(product => {
          const name = product.product_Name.toLowerCase();
          const description = product.product_Description.toLowerCase();
          const price = product.product_Price;
          const category = product.productCategory.name.toLowerCase();
          
          return (
            name.includes(searchTextLower)||
            price.toString().includes(searchTextLower)||
            description.includes(searchTextLower) || 
            name.includes(searchTextLower) ||
            category.includes(searchTextLower)
       
          );
        });
  
        // Update the drivers array with the filtered results
        this.products = filteredProducts;
      }
    }
  
    handleKeyUp(event: KeyboardEvent) {
      if (event.key === 'Enter') {
        this.search();
      }
    }
  
    DeleteProduct(ProductID:string)
    {
      this.dataService.DeleteProduct(ProductID).subscribe({
        next: (response) => location.reload()
      })
    }


}

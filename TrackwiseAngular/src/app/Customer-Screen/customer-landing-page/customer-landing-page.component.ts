import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTableDataSource } from '@angular/material/table';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';


@Component({
  selector: 'app-customer-landing-page',
  templateUrl: './customer-landing-page.component.html',
  styleUrls: ['./customer-landing-page.component.scss'],
  
})
export class CustomerLandingPageComponent {


  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.GetAllProducts(); 
  }

 products : Product[] = [];

GetAllProducts() {
  this.products=JSON.parse(localStorage.getItem('product')!);  
  }


 }

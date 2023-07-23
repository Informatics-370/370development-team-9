import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';


@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.scss']
})
export class CustomerOrdersComponent {

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.dataService.revertToLogin();
    this.GetCustomerOrders();
  }


  showModal: boolean = false;
  customerOrders: any[] = [];

  GetCustomerOrders() {
    this.dataService.GetCustomerOrders().subscribe(result => {
      let custOrderslist: any[] = result;
      custOrderslist.forEach((element) => {
        this.customerOrders.push(element);
        console.log(element);
      });
    });
  }

  OpenModal() {
    this.showModal = true;
  }

  CloseModal() {
    this.showModal = false;
  }

}

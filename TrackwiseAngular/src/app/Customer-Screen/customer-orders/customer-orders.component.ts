import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DataService } from 'src/app/services/data.service';
import { Order } from 'src/app/shared/order';


@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.scss']
})
export class CustomerOrdersComponent {

  constructor(private dataService: DataService, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.dataService.revertToLogin();
    this.GetCustomerOrders();
  }

  noOrdersCancelledOrCollected: boolean = true;
  noOrdersOrdered: boolean = true;
  showModal: boolean = false;

  customerOrders: any[] = [];

  orders : Order[] = [];

  GetCustomerOrders() {
    this.dataService.GetCustomerOrders().subscribe(result => {
      let custOrderslist: any[] = result;
      console.log(result);
      custOrderslist.forEach((element) => {
        this.customerOrders.push(element);
        console.log(element);
      });
  
      // Set the noOrdersCancelledOrCollected variable based on the orders' status
      this.noOrdersCancelledOrCollected = !this.customerOrders.some((order) => order.status === 'Collected' || order.status === 'Cancelled');
      this.noOrdersOrdered = !this.customerOrders.some((order) => order.status === 'Ordered');
    });
  }

  GetOrder(order_ID:string){
    this.orders = [];
    this.dataService.GetOrder(order_ID).subscribe((result) => {
      this.orders.push(result);
      console.log(result);
    })
  }

  async CancelOrder(order: any) {
    try {
      await this.dataService.CancelOrder(order.order_ID).toPromise();
      const updatedOrder = await this.dataService.GetOrder(order.order_ID).toPromise();
      this.snackBar.open(`Order cancelled`, 'X', {duration: 3000});
      // Check if updatedOrder is not undefined before accessing its properties
      if (updatedOrder?.status) {
        const index = this.customerOrders.findIndex((o) => o.order_ID === order.order_ID);
        if (index !== -1) {
          this.customerOrders[index].status = updatedOrder.status;
        }
      }
    } catch (error) {
      console.log('Error cancelling order:', error);
    }
  }

  
  
  
  OpenModal(order:any) {
    this.GetOrder(order.order_ID)
    console.log(order.order_ID)
    this.showModal = true;
  }

  CloseModal() {
    this.showModal = false;
  }

}

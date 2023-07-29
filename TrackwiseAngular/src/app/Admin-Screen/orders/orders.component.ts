  import { Component } from '@angular/core';
  import { DataService } from 'src/app/services/data.service';
  import { Order } from 'src/app/shared/order';

  @Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
  })
  export class OrdersComponent {


constructor(private dataService: DataService) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
    this.GetAllOrders();
  }

  noOrdersCancelledOrCollected: boolean = true;
  noOrdersOrdered: boolean = true;
  showModal: boolean = false;

  customerOrders: any[] = [];
  allCustomerOrders: any[] = [];

  orders: Order[] = [];

  GetCustomerOrders() {
    this.dataService.GetCustomerOrders().subscribe(result => {
      let custOrderslist: any[] = result;
      custOrderslist.forEach((element) => {
        this.customerOrders.push(element);
        console.log(element);
      });

      // Set the noOrdersCancelledOrCollected variable based on the orders' status
      this.noOrdersCancelledOrCollected = !this.customerOrders.some((order) => order.status === 'Collected' || order.status === 'Cancelled');
      this.noOrdersOrdered = !this.customerOrders.some((order) => order.status === 'Ordered');
    });
  }

  GetAllOrders(){
    this.dataService.GetAllOrders().subscribe(result => {
      let Orderslist: any[] = result;
      Orderslist.forEach((element) => {
        this.allCustomerOrders.push(element);
        console.log(element);
      });
            // Set the noOrdersCancelledOrCollected variable based on the orders' status
            this.noOrdersCancelledOrCollected = !this.allCustomerOrders.some((order) => order.status === 'Collected' || order.status === 'Cancelled');
            this.noOrdersOrdered = !this.allCustomerOrders.some((order) => order.status === 'Ordered');
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




  OpenModal(order: any) {
    this.GetOrder(order.order_ID)
    this.showModal = true;
  }

  CloseModal() {
    this.showModal = false;
  }

  }

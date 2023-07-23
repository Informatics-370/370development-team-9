import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';

class Order {
  constructor(public order_id: number, public date: string, public status: string) {}
}

@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.scss']
})
export class CustomerOrdersComponent {

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  currentOrders: Order[] = [];
  orderHistory: Order[] = [];
  showModal: boolean = false;

  
  addOrder(order: Order) {
    this.currentOrders.push(order);
  }

  moveOrderToHistory(orderId: number) {
    const orderIndex = this.currentOrders.findIndex(order => order.order_id === orderId);
    if (orderIndex !== -1) {
      const order = this.currentOrders.splice(orderIndex, 1)[0];
      this.orderHistory.push(order);
    }
  }

  OpenModal() {
    this.showModal = true;
  }

  CloseModal() {
    this.showModal = false;
  }

}

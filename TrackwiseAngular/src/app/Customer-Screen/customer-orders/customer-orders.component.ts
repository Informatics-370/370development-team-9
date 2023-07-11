import { Component } from '@angular/core';

class Order {
  constructor(public order_id: number, public date: string, public status: string) {}
}

@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.scss']
})
export class CustomerOrdersComponent {
  currentOrders: Order[] = [];
  orderHistory: Order[] = [];

  
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
}

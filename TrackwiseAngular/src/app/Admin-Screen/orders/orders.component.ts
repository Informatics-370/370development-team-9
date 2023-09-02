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
  originalOrders: Order[] = []; // Property to store the original trailer data
  searchText: string = ''; // Property to store the search text

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

  async CollectOrder(order: any) {
    try {
      await this.dataService.CollectOrder(order.order_ID).toPromise();
      const updatedOrder = await this.dataService.GetOrder(order.order_ID).toPromise();

      // Check if updatedOrder is not undefined before accessing its properties
      if (updatedOrder?.status) {
        const index = this.customerOrders.findIndex((o) => o.order_ID === order.order_ID);
        if (index !== -1) {
          this.customerOrders[index].status = updatedOrder.status;
        }
      }
    } catch (error) {
      console.log('Error collecting order:', error);
    }
    this.allCustomerOrders = [];
    this.GetAllOrders();
    this.CloseModal();
  }


  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original product data
      this.orders = [...this.originalOrders];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the trailers based on the search text
      const filteredProducts = this.originalOrders.filter(order => {
        const order_ID = order.order_ID.toLowerCase();
        const Date = order.date;
        const status = order.status;
        const total = order.total;
        
        return (
          order_ID.includes(searchTextLower)||
          Date.toString().includes(searchTextLower)||
          status.includes(searchTextLower) || 
          total
        );
      });

      this.orders = filteredProducts;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
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

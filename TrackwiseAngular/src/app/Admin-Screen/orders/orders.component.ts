  import { Component } from '@angular/core';
import { CollectNotificationComponent } from 'src/app/ConfirmationNotifications/collect-notification/collect-notification.component';
  import { DataService } from 'src/app/services/data.service';
  import { Order } from 'src/app/shared/order';
  import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

  @Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
  })
  export class OrdersComponent {


constructor(private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar) { }

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

  openConfirmationDialog(order: any): void {
    const dialogRef = this.dialog.open(CollectNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { order }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.CollectOrder(order);
        this.snackBar.open(` Order Successfully Collected`, 'X', {duration: 3000});
      }
    });
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

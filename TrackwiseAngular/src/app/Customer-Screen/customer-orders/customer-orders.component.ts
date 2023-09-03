import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CancelNotificationComponent } from 'src/app/ConfirmationNotifications/cancel-notification/cancel-notification.component';
import { DataService } from 'src/app/services/data.service';
import { Order } from 'src/app/shared/order';


@Component({
  selector: 'app-customer-orders',
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.scss']
})
export class CustomerOrdersComponent {

  constructor(private dataService: DataService, private snackBar: MatSnackBar, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.dataService.revertToLogin();
    this.GetCustomerOrders();
  }

  noOrdersCancelledOrCollected: boolean = true;
  noOrdersOrdered: boolean = true;
  showModal: boolean = false;
  originalOrders: Order[] = []; // Property to store the original trailer data
  searchText: string = ''; // Property to store the search text

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
      this.originalOrders = [...custOrderslist]
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

  openConfirmationDialog(order : any): void {
    const dialogRef = this.dialog.open(CancelNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { order }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.CancelOrder(order);
        this.snackBar.open(`Order cancelled`, 'X', {duration: 3000});
      }
    });
  }

  async CancelOrder(order: any) {
    try {
      await this.dataService.CancelOrder(order.order_ID).toPromise();
      const updatedOrder = await this.dataService.GetOrder(order.order_ID).toPromise();
      // Check if updatedOrder is not undefined before accessing its properties
      this.customerOrders=[]
      this.GetCustomerOrders()
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

  // flipCard(product: Product): void {
  //   product.this.cardFlipped = !product.cardFlipped;
  // }

 }

 

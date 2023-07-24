import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
  searchText: string = ''; // Property to store the search text
  customer: any[] = []; // Property to store the admin data
  originalCustomers: any[] = []; // Property to store the original admin data
  
  constructor(private dataService: DataService) { }
  
  ngOnInit(): void {
    this.GetCustomers();
    this.dataService.revertToLogin();
  }
  
  GetCustomers() {
    this.dataService.GetCustomers().subscribe(result => {
      let customerList: any[] = result;
      this.originalCustomers = [...customerList]; // Store a copy of the original admin data
      customerList.forEach((element) => {
        this.customer.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original admin data
      this.customer = [...this.originalCustomers];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the admins based on the search text
      const filteredAdmins = this.originalCustomers.filter(cust => {
        const fullName = cust.name.toLowerCase() + ' ' + cust.lastName.toLowerCase();
        const email = cust.email.toLowerCase();
        const password = cust.password.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          cust.lastName.toLowerCase().includes(searchTextLower) ||
          email.includes(searchTextLower) || password.includes(searchTextLower)
          
        );
      });

      // Update the admins array with the filtered results
      this.customer = filteredAdmins;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  DeleteCustomer(admin_ID:string)
  {
    this.dataService.DeleteCustomer(admin_ID).subscribe({
      next: (response) => location.reload()
    })
  }
}

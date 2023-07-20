import { Component , OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService} from 'src/app/services/data.service';
import { Client } from 'src/app/shared/client';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.scss']
})

export class ClientsComponent implements OnInit {
  searchText: string = ''; // Property to store the search text
  clients: any[] = []; // Property to store the client data
  originalClients: any[] = []; // Property to store the original client data
  
  constructor(private dataService: DataService) { }
  
  ngOnInit(): void {
    this.GetClients();
  }

  GetClients() {
    this.dataService.GetClients().subscribe(result => {
      let clientList: any[] = result;
      this.originalClients = [...clientList]; // Store a copy of the original client data
      clientList.forEach((element) => {
        this.clients.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original client data
      this.clients = [...this.originalClients];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the clients based on the search text
      const filteredClients = this.originalClients.filter(client => {
        const fullName = client.name.toLowerCase();
        const phoneNumber = client.phoneNumber.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          phoneNumber.includes(searchTextLower)
         
        );
      });

      // Update the clients array with the filtered results
      this.clients = filteredClients;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  DeleteClient(client_ID:string)
  {
    this.dataService.DeleteClient(client_ID).subscribe({
      next: (response) => location.reload()
    })
  }

}

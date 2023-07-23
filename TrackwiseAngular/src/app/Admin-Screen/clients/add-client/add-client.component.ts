import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Client } from 'src/app/shared/client';

@Component({
  selector: 'app-add-client',
  templateUrl: './add-client.component.html',
  styleUrls: ['./add-client.component.scss']
})
export class AddClientComponent {

  clientDetails: Client =
  {
    client_ID:"0",
    name:"",
    phoneNumber:"",
    
  };

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddClient()
  {
    this.dataService.AddClient(this.clientDetails).subscribe({
      next: (client) => {this.router.navigate(['/Admin-Screen/clients'])}
    })
  }
}

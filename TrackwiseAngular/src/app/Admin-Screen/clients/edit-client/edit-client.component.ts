import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Client } from 'src/app/shared/client';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss']
})
export class EditClientComponent implements OnInit {

  clientDetails: Client =
  {
    client_ID:0,
    name:"",
    lastname:"",
    phoneNumber:"",
    
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetClient(params['client_ID']).subscribe({
            next: (response) => {
              this.clientDetails = response;
            }
          })

      }
    })
  }

  EditClient()
  {    
    this.dataService.EditClient(this.clientDetails.client_ID, this.clientDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/clients'])}
    })
    console.log('yes')
  }
}

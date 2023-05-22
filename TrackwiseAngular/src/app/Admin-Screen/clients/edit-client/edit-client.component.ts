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

  EditClientReq: Client =
  {
    client_ID:0,
    name:'',
    phoneNumber:'',
    
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetClient(params['client_ID']).subscribe({
            next: (response) => {
              this.EditClientReq = response;
            }
          })

      }
    })
  }

  EditClient()
  {    
    this.dataService.EditClient(this.EditClientReq.client_ID, this.EditClientReq).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/clients'])}
    })
  }
}

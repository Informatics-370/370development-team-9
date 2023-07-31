import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Client } from 'src/app/shared/client';

@Component({
  selector: 'app-edit-client',
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss']
})
export class EditClientComponent implements OnInit {

  clientDetails: Client = {
    client_ID: "0",
    name: "",
    phoneNumber: "",
    email:"",
    password:"",
  };
  originalPhoneNumber = '';

  constructor(private route: ActivatedRoute, private dataService: DataService, private router: Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {
        this.dataService.GetClient(params['client_ID']).subscribe({
          next: (response) => {
            this.clientDetails = response;
          }
        });
      }
    });

    this.dataService.revertToLogin();
  }

  KeepZeros() {
    if (this.clientDetails.phoneNumber) {
      this.originalPhoneNumber = this.clientDetails.phoneNumber.toString();
    }
  }

  EditClient() {
    this.dataService.EditClient(this.clientDetails.client_ID, this.clientDetails).subscribe({
      next: (response) => {
        this.router.navigate(['/Admin-Screen/clients']);
        this.snackBar.open(`Client successfully edited`, 'X', {duration: 3000});
      }
    });
  }
}

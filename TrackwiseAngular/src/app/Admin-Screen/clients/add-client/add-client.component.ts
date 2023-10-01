import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Client } from 'src/app/shared/client';
import { MatSnackBar } from '@angular/material/snack-bar';

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
    email:"",
    password:"",
    
  };
  showHelpModal: boolean = false;

  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddClient()
  {
    this.dataService.AddClient(this.clientDetails).subscribe({
      next: (client) => {this.router.navigate(['/Admin-Screen/clients']);
      this.snackBar.open(this.clientDetails.name + ` successfully registered`, 'X', {duration: 3000});
    }
    })
  }
  OpenHelpModal() {
    this.showHelpModal = true;
    document.body.style.overflow = 'hidden';
  }

  CloseHelpModal() {
    this.showHelpModal = false;
    document.body.style.overflow = 'auto';
  }

}

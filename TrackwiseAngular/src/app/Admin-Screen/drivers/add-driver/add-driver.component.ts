import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Driver } from 'src/app/shared/driver';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.scss']
})
export class AddDriverComponent {

  driverDetails: Driver =
  {
    driver_ID:"0",
    name:"",
    lastname:"",
    phoneNumber:"",
    email:"",
    password:"",
    driver_Status_ID:"",
    driverStatus:{
      driver_Status_ID:"",
      status:"",
      description:""
    },
  };
  showHelpModal: boolean = false;

  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddDriver()
  {
    this.dataService.AddDriver(this.driverDetails).subscribe({
      next: (driver) => {this.router.navigate(['/Admin-Screen/drivers']);
      this.snackBar.open(this.driverDetails.name + ` successfully registered`, 'X', {duration: 3000});
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
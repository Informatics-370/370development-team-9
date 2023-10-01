import { Component, OnInit } from '@angular/core';
import { Truck } from 'src/app/shared/truck';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-trucks',
  templateUrl: './add-trucks.component.html',
  styleUrls: ['./add-trucks.component.scss']
})
export class AddTrucksComponent {
  AddTruckRequest: Truck =
  {
    truckID:"",
    truck_License:"",
    model:"",
    truck_Status_ID:"",
    truckStatus:{
      truck_Status_ID:"",
      status:"",
      description:""
    },
  };
  showHelpModal: boolean = false;

  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddTruck()
  {
    this.dataService.AddTruck(this.AddTruckRequest).subscribe({
      next: (truck) => {this.router.navigate(['/Admin-Screen/trucks']);
      this.snackBar.open(this.AddTruckRequest.truck_License +  ` successfully registered`, 'X', {duration: 3000});}
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

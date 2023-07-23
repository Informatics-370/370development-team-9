import { Component, OnInit } from '@angular/core';
import { Truck } from 'src/app/shared/truck';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';

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

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddTruck()
  {
    this.dataService.AddTruck(this.AddTruckRequest).subscribe({
      next: (truck) => {this.router.navigate(['/Admin-Screen/trucks'])}
    })
  }
}

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
    truckID:0,
    truck_License:"",
    model:"",
    truck_Status_ID:0,
    truckStatus:{
      truck_Status_ID:0,
      status:"",
      description:""
    },
  };

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  AddTruck()
  {
    this.dataService.AddTruck(this.AddTruckRequest).subscribe({
      next: (course) => {this.router.navigate(['/Admin-Screen/trucks'])}
    })
  }
}

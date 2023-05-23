import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Driver } from 'src/app/shared/driver';

@Component({
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.scss']
})
export class AddDriverComponent {

  driverDetails: Driver =
  {
    driver_ID:0,
    name:"",
    lastname:"",
    phoneNumber:"",
    driver_Status_ID:1,
    driverStatus:{
      driver_Status_ID:0,
      status:"",
      description:""
    },
  };

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  AddDriver()
  {
    this.dataService.AddDriver(this.driverDetails).subscribe({
      next: (driver) => {this.router.navigate(['/Admin-Screen/drivers'])}
    })
  }
}
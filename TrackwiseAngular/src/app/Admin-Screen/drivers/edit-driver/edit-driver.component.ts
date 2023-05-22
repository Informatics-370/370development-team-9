import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Driver } from 'src/app/shared/driver';

@Component({
  selector: 'app-edit-driver',
  templateUrl: './edit-driver.component.html',
  styleUrls: ['./edit-driver.component.scss']
})
export class EditDriverComponent implements OnInit{

  driverDetails: Driver =
  {
    driver_ID:0,
    name:"",
    lastname:"",
    phoneNumber:"",
    driver_Status_ID:0,
    driverStatus:{
      driver_Status_ID:0,
      status:"",
      description:""
    },
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetDriver(params['driver_ID']).subscribe({
            next: (response) => {
              this.driverDetails = response;
            }
          })

      }
    })
  }

  EditDriver()
  {    
    this.dataService.EditDriver(this.driverDetails.driver_ID, this.driverDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/drivers'])}
    })
    console.log('yes')
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Truck } from 'src/app/shared/truck';

@Component({
  selector: 'app-edit-truck',
  templateUrl: './edit-truck.component.html',
  styleUrls: ['./edit-truck.component.scss']
})



export class EditTruckComponent implements OnInit{

  truckDetails: Truck =
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

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    // this.route.paramMap.subscribe({
    //   next: (params) => { const truckID = params.get('truckID');
    
    //   if(truckID)
    //   {
    //       this.dataService.GetTruck(truckID).subscribe({
    //         next: (response) => {this.truckDetails = response;}
    //       })
    //   }
    // }
    // })

    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetTruck(params['truckID']).subscribe({
            next: (response) => {
              this.truckDetails = response;
            }
          })

      }
    })
  }

  EditTruck()
  {    
    this.dataService.EditTruck(this.truckDetails.truckID, this.truckDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/trucks'])}
    })
    console.log('yes')
  }
}

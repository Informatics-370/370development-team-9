import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
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
    truckID:"",
    truck_License:"",
    model:"",
    mileage:0,
    truck_Status_ID:"",
    truckStatus:{
      truck_Status_ID:"",
      status:"",
      description:""
    },
  };

  showHelpModal: boolean = false;

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {

    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetTruck(params['truckID']).subscribe({
            next: (response) => {
              this.truckDetails = response;
            }
          })

      }
    })

    this.dataService.revertToLogin();
  }

  EditTruck()
  {    
    this.dataService.EditTruck(this.truckDetails.truckID, this.truckDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/trucks']);
      this.snackBar.open(`Truck successfully edited`, 'X', {duration: 3000});}
    })
    console.log('yes')
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

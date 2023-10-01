import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
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
    driver_ID:"",
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

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

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

    this.dataService.revertToLogin();
  }

  EditDriver()
  {    
    this.dataService.EditDriver(this.driverDetails.driver_ID, this.driverDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/drivers']);
      this.snackBar.open(`Driver successfully edited`, 'X', {duration: 3000});
    }
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

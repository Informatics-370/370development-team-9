import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Admin } from 'src/app/shared/admin';
import {Document} from 'src/app/shared/document';
import { MileageFuel } from 'src/app/shared/mileage_fuel';
import { Weight } from 'src/app/shared/weight';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Fuel } from 'src/app/shared/fuel';
import { Delivery } from 'src/app/shared/delivery';
import { Truck } from 'src/app/shared/truck';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-document-information',
  templateUrl: './document-information.component.html',
  styleUrls: ['./document-information.component.scss']
})
export class DocumentInformationComponent implements OnInit{
  showWB: boolean = true;
  showFS: boolean = false;
  showM: boolean = false;

  delivery_ID: string ="";
  hasWeighBridgeDocuments: boolean = false;
  hasFuelDocuments: boolean = false;
  hasMileageDocuments: boolean = false;
  documents: any[] = [];
  mileage: number = 0;
  truck: Truck =
  {
    truckID: '',
    truck_License: '',
    model: '',
    mileage:0,
    truck_Status_ID: '',
    truckStatus: {
      truck_Status_ID: '',
      status: '',
      description: ''
    }
  }


   constructor(private dataService: DataService, private router:Router ,private route: ActivatedRoute, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.delivery_ID = params['delivery_ID'];
      this.mileage = params['mileage'];
      console.log(this.mileage);
      
      this.loadDocuments(this.delivery_ID);
    });
    this.showWB=true;
    this.showFS=false;
    this.showM=false;
  }

  loadDocuments(delivery_ID : string) {
    this.dataService.GetDocuments(delivery_ID).subscribe(response=>{
      
        let custOrderslist: any[] = response;
        custOrderslist.forEach((element) => {
          this.documents.push(element);
          if (element.docType === 'Weight') {
            this.hasWeighBridgeDocuments = true;
        } else if(element.docType === 'Fuel'){
            this.hasFuelDocuments = true;
        } else if(element.docType === 'Mileage'){
            this.hasMileageDocuments = true;
        }
          console.log(element);
        });
        console.log(this.documents);
      
    });
  }

  request: Weight = {
    actual_Weight: 0,
  };

  updateActualWeight(): void {
    this.dataService.UpdateActualWeight(this.delivery_ID, this.request).subscribe(
      response => {
        console.log(response); // Handle success, show a message, etc.
        this.snackBar.open(` Weight Updated`, 'X', {duration: 3000});
      },
      error => {
        console.error(error); // Handle error, show an error message, etc.
      }
    );
  }

  MileageRequest: MileageFuel = {
    initial_Mileage: 0,
    final_Mileage: 0,
    total_Fuel:0,

  delivery_ID: '',
  mileage: 0,
     fuel: 0

  };

  fuelRequest: Fuel = {
    total_Fuel:0,
  };

  updateFuel(): void {
    this.dataService.EditFuel(this.delivery_ID, this.fuelRequest).subscribe(
      response => {
        console.log(response); // Handle success, show a message, etc.
        this.snackBar.open(` Fuel Updated`, 'X', {duration: 3000});
      },
      error => {
        console.error(error); // Handle error, show an error message, etc.
      }
    );
  }

  updateMileageFuel(): void {
      if(this.MileageRequest.initial_Mileage < this.mileage || this.MileageRequest.final_Mileage < this.mileage) {
        this.snackBar.open(` Truck Milage is greater than inserted milage`, 'X', {duration: 3000});
      }
      else if (this.MileageRequest.initial_Mileage > this.MileageRequest.final_Mileage)
      {
        this.snackBar.open(` Initial Mileage cannot be greater that final milage`, 'X', {duration: 3000});
      }else{
        this.dataService.EditMileageFuel(this.delivery_ID, this.MileageRequest).subscribe(
          response => {
            console.log(response); // Handle success, show a message, etc.
            this.snackBar.open(` Mileage Updated`, 'X', {duration: 3000});
          },
          error => {
            console.error(error); // Handle error, show an error message, etc.
          }
        );
      }
  }

  removeNegativeSign() {
    if (this.MileageRequest.initial_Mileage < 0) {
      this.MileageRequest.initial_Mileage = 0;
    }

    if (this.MileageRequest.final_Mileage < 0) {
      this.MileageRequest.final_Mileage = 0;
    }

    if (this.fuelRequest.total_Fuel < 0) {
      this.fuelRequest.total_Fuel = 0;
    }

    if (this.request.actual_Weight < 0) {
      this.request.actual_Weight = 0;
    }
  }

  ShowWB() {
    this.showWB = true;
    this.showFS = false;
    this.showM = false;
  }

  ShowFS() {
    this.showWB = false;
    this.showFS = true;
    this.showM = false;
  }
  ShowM() {
    this.showWB = false;
    this.showFS = false;
    this.showM = true;
  }
}

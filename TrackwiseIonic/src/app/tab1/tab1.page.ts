import { Component, OnInit } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { ExploreContainerComponent } from '../explore-container/explore-container.component';
import { Router } from '@angular/router';
import { DataserviceService } from '../services/dataservice.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import {addDocument} from '../shared/Document';
import { Delivery } from '../shared/Delivery';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss'],
  standalone: true,
  imports: [IonicModule, ExploreContainerComponent,HttpClientModule,NgFor, NgIf, DatePipe],
  providers:[DataserviceService, DatePipe],
})
export class Tab1Page implements OnInit{


  docrequest: addDocument =
  {
      documents: []
  };

  constructor(public dataService: DataserviceService, private router: Router,private datePipe: DatePipe,private http: HttpClient) {}
  
  ngOnInit() { // Implement ngOnInit lifecycle hook
    this.GetDriverDeliveries(); // Call the correct method to get driver deliveries
  }

  GetDriverDeliveries() {
    this.dataService.GetDriverDeliveries().subscribe(result => {
      this.deliveries = result;

      console.log(result)
    });
  }

  RouteDoc(delivery_ID : string){
    this.router.navigate(['/tabs/tabs/tab2', delivery_ID]);
   }

  // AddDoc() {
  //   this.dataService.AddDoc(this.docrequest).subscribe({
  //     next: (result) => {
  //       console.log(result);
  //       // Clear the document request after successful addition
  //       this.docrequest.documents = [];
  //     },
  //     error: (error) => {
  //       console.error(error);
  //     },
  //   });
  // }

  // Complete(deliveryId: string, delivery:any) {
  //   this.AddDoc();
  //   this.dataService.updateDeliveryStatus(deliveryId).subscribe(
  //     () => {
  //       // Find the index of the delivery with the given deliveryId in the array
  //       const index = this.deliveries.findIndex((d) => d.delivery_ID === deliveryId);
  //       console.log(index);
  //       if (index != -1) {
  //         if(this.deliveries.length == 1){
  //           this.dataService.updateJobStatus(delivery.jobs.job_ID);
  //           console.log(delivery.job_ID);
  //           this.dataService.updateDriverStatus(delivery.driver_ID);
  //           this.dataService.updateTrailerStatus(delivery.trailerID);
  //           this.dataService.updateTruckStatus(delivery.truckID);
  //         }
  //         // Update the Delivery_Status_ID of the found delivery to '2'
  //         this.deliveries[index].delivery_Status_ID = '2';
  //         this.deliveries[index].job_Status_ID = "2";
  //         this.deliveries = [];
  //         this.GetDriverDeliveries();
  //         console.log('Delivery status updated successfully.');
  //       } else {
  //         // Handle the case where the delivery with the given ID was not found in the array
  //         console.error('Delivery not found with ID:', deliveryId);
  //       }
  //     },
  //     (error) => {
  //       console.error('Error updating delivery status:', error);
  //       // Handle error (e.g., show an error message)
  //     }
  //   );
  // }
  
  // formData = new FormData();
  // fileNameUploaded = '';

  // uploadFile = (files: any, deliveries : any) => {
  //   let fileToUpload = <File>files[0];
  //   this.formData.append('file', fileToUpload, fileToUpload.name);
  //   const reader = new FileReader();
  //   reader.onload = (e: any) => {
  //     const document: {
  //       document_ID: string;
  //       image: string;
  //       delivery_ID: string;
  //     } = {
  //       document_ID: '', // Set appropriate document_ID if needed
  //       image: e.target.result,
  //       delivery_ID: deliveries.delivery_ID, // Set the delivery_ID that corresponds to the delivery you want to associate this document with
  //     };
  //     console.log(document)
  //     this.docrequest.documents.push(document);
  //   };
  //   reader.readAsDataURL(fileToUpload);
  //   this.fileNameUploaded = fileToUpload.name;
  // };

}

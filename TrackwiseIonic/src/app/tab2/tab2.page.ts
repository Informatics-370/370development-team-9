import { Component, NgModule,OnInit } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { ExploreContainerComponent } from '../explore-container/explore-container.component';
import { NgFor, NgIf } from '@angular/common';
import { NgModel } from '@angular/forms';
import { addDocument } from '../shared/Document';
import { Delivery } from '../shared/Delivery';
import { DataserviceService } from '../services/dataservice.service';
import { HttpClient,HttpClientModule } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

interface Files extends File {
  docType:string
}

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss'],
  standalone: true,
  imports: [IonicModule, ExploreContainerComponent, NgIf, NgFor, HttpClientModule],
  providers:[DataserviceService]
})

export class Tab2Page{
  weightRows: any[] = [];
  milageRows: any[] = [];
  fuelRows: any[] = [];
  selectedFiles: Files[] = [];
  

  docrequest: addDocument =
  {
      documents: []
  };

  constructor(private route: ActivatedRoute, private dataService : DataserviceService, private http: HttpClient) {  }

  addWeightRow() {
    this.weightRows.push({ showUploadOptions: true });
  }

  addMileageRow() {
    this.milageRows.push({ showUploadOptions: true });
  }

  addFuelRow() {
    this.fuelRows.push({ showUploadOptions: true });
  }

  // uploadFile(files: FileList | null) {
  //   if (files && files.length > 0) {
  //   }
  // }

  AddDoc() {
    //console.log(this.docrequest)
    this.dataService.AddDoc(this.docrequest).subscribe({
      next: (result) => {
        console.log(result);
        // Clear the document request after successful addition
        this.docrequest.documents = [];
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  addToSelectedFiles(files: any, docType:string) {
    let fileToUpload = <Files>files[0];
    fileToUpload.docType = docType;
    this.selectedFiles.push(fileToUpload);
    console.log(fileToUpload)
  }

  async uploadSelectedFiles() {
    // Loop through selected files and upload them
    for (const file of this.selectedFiles) {
      await this.uploadFile(file);
    }
  
    this.AddDoc();
    // Clear selected files and update docrequest if needed
    this.selectedFiles = [];
    // if (this.docrequest.documents.length > 0) {
    //   // Perform any necessary action with docrequest, e.g., sending it to the server
    //   console.log(this.docrequest);
    //   // Reset docrequest for the next batch of uploads
    //   this.docrequest.documents = [];
    // }
  }

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
  
  formData = new FormData();
  fileNameUploaded = '';

  uploadFile = (files: any) => {
    this.route.params.subscribe({
      next: (params) => {
        let deliveryID = params['delivery_ID'];
  
        this.formData.append('file', files, files.name);
        const reader = new FileReader();
        reader.onload = (e: any) => {
          const document: {
            document_ID: string;
            image: string;
            delivery_ID: string;
            docType:string;
          } = {
            document_ID: '', // Set appropriate document_ID if needed
            image: e.target.result,
            delivery_ID: deliveryID, // Set the delivery_ID that corresponds to the delivery you want to associate this document with
            docType: files.docType,
          };
          console.log(document);
          this.docrequest.documents.push(document);
          console.log( this.docrequest.documents)
        };
        reader.readAsDataURL(files);
        this.fileNameUploaded = files.name;
      }
    });
  }
  

}

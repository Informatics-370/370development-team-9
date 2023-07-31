import { Component, OnInit } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { ExploreContainerComponent } from '../explore-container/explore-container.component';
import { Router } from '@angular/router';
import { DataserviceService } from '../services/dataservice.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { DatePipe, NgFor, NgIf } from '@angular/common';
import {addDocument} from '../shared/Document';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss'],
  standalone: true,
  imports: [IonicModule, ExploreContainerComponent,HttpClientModule,NgFor, NgIf, DatePipe],
  providers:[DataserviceService, DatePipe],
})
export class Tab1Page implements OnInit{
  deliveries: any[] = []; // Array to store the deliveries

  docrequest: addDocument =
  {
      documents: []
  };

  constructor(private dataService: DataserviceService, private router: Router,private datePipe: DatePipe,private http: HttpClient) {}
  
  ngOnInit() { // Implement ngOnInit lifecycle hook
    this.GetDriverDeliveries(); // Call the correct method to get driver deliveries
  }

  GetDriverDeliveries() {
    this.dataService.GetDriverDeliveries().subscribe(result => {
      this.deliveries = result;
      console.log(result)
    });
  }

  AddDoc() {
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

  Complete(delivery : any, fileNameUploaded : any){
      delivery.delivery_Status_ID = "2";
      this.deliveries = delivery;
      console.log(this.deliveries)
  }
  
  formData = new FormData();
  fileNameUploaded = '';

  uploadFile = (files: any, deliveries : any) => {
    let fileToUpload = <File>files[0];
    this.formData.append('file', fileToUpload, fileToUpload.name);
    const reader = new FileReader();
    reader.onload = (e: any) => {
      const document: {
        document_ID: string;
        image: string;
        delivery_ID: string;
      } = {
        document_ID: '', // Set appropriate document_ID if needed
        image: e.target.result,
        delivery_ID: deliveries.delivery_ID, // Set the delivery_ID that corresponds to the delivery you want to associate this document with
      };
      console.log(document)
      this.docrequest.documents.push(document);
    };
    reader.readAsDataURL(fileToUpload);
    this.fileNameUploaded = fileToUpload.name;
  };

}

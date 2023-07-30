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
    document_ID: '',
    image: '',    
    delivery:{
      delivery_ID:'',
    }
  };

  constructor(private dataService: DataserviceService, private router: Router,private datePipe: DatePipe,private http: HttpClient) {}
  
  ngOnInit() { // Implement ngOnInit lifecycle hook
    this.GetDriverDeliveries(); // Call the correct method to get driver deliveries
  }

  GetDriverDeliveries() {
    this.dataService.GetDriverDeliveries().subscribe(result => {
      this.deliveries = result;
    });
  }

  AddDoc() {
    this.dataService.AddDoc(this.docrequest).subscribe({
      next: (result) => {
        console.log(result); 
      }
    });
  }
  
  formData = new FormData();
  fileNameUploaded = ''
  uploadFile = (files: any) => {
    let fileToUpload = <File>files[0];
    this.formData.append('file', fileToUpload, fileToUpload.name);
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.docrequest.image = e.target.result; // Assign the base64 string to the image property
    };
    reader.readAsDataURL(fileToUpload);
    this.fileNameUploaded = fileToUpload.name
  }

}

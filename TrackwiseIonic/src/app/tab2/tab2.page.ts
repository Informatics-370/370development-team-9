import { Component, NgModule,OnInit } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { ExploreContainerComponent } from '../explore-container/explore-container.component';
import { NgFor, NgIf } from '@angular/common';
import { NgModel } from '@angular/forms';
import { addDocument } from '../shared/Document';
import { Delivery } from '../shared/Delivery';
import { DataserviceService } from '../services/dataservice.service';
import { HttpClient,HttpClientModule } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { Camera, CameraResultType, CameraSource} from '@capacitor/camera';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

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
  selectedPhotos: addDocument['documents'] = [];

  photo: any;
  

  docrequest: addDocument =
  {
      documents: []
  };

  constructor(private route: ActivatedRoute, private dataService : DataserviceService, private http: HttpClient, private router: Router, private sanitizer: DomSanitizer ) {  }

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
    console.log(this.docrequest.documents)
    this.dataService.AddDoc(this.docrequest).subscribe({
      next: (result) => {
        //console.log(result);
        // Clear the document request after successful addition
        this.docrequest.documents = [];
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  addToSelectedFiles(files: any, docType:string) {
    console.log(files)
    let fileToUpload = <Files>files[0];
    fileToUpload.docType = docType;
    this.selectedFiles.push(fileToUpload);
    console.log(fileToUpload)
  }

  addSelectedPhotos(imageurl: string, docType:string) {
    this.route.params.subscribe({
      next: (params) => {
        let deliveryID = params['delivery_ID'];
    const document: {
      document_ID: string;
      image: string;
      delivery_ID: string;
      docType:string;
    } = {
      document_ID: '', // Set appropriate document_ID if needed
      image: imageurl,
      delivery_ID: deliveryID, // Set the delivery_ID that corresponds to the delivery you want to associate this document with
      docType: docType,
    };
    this.docrequest.documents.push(document);
    console.log(document)
  }
  })
  }

  uploadPhotoDoc(){

  }

  async uploadSelectedFiles() {
    const promises = this.selectedFiles.map(file => this.uploadFile(file));
    await Promise.all(promises);
  
    await this.AddDoc();
  
    // Clear selected files and update docrequest if needed
    this.selectedFiles = [];
    this.router.navigate(['/tabs/tabs/tab1']);
  }

  async takeWeightPicture(){
    const image = await Camera.getPhoto({
      quality: 100,
      allowEditing: false,
      resultType: CameraResultType.DataUrl,
      source: CameraSource.Camera
    });

    this.photo = image.dataUrl;
    console.log(this.photo)

    this.addSelectedPhotos(this.photo, "Weight")
  }

  async takeMileagePicture(){
    const image = await Camera.getPhoto({
      quality: 100,
      allowEditing: false,
      resultType: CameraResultType.DataUrl,
      source: CameraSource.Camera
    });

    this.photo = image.dataUrl;

    this.addSelectedPhotos(this.photo, "Mileage")
  }

  async takeFuelPicture(){
    const image = await Camera.getPhoto({
      quality: 100,
      allowEditing: false,
      resultType: CameraResultType.DataUrl,
      source: CameraSource.Camera
    });

    this.photo = image.dataUrl;

    this.addSelectedPhotos(this.photo, "Fuel")
  }
  
  formData = new FormData();
  fileNameUploaded = '';

  uploadFile = (files: any) => {
    return new Promise<void>((resolve, reject) => {
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
          //console.log(document);
          this.docrequest.documents.push(document);
          resolve();
          //console.log( this.docrequest.documents)
        };
        reader.readAsDataURL(files);
        this.fileNameUploaded = files.name;
      }
    });
  });
  }
  

}

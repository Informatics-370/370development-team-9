import { Component, NgModule,OnInit } from '@angular/core';
import { IonicModule } from '@ionic/angular';
import { ExploreContainerComponent } from '../explore-container/explore-container.component';
import { NgFor, NgIf } from '@angular/common';
import { NgModel } from '@angular/forms';
import { addDocument } from '../shared/Document';
import { Delivery } from '../shared/Delivery';
import { DataserviceService } from '../services/dataservice.service';
import { HttpClient,HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss'],
  standalone: true,
  imports: [IonicModule, ExploreContainerComponent, NgIf, NgFor, HttpClientModule],
  providers:[DataserviceService]
})
export class Tab2Page{
  rows: any[] = [];
  docrequest: addDocument =
  {
      documents: []
  };
  
  constructor(private dataService : DataserviceService, private http: HttpClient) {  }

  addRow() {
    this.rows.push({ showUploadOptions: true });
  }

  uploadFile(files: FileList | null) {
    if (files && files.length > 0) {
    }
  }

}

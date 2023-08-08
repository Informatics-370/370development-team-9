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
  deliveries: Delivery[] = []; // Array to store the deliveries

  constructor(private dataService: DataserviceService, private router: Router,private datePipe: DatePipe,private http: HttpClient) {}
  
  ngOnInit() { // Implement ngOnInit lifecycle hook
    this.GetDriverDeliveries(); // Call the correct method to get driver deliveries
  }

  RouteDoc(){
   this.router.navigate(['/tabs/tabs/tab2']);
  }

  GetDriverDeliveries() {
    this.dataService.GetDriverDeliveries().subscribe(result => {
      this.deliveries = result;
      console.log(result);
    });
  }
  
}
